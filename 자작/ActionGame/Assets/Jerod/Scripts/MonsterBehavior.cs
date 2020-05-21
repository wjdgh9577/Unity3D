using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 몬스터의 상태와 AI를 정의하는 스크립트
 * 대기 - 감지 - 접근 - 공격
 */
public class MonsterBehavior : MonoBehaviour
{
    Status_Monster status;
    Rigidbody rb;
    Animator anim;
    Vector3 spawnPos;          //스폰 위치
    bool add = false;          //플레이어 탐지
    bool chase = false;        //전투(추격) 시작

    public GameObject attack1;
    public GameObject attack2;
    public GameObject player;

    IEnumerator fight;
    IEnumerator chaseAndAttack;

    void Start()
    {
        this.status = GetComponent<Status_Monster>();
        this.rb = GetComponent<Rigidbody>();
        this.anim = GetComponent<Animator>();
        this.spawnPos = transform.position;

        this.chaseAndAttack = ChaseAndAttack();
        StartCoroutine(this.chaseAndAttack);
    }

    void Update()
    {
        //시야 내로 플레이어가 감지되면 애드가 일어남
        if (Vector3.Magnitude(this.player.transform.position - transform.position) <= this.status.detectSight && !this.add)
        {
            this.add = true;
            this.anim.SetBool("Add", true);
            fight = WhenChase(this.status.hostility);
            StartCoroutine(fight);
        }
        else if (Vector3.Magnitude(this.player.transform.position - transform.position) > this.status.sight && this.add)
        { 
            this.add = false;
            this.anim.SetBool("Add", false);
            StopCoroutine(fight);
        }

        //선공을 맞을 경우
        if (this.status.state == 1)
        {
            this.add = true;
            this.chase = true;
        }

        //전투시 시야가 두배 확장됨 <= 전투중 리셋 방지
        if (this.chase) this.status.sight = this.status.detectSight * 3;

        if(this.add)
        {
            if (Vector3.Magnitude(transform.position - this.spawnPos) > 30)        //스폰 위치에서 일정 거리가 떨어지면 리셋
            {
                this.chase = false;
                transform.position = this.spawnPos;
                this.rb.velocity = Vector3.zero;
                this.status.StatusReset();
            }
        }
        else                              //애드가 풀리면 리셋
        {
            this.chase = false;
            transform.position = this.spawnPos;
            this.rb.velocity = Vector3.zero;
            this.status.StatusReset();
        }

        //몬스터 사망시 추격 및 공격 중지
        if (this.status.state == 2)
        {
            StopCoroutine(this.chaseAndAttack);
        }
    }

    private IEnumerator ChaseAndAttack()
    {
        WaitForSeconds sec = new WaitForSeconds(1f);
        while (true)
        {
            if (this.add && !(this.status.state == 1) && !(this.status.state == 2))
            {
                Vector3 forward = this.player.transform.position - transform.position;
                transform.forward = new Vector3(forward.x, 0, forward.z);
            }
            if (this.chase)
            {
                float distance = Vector3.Magnitude(this.player.transform.position - transform.position);

                if (!(this.status.state == 1))                         //스턴 상태이상에 걸리지 않은 경우만 추격과 공격
                {
                    if (distance > this.status.reach)            //사거리 밖일 경우 추격
                    {
                        this.rb.velocity = new Vector3(0, this.rb.velocity.y, 0);
                        this.rb.AddForce(transform.forward * this.status.speed, ForceMode.VelocityChange);
                    }
                    else                                      //사거리 안일 경우 공격
                    {
                        yield return sec;
                        if (this.status.stamina == this.status.maxStamina)                //특수공격(머리에 타고있을 수도 있으므로 y축도 공격
                        {
                            this.anim.SetTrigger("Attack2");
                            this.status.SpendStamina(100);
                            yield return StartCoroutine(this.attack2.GetComponent<HitDecision_Monster>().HitEnemy_Attack2());
                        }
                        else                             //일반공격
                        {
                            this.anim.SetTrigger("Attack1");
                            this.status.GetStamina(10);
                            yield return StartCoroutine(this.attack1.GetComponent<HitDecision_Monster>().HitEnemy_Attack1());
                        }
                        yield return sec;
                    }
                }
            }

            yield return GameController.waitForFixedUpdate;
        }
    }

    //특정 시간 경과후 추격 시작
    IEnumerator WhenChase(float time)
    {
        yield return new WaitForSeconds(time);
        this.chase = true;
    }
}
