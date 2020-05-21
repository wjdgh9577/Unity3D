using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 캐릭터와 관련된 UI를 처리하는 클래스
 * 각 씬의 특수한 UI는 SceneManager가 처리한다.
 */
public class GameController : MonoBehaviour
{
    public static WaitForFixedUpdate waitForFixedUpdate;

    public GameObject hpGaugeUI;
    public GameObject hpTextUI;
    public GameObject staminaGaugeUI;
    public GameObject staminaTextUI;
    public GameObject expUI;
    public GameObject expTextUI;
    public GameObject nicknameTextUI;
    public GameObject levelTextUI;
    public GameObject player;
    public GameObject monsterMaxHpGaugeUI;
    public GameObject monsterHpGaugeUI;
    public GameObject monsterHpGaugeTextUI;
    public GameObject monsterNameTextUI;
    public GameObject monsterLevelTextUI;
    public GameObject inventory;
    public GameObject escapeWindow;

    //플레이어
    Image hpGauge;
    Image staminaGauge;
    Image expGauge;
    Text hpText;
    Text staminaText;
    Text expText;
    Text nicknameText;
    Text levelText;

    bool inventorySwitch;
    bool escapeWindowSwitch;

    //몬스터
    Image monsterHpGauge;
    Text monsterHpText;
    Text monsterNameText;
    Text monsterLevelText;

    Status_Player status;
    IEnumerator resetCoroutine;

    private void Awake()
    {
        Application.targetFrameRate = Setting.frame;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        this.hpGauge = this.hpGaugeUI.GetComponent<Image>();
        this.staminaGauge = this.staminaGaugeUI.GetComponent<Image>();
        this.expGauge = this.expUI.GetComponent<Image>();

        this.hpText = this.hpTextUI.GetComponent<Text>();
        this.staminaText = this.staminaTextUI.GetComponent<Text>();
        this.expText = this.expTextUI.GetComponent<Text>();
        this.nicknameText = this.nicknameTextUI.GetComponent<Text>();
        this.levelText = this.levelTextUI.GetComponent<Text>();
        this.inventorySwitch = false;
        this.inventory.SetActive(false);
        this.escapeWindowSwitch = false;
        this.escapeWindow.SetActive(false);

        this.monsterHpGauge = this.monsterHpGaugeUI.GetComponent<Image>();
        this.monsterHpText = this.monsterHpGaugeTextUI.GetComponent<Text>();
        this.monsterNameText = this.monsterNameTextUI.GetComponent<Text>();
        this.monsterLevelText = this.monsterLevelTextUI.GetComponent<Text>();
        
        this.status = this.player.GetComponent<Status_Player>();
        this.resetCoroutine = ResetAfterNSeconds(10);
        
        SetNicknameUI();
        SetLevelUI();
        SetHpUI();
        SetStaminaUI();
        SetExpUI();

        this.monsterMaxHpGaugeUI.SetActive(false);
        this.monsterHpGaugeUI.SetActive(false);
        this.monsterHpText.text = "";
        this.monsterNameText.text = "";
        this.monsterLevelText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))    //커서 락모드
        {
            SetCursorLock();
        }
        if (Input.GetKeyDown(KeyCode.I))      //인벤토리
        {
            SetInventoryUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape))    //종료창
        {
            SetEscapeWindowUI();
        }
    }

    //UI상의 닉네임을 설정
    public void SetNicknameUI()
    {
        this.nicknameText.text = this.status.nickname;
    }

    //UI상의 레벨을 설정
    public void SetLevelUI()
    {
        this.levelText.text = "Lv. " + this.status.level;
    }

    //UI상의 HP를 설정
    public void SetHpUI()
    {
        this.hpText.text = this.status.healthPoint + " / " + this.status.maxHealthPoint;
        this.hpGauge.fillAmount = (float)this.status.healthPoint / this.status.maxHealthPoint;
    }

    //UI상의 스테미나를 설정
    public void SetStaminaUI()
    {
        this.staminaText.text = this.status.stamina + " / " + this.status.maxStamina;
        this.staminaGauge.fillAmount = (float)this.status.stamina / this.status.maxStamina;
    }

    //UI상의 경험치를 설정
    public void SetExpUI()
    {
        this.expText.text = this.status.exp + " / " + this.status.maxExp;
        this.expGauge.fillAmount = (float)this.status.exp / this.status.maxExp;
    }

    public void SetMonsterHpUI(string name, int level, int hp, int maxHp)
    {
        float time = 10;
        if (hp == 0) time = 1;

        StopCoroutine(this.resetCoroutine);
        this.resetCoroutine = ResetAfterNSeconds(time);
        StartCoroutine(this.resetCoroutine);

        this.monsterMaxHpGaugeUI.SetActive(true);
        this.monsterHpGaugeUI.SetActive(true);

        this.monsterHpGauge.fillAmount = (float)hp / maxHp;
        this.monsterHpText.text = hp + " / " + maxHp;
        this.monsterNameText.text = name;
        this.monsterLevelText.text = "Lv." + level;
    }

    private IEnumerator ResetAfterNSeconds(float N)
    {
        yield return new WaitForSeconds(N);

        this.monsterMaxHpGaugeUI.SetActive(false);
        this.monsterHpGaugeUI.SetActive(false);
        this.monsterHpText.text = "";
        this.monsterNameText.text = "";
        this.monsterLevelText.text = "";
    }

    //캐릭터 사망시 UI를 설정
    public void IsDieUI()
    {
        /*
         * 사망 UI를 보여줌
         * 구성요소 : 사망 문구, 마을로 귀환 버튼
         * 버튼 클릭 시 FadeManager.Instance.FadeIn("마을 씬");
         */
    }

    void SetInventoryUI()
    {
        if (this.inventorySwitch)
        {
            this.inventorySwitch = false;
            this.inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Setting.cursorLockOn = true;
        }
        else
        {
            this.inventorySwitch = true;
            this.inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Setting.cursorLockOn = false;
        }
    }

    void SetEscapeWindowUI()
    {
        if (this.escapeWindowSwitch)
        {
            this.escapeWindowSwitch = false;
            this.escapeWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Setting.cursorLockOn = true;
        }
        else
        {
            this.escapeWindowSwitch = true;
            this.escapeWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Setting.cursorLockOn = false;
        }
    }

    void SetCursorLock()
    {
        if (Setting.cursorLockOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Setting.cursorLockOn = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Setting.cursorLockOn = true;
        }
    }

    void SaveData()
    {
        //세이브로드 객체 참조하여 진행
    }

    
}
