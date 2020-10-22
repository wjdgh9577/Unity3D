using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JudgeRank
{
    critical,
    Normal,
    Fail
}

public class VitalSign : MonoBehaviour
{
    public RectTransform heartTM;
    public RectTransform judgeTM;

    private List<Note> notes;
    private PlayerChar player;

    public int combo { get; private set; } = 0;
    private float afterSpawnTime;
    private float nextSpawnTime;
    private bool poolLock = true;

    public void Initialize()
    {
        CombatManager.onMapEnd += Hide;
        CombatManager.onStageSet += Show;
        CombatManager.onStageStart += UnlockPoolNote;
        CombatManager.onStageEnd += DespawnAll;
        this.notes = new List<Note>();
    }

    private void Update()
    {
        if (!this.poolLock)
        {
            this.afterSpawnTime += Time.deltaTime;
            if (!this.player.isDead && this.afterSpawnTime >= this.nextSpawnTime) PoolNote();
            if (this.notes.Count > 0 && this.notes[0].TM.anchoredPosition.y < 0)
            {
                this.combo = 0;
                this.notes[0].Despawn();
                this.notes.RemoveAt(0);
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.combo = 0;
        DespawnAll();
        gameObject.SetActive(false);
    }

    public void SetPlayer(PlayerChar player)
    {
        this.player = player;
    }

    /// <summary>
    /// 노트 생성
    /// </summary>
    public void PoolNote()
    {
        Note newNote = PoolingManager.Instance.Spawn<Note>(PlayerData.NoteUI, this.heartTM);
        this.notes.Add(newNote);

        List<float> sign = this.player.stats.GetSign();
        this.afterSpawnTime = 0;
        this.nextSpawnTime = sign[0];
        newNote.Initialize(sign[1], Time.time + this.nextSpawnTime, this.judgeTM);
    }

    /// <summary>
    /// 노트 생성 잠금 해제
    /// </summary>
    public void UnlockPoolNote()
    {
        this.poolLock = false;
        this.afterSpawnTime = float.PositiveInfinity;
    }

    /// <summary>
    /// 노트가 처리된 판정을 반환
    /// </summary>
    /// <returns></returns>
    public JudgeRank Judge()
    {
        if (this.notes.Count > 0)
        {
            Note note = this.notes[0];
            float gap = Mathf.Abs(Time.time - note.judgeTime);
            if (gap < 0.03f)
            {
                this.combo += 1;
                note.Despawn();
                this.notes.RemoveAt(0);
                return JudgeRank.critical;
            }
            else if (gap < 0.1f)
            {
                this.combo += 1;
                note.Despawn();
                this.notes.RemoveAt(0);
                return JudgeRank.Normal;
            }
        }
        this.combo = 0;
        return JudgeRank.Fail;
    }

    /// <summary>
    /// 모든 노트를 제거하고 노트 생성을 잠금
    /// </summary>
    public void DespawnAll()
    {
        for (int i = 0; i < this.notes.Count; i++)
        {
            this.notes[i].Despawn();
        }
        this.notes.Clear();
        this.poolLock = true;
    }
}
