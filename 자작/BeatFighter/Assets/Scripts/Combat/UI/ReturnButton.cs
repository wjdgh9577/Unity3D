using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    [SerializeField]
    private ReturnPanel returnPanel;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        returnPanel.Hide();
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        returnPanel.Show();
    }
}
