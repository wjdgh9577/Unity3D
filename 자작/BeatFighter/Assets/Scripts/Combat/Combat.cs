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
    public ComboText comboText;
    public WarningScreen warningScreen;

    [Header("Character Setting")]
    public CharSetting playerSetting;
    public List<CharSetting> mobSettings;

    private MapInfo meta;
    private Queue<int> stageIDs;
    private Field field;
    private Targeting targeting;

    public PlayerChar player { get; private set; }
    public MobChar[] mobs { get; private set; } = new MobChar[5];
    [NonSerialized]
    public int mobCount;

    protected override void Awake()
    {
        base.Awake();
        BaseChar.onPlayerDeath += PlayerDeath;
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
        transform.position = Vector3.zero;

        meta = TableData.instance.mapDataDic[mapID];
        stageIDs = new Queue<int>();
        
        MapInfo mapInfo = TableData.instance.mapDataDic[mapID];
        if (mapInfo == null)
        {
            Debug.LogError("존재하지 않는 맵");
            return;
        }
        
        for (int i = 0; i < mapInfo.stageList.Count; i++)
        {
            stageIDs.Enqueue(mapInfo.stageList[i]);
        }
        
        field = PoolingManager.Instance.Spawn<Field>(mapInfo.fieldID, combatFieldTM);
        field.SetBossTrigger(stageIDs.Count);
        player = playerSetting.SetPlayer(PlayerData.currentChar);
        vitalSign.SetPlayer(player);
        GameManager.Instance.ChangeCam(true);
        CreateTargetParticle();
        onMapSet();
        AudioManager.Instance.PlayBattle(meta.fieldID);

        GotoNextStage(0);
    }

    /// <summary>
    /// 스테이지 세팅
    /// </summary>
    /// <param name="stageID"></param>
    private void SetStage(int stageID)
    {
        mobCount = 0;
        for (int i = 0; i < mobs.Length; i++) mobs[i] = null;

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
        StartCurrentStage();
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
        int nextStage = GetAndRemoveCurrentStage();

        if (nextStage != -1) StartCoroutine(EndStageCoroutine(nextStage, time));
        else EndMap(time);
    }

    private IEnumerator EndStageCoroutine(int nextStage, float time)
    {
        yield return new WaitForSeconds(time);

        if (Targetable(player))
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
            SetStage(nextStage);
        }
    }

    /// <summary>
    /// 모든 스테이지 클리어 또는 플레이어 사망시 맵 종료
    /// </summary>
    /// <param name="time"></param>
    private void EndMap(float time)
    {
        AudioManager.Instance.Stop();
        StartCoroutine(EndMapCoroutine(time));
    }

    private IEnumerator EndMapCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        int rewardExp = 0;
        int rewardGold = 0;

        if (Targetable(player))
        {
            // tutorial clear
            if (meta.typeID == 50000) PlayerData.tutorial = 1;

            RewardInfo info = TableData.instance.rewardDataDic[meta.rewardID];
            rewardExp = info.exp;
            rewardGold = info.gold;

            GameManager.Instance.GetExp(rewardExp);
            GameManager.Instance.GetReward(info.GetReward());

            AudioManager.Instance.PlayVictory();
        }
        else
        {
            AudioManager.Instance.PlayGameover();
        }
        
        GUIManager.Instance.messageBoxPanel.CallRewardMessageBox("Message_RewardExpGold",
            () =>
            {
                GUIManager.Instance.FadeIn(() =>
                {
                    ClearCombat();
                    onMapEnd();
                    GameManager.Instance.DequeuePopups();
                    GUIManager.Instance.menuPanel.Show();
                    GameManager.Instance.ChangeCam(false);
                    AudioManager.Instance.PlayMain();
                    GUIManager.Instance.FadeOut();
                });
            },
            () =>
            {
                GUIManager.Instance.FadeIn(() =>
                {
                    ClearCombat();
                    onMapEnd();
                    SetMap(meta.typeID);
                    GUIManager.Instance.FadeOut();
                });
            }, rewardExp, rewardGold);
    }

    /// <summary>
    /// 현재 스테이지를 반환하고 리스트에서 삭제
    /// </summary>
    private int GetAndRemoveCurrentStage()
    {
        int currentStage = -1;
        
        if (stageIDs.Count > 0)
        {
            currentStage = stageIDs.Dequeue();
        }
        
        return currentStage;
    }

    /// <summary>
    /// 전투상의 모든 오브젝트 제거
    /// </summary>
    private void ClearCombat()
    {
        field.Despawn();
        field = null;
        player.Despawn();
        player = null;
        for (int i = 0; i < mobs.Length; i++)
        {
            if (mobs[i] != null && !mobs[i].isDead) mobs[i].Despawn();
            mobs[i] = null;
        }
        targeting.Despawn();
        targeting = null;
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
    /// 콤보 UI를 표시
    /// </summary>
    public void SetComboUI()
    {
        comboText.Show(vitalSign.combo);
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
    /// 플레이어가 죽는 순간 실행되는 함수
    /// </summary>
    public void PlayerDeath()
    {
        mobCount = 0;
        onStageEnd();
        EndMap(2);
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

    public void EnterBossTrigger()
    {
        AudioManager.Instance.PlayBoss(meta.fieldID);
        warningScreen.StartWarningScreen();
    }

    /// <summary>
    /// 대상을 타겟으로 삼을 수 있는지 판단
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool Targetable(BaseChar target)
    {
        return target != null && target.gameObject.activeInHierarchy && !target.isDead;
    }
}
