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

    public void CallOKMessageBox(string stringID, Action okAction = null)
    {
        Show();
        okMessageBox.SetActive(true);
        okMessage.text = TableData.instance.GetString(stringID);
        onOKButton = okAction;
    }

    public void OnOKButton()
    {
        onOKButton?.Invoke();
        okMessageBox.SetActive(false);
        Hide();
    }

    public void CallYesNoMessageBox(string stringID, Action yesAction = null, Action noAction = null)
    {
        Show();
        yesNoMessageBox.SetActive(true);
        yesNoMessage.text = TableData.instance.GetString(stringID);
        onYesButton = yesAction;
        onNoButton = noAction;
    }

    public void OnYesButton()
    {
        onYesButton?.Invoke();
        yesNoMessageBox.SetActive(false);
        Hide();
    }

    public void OnNoButton()
    {
        onNoButton?.Invoke();
        yesNoMessageBox.SetActive(false);
        Hide();
    }

    public void CallRewardMessageBox(string stringID, int exp, int gold, Action exitAction = null, Action retryAction = null)
    {
        Show();
        rewardMessageBox.SetActive(true);
        string format = string.Format(TableData.instance.GetString(stringID), exp, gold);
        rewardMessage.text = format;
        onExitButton = exitAction;
        onRetryButton = retryAction;
    }

    public void OnExitButton()
    {
        onExitButton?.Invoke();
        rewardMessageBox.SetActive(false);
        Hide();
    }

    public void OnRetryButton()
    {
        onRetryButton?.Invoke();
        rewardMessageBox.SetActive(false);
        Hide();
    }
}
