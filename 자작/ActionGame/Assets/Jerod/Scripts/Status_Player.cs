using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 플레이어의 상태에 관한 클래스
 * 플레이어의 스테이터스의 변화를 관리하고 GameController를 호출하여 UI를 변경한다.
 */
public class Status_Player : MonoBehaviour
{
    public string nickname;            //캐릭터명
    public int level;                  //레벨
    public int maxHealthPoint;       //Max HP
    public int healthPoint;          //HP
    public int recoveryHealthPoint;   //HP 자연회복량
    public int maxStamina;        //Max 스테미나
    public int stamina;           //스테미나
    public int recoveryStamina;   //스테미나 자연회복량
    public int attackPoint;          //공격력
    public int defencePoint;         //방어력
    public float speed;                //이동속도
    public float jumpPower;            //점프력
    public float sight;                //야간 시야
    public int maxExp;               //레벨업 필요 경험치
    public int exp;                  //경험치
    public int state;                //캐릭터의 현재 상태, 일반(0), 스턴(1), 사망(2)

    public GameObject hitBox;
    public GameObject gameController;
    PlayerAudio aud;
    GameController gc;

    IEnumerator startSelfRecovory;

    private void Awake()
    {
        this.healthPoint = this.maxHealthPoint;
        this.stamina = 0;
        this.aud = GetComponent<PlayerAudio>();
        this.gc = this.gameController.GetComponent<GameController>();
        this.startSelfRecovory = SelfRecovery();
        StartCoroutine(this.startSelfRecovory);
    }

    //피해량을 계산하여 HP를 감소
    public void GetDamage(int atk)
    {
        int damage = atk - this.defencePoint;
        if (damage < 0) damage = 0;
        this.healthPoint = Mathf.Clamp(this.healthPoint - damage, 0, this.maxHealthPoint);
        this.gc.SetHpUI();

        if (this.healthPoint <= 0)    //플레이어 사망
        {
            this.healthPoint = 0;
            StartCoroutine(this.IsDie());
        }
        else
        {
            this.aud.HitSound();
        }
    }

    public void GetHealthPoint(int amount)
    {
        this.healthPoint = Mathf.Clamp(this.healthPoint + amount, 0, this.maxHealthPoint);
    }

    //스킬 사용시 스테니마 소모
    public void SpendStamina(int amount)
    {
        this.stamina = Mathf.Clamp(this.stamina - amount, 0, this.maxStamina);
        this.gc.SetStaminaUI();
    }

    //타격 성공시 스테미나 회복
    public void GetStamina(int amount)
    {
        this.stamina = Mathf.Clamp(this.stamina + amount, 0, this.maxStamina);
        this.gc.SetStaminaUI();
    }

    //HP, 스테미나 자연회복(5초 마다 갱신)
    IEnumerator SelfRecovery()
    {
        WaitForSeconds sec = new WaitForSeconds(5f);
        while(true)
        {
            yield return sec;
            if (this.healthPoint < this.maxHealthPoint)
            {
                GetHealthPoint(this.recoveryHealthPoint);
                this.gc.SetHpUI();
            }
            if (this.stamina < this.maxStamina)
            {
                GetStamina(this.recoveryStamina);
                this.gc.SetStaminaUI();
            }
        }
    }

    public void GetExp(int amount)
    {
        this.exp += amount;
        while(this.exp >= this.maxExp)
        {
            LevelUp();
        }
        this.gc.SetExpUI();
    }

    void LevelUp()
    {
        this.level++;
        this.exp -= this.maxExp;

        // 레벨별 능력치
        this.maxExp *= 2;
        this.attackPoint += 2;
        this.maxHealthPoint += 10;
        this.healthPoint = this.maxHealthPoint;

        this.gc.SetHpUI();
        this.gc.SetLevelUI();
    }

    public IEnumerator GetStun()
    {
        if (this.state == 0)
        {
            this.state = 1;
            GetComponent<Animator>().SetTrigger("Stun");
            yield return new WaitForSeconds(2.378f);
            if (this.state != 2) this.state = 0;
        }
    }

    // 캐릭터 사망시 호출
    IEnumerator IsDie()
    {
        this.state = 2;
        StopCoroutine(this.startSelfRecovory);
        this.aud.DieSound();
        this.hitBox.GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(2);
        this.gc.IsDieUI();
    }
}
