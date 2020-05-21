using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDecision_Monster : MonoBehaviour
{
    public GameObject monster;

    public float ratio;          //스킬에 따른 데미지 비율
    List<GameObject> enemys;     //타격 박스 내의 오브젝트
    public Collider hurtBox;
    Status_Monster status;

    void Start()
    {
        this.enemys = new List<GameObject>();
        this.hurtBox = GetComponent<Collider>();
        this.status = this.monster.GetComponent<Status_Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox" && other.gameObject.transform.parent.tag == "Player")
        {
            this.enemys.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox" && other.gameObject.transform.parent.tag == "Player")
        {
            GameObject remove = this.enemys.Find(enemy => enemy.Equals(other.gameObject.transform.parent.gameObject));
            this.enemys.Remove(remove);
        }
    }

    public IEnumerator HitEnemy_Attack1()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Status_Player>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }

    public IEnumerator HitEnemy_Attack2()
    {
        this.hurtBox.enabled = true;
        yield return GameController.waitForFixedUpdate;
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Status_Player>().GetDamage(Mathf.RoundToInt(this.ratio * this.status.attackPoint));
            //enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            StartCoroutine(enemy.GetComponent<Status_Player>().GetStun());
        }
        yield return GameController.waitForFixedUpdate;
        this.hurtBox.enabled = false;
        this.enemys.Clear();
    }
}
