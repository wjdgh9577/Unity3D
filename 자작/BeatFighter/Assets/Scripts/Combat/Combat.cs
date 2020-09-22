﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Singleton<Combat>
{
    public static Action onStageSet;
    public static Action onStageEnd;

    public Transform cameraPoint;
    public GameObject targeting;

    public CharSetting playerSetting;
    public List<CharSetting> mobSettings;

    private int mapID;
    private List<int> stageIDs;
    
    public PlayerChar player { get; private set; }
    public MobChar[] mobs { get; private set; } = new MobChar[5];
    public int mobCount;

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 맵 세팅
    /// </summary>
    /// <param name="mapID"></param>
    public void SetMap(int mapID)
    {
        this.mapID = mapID;
        stageIDs = new List<int>();
        
        MapInfo mapInfo = TableData.instance.mapDataDic[mapID];
        if (mapInfo == null)
        {
            Debug.LogError("존재하지 않는 맵");
            return;
        }
        
        for (int i = 0; i < mapInfo.stageList.Count; i++)
        {
            stageIDs.Add(mapInfo.stageList[i]);
        }

        int currentStage = GetAndRemoveCurrentStage();
        if (currentStage == -1)
        {
            Debug.LogError("현재 맵에 스테이지가 존재하지 않음");
            return;
        }

        player = playerSetting.SetPlayer(10000);//
        SetStage(currentStage);
    }

    /// <summary>
    /// 스테이지 세팅
    /// </summary>
    /// <param name="stageID"></param>
    public void SetStage(int stageID)
    {
        mobCount = 0;
        StageInfo stageInfo = TableData.instance.stageDataDic[stageID];
        if (stageInfo == null)
        {
            Debug.LogError("존재하지 않는 스테이지");
            return;
        }

        foreach (var pair in stageInfo.mobList)
        {
            mobs[pair.Key] = mobSettings[pair.Key].SetMob(pair.Value);
            mobCount += 1;
        }

        onStageSet();
        player.SetTarget(mobs[0]);
        HPGroup.Instance.Refresh();
    }

    /// <summary>
    /// 현재 스테이지 시작
    /// </summary>
    public void StartCurrentStage()
    {

    }

    /// <summary>
    /// 현재 스테이지를 반환하고 리스트에서 삭제
    /// </summary>
    public int GetAndRemoveCurrentStage()
    {
        int currentStage = -1;
        
        if (stageIDs.Count > 0)
        {
            currentStage = stageIDs[0];
            stageIDs.RemoveAt(0);
        }
        
        return currentStage;
    }

    /// <summary>
    /// 타겟팅 표시 설정
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(BaseChar target)
    {
        targeting.SetActive(true);
        targeting.transform.position = target.transform.position + Vector3.up * 0.1f;
    }
}
