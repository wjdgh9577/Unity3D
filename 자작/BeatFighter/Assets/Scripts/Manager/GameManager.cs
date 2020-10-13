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

    #region Login System
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
    #endregion

    public void ChangeCam(bool isCombat)
    {
        followCam.gameObject.SetActive(isCombat);
        menuCam.gameObject.SetActive(!isCombat);
    }

    public void GetExp(int exp)
    {
        PlayerData.CharData charData = PlayerData.charDataDic[PlayerData.currentChar];

        // Character level, exp
        int curLevel = charData.level;
        int curExp = charData.exp + exp;
        while (curExp >= TableData.instance.charExpDataDic[curLevel].requireExp)
        {
            if (TableData.instance.charExpDataDic[curLevel].requireExp == 0)
            {
                curExp = 0;
                break;
            }
            curExp -= TableData.instance.charExpDataDic[curLevel].requireExp;
            curLevel += 1;
        }
        charData.SetData(curLevel, curExp);

        // Skill level, exp
        List<int> skills = charData.skills;
        for (int i = 0; i < skills.Count; i++)
        {
            if (PlayerData.currentSkills.Contains(skills[i]))
            {
                PlayerData.SkillData skillData = charData.skillDatas[i];
                curLevel = skillData.level;
                curExp = skillData.exp + exp;
                while (curExp >= TableData.instance.skillExpDataDic[curLevel].requireExp)
                {
                    if (TableData.instance.skillExpDataDic[curLevel].requireExp == 0)
                    {
                        curExp = 0;
                        break;
                    }
                    curExp -= TableData.instance.skillExpDataDic[curLevel].requireExp;
                    curLevel += 1;
                }
                skillData.SetData(curLevel, curExp);
                charData.skillDatas[i] = skillData;
            }
        }

        PlayerData.charDataDic[PlayerData.currentChar] = charData;

        PlayerData.SaveData();
    }
}
