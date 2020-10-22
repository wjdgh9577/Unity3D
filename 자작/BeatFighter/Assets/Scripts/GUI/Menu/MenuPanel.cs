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

    public override void Initialize()
    {
        this.journeyMode.Initialize();
    }

    public override void Show()
    {
        base.Show();
        if (PlayerData.tutorial == 0)
        {
            this.tutorialButton.SetActive(true);
            this.journeyButton.SetActive(false);
        }
        else
        {
            this.tutorialButton.SetActive(false);
            this.journeyButton.SetActive(true);
        }
    }

    /// <summary>
    /// 계정 생성 최초 1회 노출
    /// </summary>
    public void OnTutorialButton()
    {
        GameManager.Instance.SetTutorial();
    }

    public void OnJourneyButton()
    {
        this.journeyMode.Show();
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
