using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonListContent : MonoBehaviour
{
    [SerializeField]
    private GameObject nameText;
    [SerializeField]
    private GameObject playText;
    [SerializeField]
    private int mapID;

    public void Switch()
    {
        bool selected = playText.activeInHierarchy;
        nameText.SetActive(selected);
        playText.SetActive(!selected);
    }

    public void OnClick()
    {
        if (GUIManager.Instance.menuPanel.journeyMode.mapID == this.mapID)
        {
            GUIManager.Instance.FadeIn(() =>
            {
                GUIManager.Instance.menuPanel.journeyMode.Hide();
                GUIManager.Instance.menuPanel.Hide();
                Combat.Instance.SetMap(mapID);
                GUIManager.Instance.FadeOut();
            });
        }
        else
        {
            GUIManager.Instance.menuPanel.journeyMode.Switch(this, mapID);
            Switch();
        }
    }
}
