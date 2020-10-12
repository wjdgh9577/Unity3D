using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera followCam;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera menuCam;
    [SerializeField]
    private Cinemachine.CinemachineSmoothPath track;
    [SerializeField]
    private Cinemachine.CinemachineDollyCart cart;

    protected override void Awake()
    {
        base.Awake();
        PlayerData.SetLanguage();
        menuCam.gameObject.SetActive(true);
        PreloadManager.Instance.PreloadResources();
        GUIManager.Instance.loginPanel.Show();
    }

    public void Login()
    {
        cart.m_Speed = 10;
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        while (true)
        {
            if (cart.m_Position == track.PathLength) break;
            yield return null;
        }
        
        GUIManager.Instance.menuPanel.Show();
        BackGround.Instance.SetBackGroundCharacter();
    }

    public void Logout()
    {
        cart.m_Speed = -10;
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LogoutCoroutine()
    {
        BackGround.Instance.DeleteBackGroundCharacter();
        while (true)
        {
            if (cart.m_Position == 0) break;
            yield return null;
        }
        GUIManager.Instance.loginPanel.Show();
    }

    public void ChangeCam(bool isCombat)
    {
        followCam.gameObject.SetActive(isCombat);
        menuCam.gameObject.SetActive(!isCombat);
    }
}
