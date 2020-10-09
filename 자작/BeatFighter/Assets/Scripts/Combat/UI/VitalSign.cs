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

    private List<RectTransform> notes;
    private PlayerChar player;

    public int combo { get; private set; } = 0;
    private float time;
    private float period;
    private bool poolLock = true;

    public void Initialize()
    {
        Combat.onMapEnd += Hide;
        Combat.onStageSet += Show;
        Combat.onStageStart += UnlockPoolNote;
        Combat.onStageEnd += DespawnAll;
        notes = new List<RectTransform>();
        Hide();
    }

    private void FixedUpdate()
    {
        if (!poolLock)
        {
            time += Time.fixedDeltaTime;
            if (!player.isDead && time >= period) PoolNote();
            if (notes.Count > 0 && notes[0].anchoredPosition.y < 0)
            {
                combo = 0;
                notes[0].GetComponent<Note>().Despawn();
                notes.RemoveAt(0);
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
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
        time = 0;
        Note newNote = PoolingManager.Instance.Spawn<Note>(PlayerData.NoteUI, heartTM);
        RectTransform newNoteTM = newNote.GetComponent<RectTransform>();
        notes.Add(newNoteTM);
        List<float> sign = player.stats.GetSign();
        period = sign[0];
        newNote.SetSpeed(sign[1]);
        newNote.judgeTime = Time.time + period;
        newNoteTM.anchoredPosition = judgeTM.anchoredPosition + Vector2.up * period * newNote.speed;
    }

    /// <summary>
    /// 노트 생성 잠금 해제
    /// </summary>
    public void UnlockPoolNote()
    {
        poolLock = false;
        time = float.PositiveInfinity;
    }

    /// <summary>
    /// 노트가 처리된 판정을 반환
    /// </summary>
    /// <returns></returns>
    public JudgeRank Judge()
    {
        if (notes.Count > 0)
        {
            Note note = notes[0].GetComponent<Note>();
            float gap = Mathf.Abs(Time.time - note.judgeTime);
            if (gap < 0.03f)
            {
                combo += 1;
                note.Despawn();
                notes.RemoveAt(0);
                return JudgeRank.critical;
            }
            else if (gap < 0.1f)
            {
                combo += 1;
                note.Despawn();
                notes.RemoveAt(0);
                return JudgeRank.Normal;
            }
        }
        combo = 0;
        return JudgeRank.Fail;
    }

    /// <summary>
    /// 모든 노트를 제거하고 노트 생성을 잠금
    /// </summary>
    public void DespawnAll()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[0].GetComponent<Note>().Despawn();
        }
        notes.Clear();
        poolLock = true;
    }
}
