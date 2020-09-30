using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Cinemachine.CinemachineVirtualCamera followCam;

    protected override void Awake()
    {
        base.Awake();
        TableData.instance = new TableData();
        TableData.instance.LoadTableDatas();
        PreloadManager.Instance.PreloadResources();
    }

    public void test()
    {
        Combat.Instance.gameObject.SetActive(true);
        Combat.Instance.SetMap(30000);
        followCam.Follow = Combat.Instance.cameraPoint;
        followCam.LookAt = Combat.Instance.cameraPoint;
    }
}
