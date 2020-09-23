using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGroup : MonoBehaviour
{
    private void Awake()
    {
        Combat.onStageSet += Show;
        Combat.onStageEnd += Hide;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
