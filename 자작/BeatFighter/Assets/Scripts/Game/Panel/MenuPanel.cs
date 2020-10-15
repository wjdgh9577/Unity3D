using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : PanelBase
{
    [SerializeField]
    private GameObject tutorialButton;
    [SerializeField]
    private GameObject journeyButton;

    public JourneyMode journeyMode;

    public override void Show()
    {
        base.Show();
        if (PlayerData.tutorial == 0)
        {
            tutorialButton.SetActive(true);
            journeyButton.SetActive(false);
        }
        else
        {
            tutorialButton.SetActive(false);
            journeyButton.SetActive(true);
        }
    }

    /// <summary>
    /// 계정 생성 최초 1회 노출
    /// </summary>
    public void OnTutorialButton()
    {
        GUIManager.Instance.FadeIn(() =>
        {
            GUIManager.Instance.menuPanel.Hide();
            Combat.Instance.SetMap(50000);
            GUIManager.Instance.FadeOut();
        });
    }

    public void OnJourneyButton()
    {
        journeyMode.Show();
    }

    public void OnLogoutButton()
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
