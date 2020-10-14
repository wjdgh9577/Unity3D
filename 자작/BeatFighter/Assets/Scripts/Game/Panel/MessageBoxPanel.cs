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

    public void CallOKMessageBox(string stringID, Action okAction, params object[] args)
    {
        Show();
        okMessageBox.SetActive(true);
        okMessage.text = string.Format(TableData.instance.GetString(stringID), args);
        onOKButton = okAction;
    }

    public void OnOKButton()
    {
        okMessageBox.SetActive(false);
        Hide();
        onOKButton?.Invoke();
    }

    public void CallYesNoMessageBox(string stringID, Action yesAction, Action noAction, params object[] args)
    {
        Show();
        yesNoMessageBox.SetActive(true);
        yesNoMessage.text = string.Format(TableData.instance.GetString(stringID), args);
        onYesButton = yesAction;
        onNoButton = noAction;
    }

    public void OnYesButton()
    {
        yesNoMessageBox.SetActive(false);
        Hide();
        onYesButton?.Invoke();
    }

    public void OnNoButton()
    {
        yesNoMessageBox.SetActive(false);
        Hide();
        onNoButton?.Invoke();
    }

    public void CallRewardMessageBox(string stringID, Action exitAction, Action retryAction, params object[] args)
    {
        Show();
        rewardMessageBox.SetActive(true);
        string format = string.Format(TableData.instance.GetString(stringID), args);
        rewardMessage.text = format;
        onExitButton = exitAction;
        onRetryButton = retryAction;
    }

    public void OnExitButton()
    {
        rewardMessageBox.SetActive(false);
        Hide();
        onExitButton?.Invoke();
    }

    public void OnRetryButton()
    {
        rewardMessageBox.SetActive(false);
        Hide();
        onRetryButton?.Invoke();
    }
}
