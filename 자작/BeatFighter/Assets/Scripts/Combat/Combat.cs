using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Singleton<Combat>
{
    public static Action onMapSet;
    public static Action onMapEnd;
    public static Action onStageSet;
    public static Action onStageStart;
    public static Action onStageEnd;

    [Header("References")]
    public Transform combatFieldTM;
    public Transform cameraPoint;
    public VitalSign vitalSign;
    public HPGroup hpGroup;
    public SkillGroup skillGroup;

    [Header("Character Setting")]
    public CharSetting playerSetting;
    public List<CharSetting> mobSettings;

    private int mapID;
    private List<int> stageIDs;
    private Field field;
    private Targeting targeting;

    public PlayerChar player { get; private set; }
    public MobChar[] mobs { get; private set; } = new MobChar[5];
    [NonSerialized]
    public int mobCount;

    protected override void Awake()
    {
        base.Awake();
        onMapSet += CreateTargetParticle;
        onStageSet += StartCurrentStage;
        BaseChar.onMobDeath += MobDeath;
        
        vitalSign.Initialize();
        hpGroup.Initialize();
        skillGroup.Initialize();
    }

    /// <summary>
    /// 맵 세팅
    /// </summary>
    /// <param name="mapID"></param>
    public void SetMap(int mapID)
    {
        this.mapID = mapID;
        stageIDs = new List<int>();
        
        MapInfo mapInfo = TableData.instance.mapDataDic[mapID];
        if (mapInfo == null)
        {
            Debug.LogError("존재하지 않는 맵");
            return;
        }
        
        for (int i = 0; i < mapInfo.stageList.Count; i++)
        {
            stageIDs.Add(mapInfo.stageList[i]);
        }

        //int currentStage = GetAndRemoveCurrentStage();
        //if (currentStage == -1)
        //{
        //    Debug.LogError("현재 맵에 스테이지가 존재하지 않음");
        //    return;
        //}
        
        field = PoolingManager.Instance.Spawn<Field>(mapInfo.fieldID, combatFieldTM);
        player = playerSetting.SetPlayer(PlayerData.currentChar);
        vitalSign.SetPlayer(player);
        onMapSet();
        //SetStage(currentStage);
        GotoNextStage(0);
    }

    /// <summary>
    /// 스테이지 세팅
    /// </summary>
    /// <param name="stageID"></param>
    private void SetStage(int stageID)
    {
        mobCount = 0;
        StageInfo stageInfo = TableData.instance.stageDataDic[stageID];
        if (stageInfo == null)
        {
            Debug.LogError("존재하지 않는 스테이지");
            return;
        }

        foreach (var pair in stageInfo.mobList)
        {
            mobs[pair.Key] = mobSettings[pair.Key].SetMob(pair.Value[0], pair.Value[1]);
            mobs[pair.Key].SetTarget(player);
            mobCount += 1;
        }

        player.SetTarget(GetRandomMob());
        onStageSet();
    }

    /// <summary>
    /// 현재 스테이지 시작
    /// </summary>
    public void StartCurrentStage()
    {
        StartCoroutine(StartCurrentStageRoutine());
    }

    private IEnumerator StartCurrentStageRoutine()
    {
        yield return new WaitForSeconds(1);
        onStageStart();
        for (int i = 0; i < mobs.Length; i++)
        {
            if (Targetable(mobs[i])) mobs[i].StartCombat();
        }
    }

    /// <summary>
    /// 다음 스테이지로 이동
    /// </summary>
    public void GotoNextStage(float time)
    {
        if (mobCount != 0) return;

        StartCoroutine(NextStageRoutine(time));
    }

    private IEnumerator NextStageRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        int currentStage = GetAndRemoveCurrentStage();
        if (currentStage != -1)
        {
            player.Play("Run");
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + Vector3.right * 20;
            float progress = 0;
            while (true)
            {
                if (transform.position == endPos) break;
                progress += Time.deltaTime * 0.5f;
                transform.position = Vector3.Lerp(startPos, endPos, progress);
                yield return null;
            }
            player.CrossFade("Idle");
            SetStage(currentStage);
        }
        else onMapEnd();
    }

    /// <summary>
    /// 현재 스테이지를 반환하고 리스트에서 삭제
    /// </summary>
    private int GetAndRemoveCurrentStage()
    {
        int currentStage = -1;
        
        if (stageIDs.Count > 0)
        {
            currentStage = stageIDs[0];
            stageIDs.RemoveAt(0);
        }
        
        return currentStage;
    }

    /// <summary>
    /// 타겟팅 표시 생성
    /// </summary>
    private void CreateTargetParticle()
    {
        targeting = PoolingManager.Instance.Spawn<Targeting>(PlayerData.targetingParticle, transform);
        targeting.SetActive(false);
    }

    /// <summary>
    /// 데미지 파티클 생성
    /// </summary>
    /// <param name="info"></param>
    public void CreateDmgParticle(DamageInfo info)
    {
        HPParticle particle = PoolingManager.Instance.Spawn<HPParticle>(PlayerData.HPParticle);
        particle.transform.position = info.to.transform.position;
        particle.SetMesh(info);
    }

    /// <summary>
    /// 타겟팅 표시 설정
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(BaseChar target)
    {
        targeting.transform.position = target.transform.position + Vector3.up * 0.1f;
    }

    /// <summary>
    /// 현재 살아있는 몬스터 중 랜덤으로 반환
    /// </summary>
    /// <returns></returns>
    private MobChar GetRandomMob()
    {
        List<int> mobIndexes = new List<int>();
        for (int i = 0; i < mobs.Length; i++)
        {
            if (Targetable(mobs[i])) mobIndexes.Add(i);
        }
        if (mobIndexes.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, mobIndexes.Count);
        return mobs[mobIndexes[index]];
    }

    /// <summary>
    /// 몬스터가 죽는 순간 실행되는 함수
    /// </summary>
    public void MobDeath()
    {
        mobCount -= 1;
        if (mobCount != 0 && player.Target.isDead) player.SetTarget(GetRandomMob());
        else if (mobCount == 0)
        {
            onStageEnd();
            GotoNextStage(2);
        }
    }

    /// <summary>
    /// 대상을 타겟으로 삼을 수 있는지 판단
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool Targetable(BaseChar target)
    {
        return target != null && target.isActiveAndEnabled && !target.isDead;
    }
}
