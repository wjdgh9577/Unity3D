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
    private GameObject Lock;
    [SerializeField]
    private int mapID;

    public bool isLock { get; private set; }

    private void OnEnable()
    {
        isLock = mapID % 100 != 0 && !PlayerData.completedMaps.Contains(mapID - 1);
        Lock.SetActive(isLock);
    }

    public void Switch()
    {
        bool selected = playText.activeInHierarchy;
        nameText.SetActive(selected);
        playText.SetActive(!selected);
    }

    public void OnClick()
    {
        if (isLock)
        {
            GUIManager.Instance.menuPanel.journeyMode.Switch(this, mapID);
        }
        else if (GUIManager.Instance.menuPanel.journeyMode.mapID == this.mapID)
        {
            GameManager.Instance.SetCombat(this.mapID);
        }
        else
        {
            GUIManager.Instance.menuPanel.journeyMode.Switch(this, mapID);
            Switch();
        }
    }
}
