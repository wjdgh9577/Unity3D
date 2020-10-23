using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    public Image _skillIcon;
    public GameObject _lock;
    public Image _cooldownImage;
    public Text _cooldownText;
    public KeyCode KeyCode;

    private PlayerChar player;
    private SkillInfo metaData;
    private float lastSkillTime = float.NegativeInfinity;
    private float remainTime { get { return Mathf.Clamp(metaData.cooldown + lastSkillTime - Time.time, 0, metaData.cooldown); } }
    private bool isCooldown { get { return Time.time - lastSkillTime < metaData.cooldown; } }

    private bool on = false;
    private bool isLock = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        on = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode)) on = true;
        if (on && !isLock && !isCooldown)
        {
            lastSkillTime = Time.time;
            _cooldownText.gameObject.SetActive(true);
            _cooldownImage.gameObject.SetActive(true);
            JudgeRank judge = GUIManager.Instance.combatPanel.vitalSign.Judge();
            CombatManager.Instance.SetComboUI();
            player.DoSkill(metaData.typeID, judge, GUIManager.Instance.combatPanel.vitalSign.combo);
        }
        if (isCooldown)
        {
            _cooldownText.text = ((int)remainTime).ToString();
            _cooldownImage.fillAmount = remainTime / metaData.cooldown;
        }
        else
        {
            _cooldownText.gameObject.SetActive(false);
            _cooldownImage.gameObject.SetActive(false);
            _lock.SetActive(false);
        }
        if (isLock) _lock.SetActive(true);
        else if (!isLock && !isCooldown) _lock.SetActive(false);
        
        on = false;
    }

    public void Refresh()
    {
        lastSkillTime = float.NegativeInfinity;
    }

    public void SetBaseData(PlayerChar player, SkillInfo meta)
    {
        this.player = player;
        this.metaData = meta;
        SetSkillIcon(meta.typeID);
    }

    private void SetSkillIcon(int typeID)
    {
        _skillIcon.sprite = PreloadManager.Instance.TryGetSprite(typeID);
    }

    public void Lock()
    {
        isLock = true;
    }

    public void Unlock()
    {
        isLock = false;
    }
}
