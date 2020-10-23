using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Camera")]
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera followCam;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera menuCam;
    [SerializeField]
    private Cinemachine.CinemachineSmoothPath track;
    [SerializeField]
    private Cinemachine.CinemachineDollyCart cart;

    private static Queue<Action> popupQueue;

    private IEnumerator Start()
    {
        popupQueue = new Queue<Action>();

        // device setting
        PlayerData.SetLanguage();
        PlayerData.SetMusicSoundDegree();
        PlayerData.SetEffectSoundDegree();
        AudioManager.Instance.Setup();
        
        // data load
        yield return StartCoroutine(PreloadManager.Instance.PreloadResources());

        // initialize
        GUIManager.Instance.Initialize();
        BackGround.Instance.Initialize();
        CombatManager.Instance.Initialize();

        AudioManager.Instance.PlayMain();
        ChangeCam(false);
    }

    #region Login System
    public void Login()
    {
        this.cart.m_Speed = 10;
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        while (true)
        {
            if (this.cart.m_Position == this.track.PathLength) break;
            yield return null;
        }
        
        GUIManager.Instance.menuPanel.Show();
        BackGround.Instance.SetBackGroundCharacter();
    }

    public void Logout()
    {
        this.cart.m_Speed = -10;
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LogoutCoroutine()
    {
        BackGround.Instance.DeleteBackGroundCharacter();
        while (true)
        {
            if (this.cart.m_Position == 0) break;
            yield return null;
        }
        GUIManager.Instance.loginPanel.Show();
    }
    #endregion

    public void SetTutorial()
    {
        GUIManager.Instance.FadeIn(() =>
        {
            GUIManager.Instance.menuPanel.Hide();
            CombatManager.Instance.SetMap(50000);
            ChangeCam(true);

            GUIManager.Instance.FadeOut();
        });
    }

    public void SetCombat(int mapID)
    {
        GUIManager.Instance.FadeIn(() =>
        {
            GUIManager.Instance.menuPanel.journeyMode.Hide();
            GUIManager.Instance.menuPanel.Hide();
            CombatManager.Instance.SetMap(mapID);
            ChangeCam(true);

            GUIManager.Instance.FadeOut();
        });
    }

    public void EndCombat()
    {
        GUIManager.Instance.menuPanel.Show();
        AudioManager.Instance.PlayMain();
        DequeuePopups();
        ChangeCam(false);
    }
    
    private void ChangeCam(bool isCombat)
    {
        this.followCam.gameObject.SetActive(isCombat);
        this.menuCam.gameObject.SetActive(!isCombat);
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
            EnqueuePopups("Message_CharacterLevelUp", curLevel - 1, curLevel);
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
                    EnqueuePopups("Message_SkillLevelUp", TableData.instance.GetString("Skill_" + skills[i]), curLevel - 1, curLevel);
                }
                skillData.SetData(curLevel, curExp);
                charData.skillDatas[i] = skillData;
            }
        }

        PlayerData.charDataDic[PlayerData.currentChar] = charData;

        PlayerData.SaveData();
    }

    public void GetReward(Dictionary<string, List<int>> reward)
    {
        foreach (var pair in reward)
        {
            if (pair.Key == "character")
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    int typeID = pair.Value[i];
                    if (PlayerData.charDataDic.ContainsKey(typeID)) continue;
                    PlayerData.CharData newChar = new PlayerData.CharData();
                    newChar.Initialize(typeID);
                    PlayerData.charDataDic.Add(typeID, newChar);
                    EnqueuePopups("Message_GetNewCharacter", TableData.instance.GetString("Character_" + typeID.ToString()));
                }
            }
            else if (pair.Key == "item")
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    // 아이템 구현 이후
                }
            }
            else Debug.LogErrorFormat("유효하지 않은 보상: {0}", pair);
        }

        PlayerData.SaveData();
    }

    public void EnqueuePopups(string stringID, params object[] args)
    {
        popupQueue.Enqueue(() => { GUIManager.Instance.messageBoxPanel.CallOKMessageBox(stringID, DequeuePopups, args); });
    }

    private void DequeuePopups()
    {
        if (popupQueue.Count == 0) return;
        Action popup = popupQueue.Dequeue();
        popup?.Invoke();
    }
}
