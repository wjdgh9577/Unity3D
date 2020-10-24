using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    public static Action onMapSet;
    public static Action onMapEnd;
    public static Action onStageSet;
    public static Action onStageStart;
    public static Action onStageEnd;

    [Header("References")]
    public Transform combatFieldTM;
    public Transform combatTM;
    public Transform cameraPoint;

    [Header("Character Setting")]
    public CharSetting playerSetting;
    public List<CharSetting> mobSettings;

    private MapInfo meta;
    private Queue<int> stageIDs;
    private Field field;
    private Targeting targeting;

    private IEnumerator startCurrentStageCoroutine;
    private IEnumerator endStageCoroutine;
    private IEnumerator endMapCoroutine;

    public PlayerChar player { get; private set; }
    public MobChar[] mobs { get; private set; } = new MobChar[5];
    [NonSerialized]
    public int mobCount;
    public bool isCombat { get; private set; }

    public void Initialize()
    {
        BaseChar.onPlayerDeath += PlayerDeath;
        BaseChar.onMobDeath += MobDeath;

        this.isCombat = false;
    }

    /// <summary>
    /// 맵 세팅
    /// </summary>
    /// <param name="mapID"></param>
    public void SetMap(int mapID)
    {
        this.combatTM.localPosition = Vector3.zero;

        this.meta = TableData.instance.mapDataDic[mapID];
        this.stageIDs = new Queue<int>();
        
        MapInfo mapInfo = TableData.instance.mapDataDic[mapID];
        
        for (int i = 0; i < mapInfo.stageList.Count; i++)
        {
            this.stageIDs.Enqueue(mapInfo.stageList[i]);
        }
        
        this.field = PoolingManager.Instance.Spawn<Field>(mapInfo.fieldID, this.combatFieldTM);
        this.field.SetBossTrigger(this.stageIDs.Count);
        this.player = playerSetting.SetPlayer(PlayerData.currentChar);
        GUIManager.Instance.combatPanel.SetPlayer(this.player);
        CreateTargetParticle();
        onMapSet();
        GUIManager.Instance.combatPanel.ShowReturnButton();
        AudioManager.Instance.PlayBattle(this.meta.fieldID);

        GotoNextStage(0);
    }

    /// <summary>
    /// 스테이지 세팅
    /// </summary>
    /// <param name="stageID"></param>
    private void SetStage(int stageID)
    {
        this.mobCount = 0;
        for (int i = 0; i < this.mobs.Length; i++) this.mobs[i] = null;

        StageInfo stageInfo = TableData.instance.stageDataDic[stageID];

        foreach (var pair in stageInfo.mobList)
        {
            this.mobs[pair.Key] = this.mobSettings[pair.Key].SetMob(pair.Value[0], pair.Value[1]);
            this.mobs[pair.Key].SetTarget(this.player);
            this.mobCount += 1;
        }

        this.player.SetTarget(GetRandomMob());
        GUIManager.Instance.combatPanel.SetHPGroup(this.player, this.mobs);
        onStageSet();
        StartCurrentStage();
    }

    /// <summary>
    /// 현재 스테이지 시작
    /// </summary>
    public void StartCurrentStage()
    {
        this.startCurrentStageCoroutine = StartCurrentStageCoroutine();
        StartCoroutine(this.startCurrentStageCoroutine);
    }

    private IEnumerator StartCurrentStageCoroutine()
    {
        yield return new WaitForSeconds(1);
        this.isCombat = true;
        onStageStart();
        for (int i = 0; i < this.mobs.Length; i++)
        {
            if (Targetable(this.mobs[i])) this.mobs[i].StartCombat();
        }
    }

    /// <summary>
    /// 다음 스테이지로 이동
    /// </summary>
    /// <param name="time"></param>
    public void GotoNextStage(float time)
    {
        int nextStage = GetAndRemoveCurrentStage();

        if (nextStage != -1)
        {
            this.endStageCoroutine = EndStageCoroutine(nextStage, time);
            StartCoroutine(this.endStageCoroutine);
        }
        else
        {
            GUIManager.Instance.combatPanel.HideReturnButton();
            EndMap(time);
        }
    }

    private IEnumerator EndStageCoroutine(int nextStage, float time)
    {
        yield return new WaitForSeconds(time);

        if (Targetable(this.player))
        {
            this.player.Play("Run");
            Vector3 startPos = this.combatTM.position;
            Vector3 endPos = this.combatTM.position + Vector3.right * 20;
            float progress = 0;
            while (true)
            {
                if (this.combatTM.position == endPos) break;
                progress += Time.deltaTime * 0.5f;
                this.combatTM.position = Vector3.Lerp(startPos, endPos, progress);
                yield return null;
            }
            this.player.CrossFade("Idle");
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
        this.endMapCoroutine = EndMapCoroutine(time);
        StartCoroutine(this.endMapCoroutine);
    }

    private IEnumerator EndMapCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        int rewardExp = 0;
        int rewardGold = 0;

        if (Targetable(this.player))
        {
            // tutorial clear
            if (this.meta.typeID == 50000) PlayerData.tutorial_combat = 1;

            RewardInfo info = TableData.instance.rewardDataDic[this.meta.rewardID];
            rewardExp = info.exp;
            rewardGold = info.gold;

            PlayerData.CompleteMap(this.meta.typeID);
            GameManager.Instance.GetExp(rewardExp);
            GameManager.Instance.GetReward(info.GetReward());

            AudioManager.Instance.PlayVictory();
        }
        else
        {
            AudioManager.Instance.PlayGameover();
        }
        
        GUIManager.Instance.messageBoxPanel.CallRewardMessageBox("Message_RewardExpGold",
            () => // 마을로 귀환
            {
                GUIManager.Instance.FadeIn(() =>
                {
                    ClearCombat();
                    onMapEnd();
                    GameManager.Instance.EndCombat();
                    GUIManager.Instance.FadeOut();
                });
            },
            () => // 다시하기
            {
                GUIManager.Instance.FadeIn(() =>
                {
                    ClearCombat();
                    onMapEnd();
                    SetMap(this.meta.typeID);
                    GUIManager.Instance.FadeOut();
                });
            }, rewardExp, rewardGold);
    }

    /// <summary>
    /// 마을로 귀환할 경우 실행되는 함수
    /// </summary>
    public void Return()
    {
        //if (this.startCurrentStageCoroutine != null) StopCoroutine(this.startCurrentStageCoroutine);
        //if (this.endStageCoroutine != null) StopCoroutine(this.endStageCoroutine);
        //if (this.endMapCoroutine != null) StopCoroutine(this.endMapCoroutine);
        StopAllCoroutines();

        this.isCombat = false;

        GUIManager.Instance.combatPanel.HideReturnButton();

        GUIManager.Instance.FadeIn(() =>
        {
            ClearCombat();
            onStageEnd();
            onMapEnd();
            GameManager.Instance.EndCombat();
            GUIManager.Instance.FadeOut();
        });
    }

    /// <summary>
    /// 현재 스테이지를 반환하고 리스트에서 삭제
    /// </summary>
    private int GetAndRemoveCurrentStage()
    {
        int currentStage = -1;
        
        if (this.stageIDs.Count > 0)
        {
            currentStage = this.stageIDs.Dequeue();
        }
        
        return currentStage;
    }

    /// <summary>
    /// 전투상의 모든 오브젝트 제거
    /// </summary>
    private void ClearCombat()
    {
        this.field.Despawn();
        this.field = null;
        this.player.Despawn();
        this.player = null;
        for (int i = 0; i < this.mobs.Length; i++)
        {
            if (this.mobs[i] != null && !this.mobs[i].isDead) this.mobs[i].Despawn();
            this.mobs[i] = null;
        }
        DespawnTargetParticle();
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
    /// 타겟팅 표시 생성
    /// </summary>
    private void CreateTargetParticle()
    {
        this.targeting = PoolingManager.Instance.Spawn<Targeting>(PlayerData.targetingParticle, transform);
        this.targeting.SetActive(false);
    }

    /// <summary>
    /// 타겟팅 표시 제거
    /// </summary>
    private void DespawnTargetParticle()
    {
        this.targeting.Despawn();
        this.targeting = null;
    }

    /// <summary>
    /// 타겟팅 표시 설정
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(BaseChar target)
    {
        this.targeting.transform.position = target.transform.position + Vector3.up * 0.1f;
    }

    /// <summary>
    /// 콤보 UI를 표시
    /// </summary>
    public void SetComboUI()
    {
        GUIManager.Instance.combatPanel.SetCombo();
    }

    /// <summary>
    /// 현재 살아있는 몬스터 중 랜덤으로 반환
    /// </summary>
    /// <returns></returns>
    private MobChar GetRandomMob()
    {
        List<int> mobIndexes = new List<int>();
        for (int i = 0; i < this.mobs.Length; i++)
        {
            if (Targetable(this.mobs[i])) mobIndexes.Add(i);
        }
        if (mobIndexes.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, mobIndexes.Count);
        return this.mobs[mobIndexes[index]];
    }

    /// <summary>
    /// 플레이어가 죽는 순간 실행되는 함수
    /// </summary>
    public void PlayerDeath()
    {
        this.mobCount = 0;
        this.isCombat = false;
        onStageEnd();
        EndMap(2);
    }

    /// <summary>
    /// 몬스터가 죽는 순간 실행되는 함수
    /// </summary>
    public void MobDeath()
    {
        this.mobCount -= 1;
        if (this.mobCount != 0 && this.player.Target.isDead) this.player.SetTarget(GetRandomMob());
        else if (this.mobCount == 0)
        {
            this.isCombat = false;
            onStageEnd();
            GotoNextStage(2);
        }
    }

    public void EnterBossTrigger()
    {
        AudioManager.Instance.PlayBoss(this.meta.fieldID);
        GUIManager.Instance.combatPanel.SetWarning();
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
