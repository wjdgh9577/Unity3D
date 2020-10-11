using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxPanel : PanelBase
{
    [Header("OK MessageBox")]
    [SerializeField]
    private GameObject okMessageBox;
    [SerializeField]
    private Text okMessage;

    [Header("YseNo MessageBox")]
    [SerializeField]
    private GameObject yesNoMessageBox;
    [SerializeField]
    private Text yesNoMessage;

    [Header("Reward MessageBox")]
    [SerializeField]
    private GameObject rewardMessageBox;
    [SerializeField]
    private Text rewardMessage;

    private Action onOKButton;
    private Action onYesButton;
    private Action onNoButton;
    private Action onExitButton;
    private Action onRetryButton;

    public void CallOKMessageBox(Action okAction)
    {
        Show();
        okMessageBox.SetActive(true);
        onOKButton = okAction;
    }

    public void OnOKButton()
    {
        onOKButton();
        okMessageBox.SetActive(false);
        Hide();
    }

    public void CallYesNoMessageBox(Action yesAction, Action noAction)
    {
        Show();
        yesNoMessageBox.SetActive(true);
        onYesButton = yesAction;
        onNoButton = noAction;
    }

    public void OnYesButton()
    {
        onYesButton();
        yesNoMessageBox.SetActive(false);
        Hide();
    }

    public void OnNoButton()
    {
        onNoButton();
        yesNoMessageBox.SetActive(false);
        Hide();
    }

    public void CallRewardMessageBox(Action exitAction, Action retryAction)
    {
        Show();
        rewardMessageBox.SetActive(true);
        onExitButton = exitAction;
        onRetryButton = retryAction;
    }

    public void OnExitButton()
    {
        onExitButton();
        rewardMessageBox.SetActive(false);
        Hide();
    }

    public void OnRetryButton()
    {
        onRetryButton();
        rewardMessageBox.SetActive(false);
        Hide();
    }
}
