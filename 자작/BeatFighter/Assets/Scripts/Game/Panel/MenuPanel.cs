using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : PanelBase
{
    public void OnNewStartButton()
    {
        GUIManager.Instance.FadeIn(() =>
        {
            GUIManager.Instance.menuPanel.Hide();
            Combat.Instance.SetMap(30000);
            GUIManager.Instance.FadeOut();
        });
    }

    public void OnCancelButton()
    {
        GameManager.Instance.Logout();
        Hide();
    }

    public void OnCollectionButton()
    {
        GUIManager.Instance.collectionPanel.Show();
        Hide();
    }
}
