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

    public override void Initialize() { }

    public void CallOKMessageBox(string stringID, Action okAction, params object[] args)
    {
        Show();
        this.okMessageBox.SetActive(true);
        this.okMessage.text = string.Format(TableData.instance.GetString(stringID), args);
        this.onOKButton = okAction;
    }

    public void OnOKButton()
    {
        this.okMessageBox.SetActive(false);
        Hide();
        this.onOKButton?.Invoke();
    }

    public void CallYesNoMessageBox(string stringID, Action yesAction, Action noAction, params object[] args)
    {
        Show();
        this.yesNoMessageBox.SetActive(true);
        this.yesNoMessage.text = string.Format(TableData.instance.GetString(stringID), args);
        this.onYesButton = yesAction;
        this.onNoButton = noAction;
    }

    public void OnYesButton()
    {
        this.yesNoMessageBox.SetActive(false);
        Hide();
        this.onYesButton?.Invoke();
    }

    public void OnNoButton()
    {
        this.yesNoMessageBox.SetActive(false);
        Hide();
        this.onNoButton?.Invoke();
    }

    public void CallRewardMessageBox(string stringID, Action exitAction, Action retryAction, params object[] args)
    {
        Show();
        this.rewardMessageBox.SetActive(true);
        string format = string.Format(TableData.instance.GetString(stringID), args);
        this.rewardMessage.text = format;
        this.onExitButton = exitAction;
        this.onRetryButton = retryAction;
    }

    public void OnExitButton()
    {
        this.rewardMessageBox.SetActive(false);
        Hide();
        this.onExitButton?.Invoke();
    }

    public void OnRetryButton()
    {
        this.rewardMessageBox.SetActive(false);
        Hide();
        this.onRetryButton?.Invoke();
    }
}
