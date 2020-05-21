using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Monster : MonoBehaviour
{
    public string nickname;            //캐릭터명
    public int level;                  //레벨
    public int maxHealthPoint;       //Max HP
    public int healthPoint;          //HP
    public int maxStamina;        //Max 스테미나
    public int stamina;           //스테미나
    public int attackPoint;          //공격력
    public int defencePoint;         //방어력
    public float speed;                //이동속도
    public float jumpPower;            //점프력
    public float detectSight;          //탐지 한정 시야
    public float sight;                //시야
    public float hostility;            //적대심(n초 후 전투모드)
    public float reach;                //사거리
    public int exp;                  //획득 가능 경험치
    public int state;                //캐릭터의 현재 상태, 일반(0), 스턴(1), 사망(2)

    public GameObject hitBox;
    public GameObject player;
    public GameObject gameController;
    MonsterAudio aud;
    GameController gc;
    Status_Player status;

    private void Awake()
    {
        this.healthPoint = this.maxHealthPoint;
        this.stamina = 0;
        this.detectSight = this.sight;
        this.state = 0;

        this.aud = GetComponent<MonsterAudio>();
        this.gc = this.gameController.GetComponent<GameController>();
        this.status = this.player.GetComponent<Status_Player>();
    }

    public void GetDamage(int atk)
    {
        int damage = atk - this.defencePoint;
        if (damage < 0) damage = 0;
        this.healthPoint = Mathf.Clamp(this.healthPoint - damage, 0, this.maxHealthPoint);
        this.gc.SetMonsterHpUI(this.nickname, this.level, this.healthPoint, this.maxHealthPoint);

        if (this.healthPoint <= 0)
        {
            this.healthPoint = 0;
            IsDie();
        }
        else
        {
            this.aud.MonsterHitSound();
        }
    }

    //스킬 사용시 스테니마 소모
    public void SpendStamina(int amount)
    {
        this.stamina = Mathf.Clamp(this.stamina - amount, 0, this.maxStamina);
    }

    //공격시 스테미나 회복
    public void GetStamina(int amount)
    {
        this.stamina = Mathf.Clamp(this.stamina + amount, 0, this.maxStamina);
    }

    //일정 시간만큼 스턴 상태이상 부여
    public IEnumerator GetStun(float time)
    {
        this.state = 1;
        yield return new WaitForSeconds(time);
        this.state = 0;
    }

    void IsDie()
    {
        this.state = 2;
        this.hitBox.GetComponent<Collider>().enabled = false;
        this.status.GetExp(this.exp);
        this.aud.MonsterDieSound();
        Destroy(gameObject, 0.7f);
    }

    public void StatusReset()
    {
        this.healthPoint = this.maxHealthPoint;
        this.stamina = 0;
        this.sight = this.detectSight;
        this.state = 0;
    }
}
