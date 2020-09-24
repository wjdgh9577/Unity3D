using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalSign : MonoBehaviour
{
    public RectTransform heartTM;
    public RectTransform judgeTM;

    private List<RectTransform> notes;
    private PlayerChar player;
    private float time;
    private float period;

    private void Awake()
    {
        Combat.onStageSet += Show;
        Combat.onStageEnd += Hide;
        notes = new List<RectTransform>();
        Hide();
    }

    private void OnEnable()
    {
        time = float.PositiveInfinity;
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (!player.isDead && time >= period) PoolNote();
        if (notes.Count > 0 && notes[0].anchoredPosition.y < 0)
        {
            notes[0].GetComponent<Note>().Despawn();
            notes.RemoveAt(0);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[0].GetComponent<Note>().Despawn();
        }
        notes.Clear();
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
        Note newNote = PoolingManager.Instance.Spawn<Note>("Note", Folder.UI, heartTM);
        RectTransform newNoteTM = newNote.GetComponent<RectTransform>();
        notes.Add(newNoteTM);
        List<float> sign = player.stats.GetSign();
        period = sign[0];
        newNote.SetSpeed(sign[1]);
        newNoteTM.anchoredPosition = judgeTM.anchoredPosition + Vector2.up * period * newNote.speed;
    }
}
