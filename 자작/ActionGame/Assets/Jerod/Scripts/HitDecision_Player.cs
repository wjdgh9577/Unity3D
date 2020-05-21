using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 대상을 타격했는지 판탄하는 클래스
 * 타격 범위의 대상을 리스트로 저장한 후 공격시 모든 대상 타격
 */
public class HitDecision_Player : MonoBehaviour
{
    public GameObject player;
    public GameObject playerHitBox;

    public float ratio;          //스킬에 따른 데미지 비율
    List<GameObject> enemys;     //타격 박스 내의 오브젝트
    Collider hurtBox;
    Collider hitBox;
    Status_Player status;

    [System.Obsolete]
    void Start()
    {
        this.enemys = new List<GameObject>();
        this.hurtBox = GetComponent<Collider>();
        this.hitBox = this.playerHitBox.GetComponent<Collider>();
        this.status = this.player.GetComponent<Status_Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            this.enemys.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            GameObject remove = this.enemys.Find(enemy => enemy.Equals(other.gameObject.transform.parent.gameObject));
            this.enemys.Remove(remove);
        }
    }

    //Jab에 대한 타격 판정
    public IEnumerator HitEnemy_Jab()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach(GameObject enemy in enemys)
        {
            Status_Monster enemyStat = enemy.GetComponent<Status_Monster>();
            enemy.GetComponent<Status_Monster>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            StartCoroutine(enemyStat.GetStun(0.3f));
            this.status.GetStamina(5);
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }

    //Hikick에 대한 타격 판정
    public IEnumerator HitEnemy_Hikick()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            Status_Monster enemyStat = enemy.GetComponent<Status_Monster>();
            enemyStat.GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            StartCoroutine(enemyStat.GetStun(0.3f));
            this.status.GetStamina(1);
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }

    //Risingpunch에 대한 타격 판정
    public IEnumerator HitEnemy_Risingpunch()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            Status_Monster enemyStat = enemy.GetComponent<Status_Monster>();
            enemy.GetComponent<Status_Monster>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            StartCoroutine(enemyStat.GetStun(1f));
            this.status.GetStamina(1);
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }

    //Spinkick에 대한 타격 판정
    public IEnumerator HitEnemy_Spinkick()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            Status_Monster enemyStat = enemy.GetComponent<Status_Monster>();
            enemy.GetComponent<Status_Monster>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            enemy.GetComponent<Rigidbody>().AddForce((enemy.transform.position - this.player.transform.position) * 5, ForceMode.VelocityChange);
            StartCoroutine(enemyStat.GetStun(1f));
            this.status.GetStamina(1);
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }

    //Screwkick에 대한 타격 판정
    public IEnumerator HitEnemy_Screwkick()
    {
        this.hitBox.enabled = false;
        yield return StartCoroutine(transform.parent.GetComponent<Player_TPS>().CallCutScene());
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            Status_Monster enemyStat = enemy.GetComponent<Status_Monster>();
            enemy.GetComponent<Status_Monster>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            StartCoroutine(enemyStat.GetStun(3f));
            enemy.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(Vector3.up + (enemy.transform.position - this.player.transform.position)) * 10, ForceMode.VelocityChange);
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
        this.hitBox.enabled = true;
    }
}
