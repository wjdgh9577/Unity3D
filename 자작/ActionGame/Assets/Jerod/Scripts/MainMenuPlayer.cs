using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayer : MonoBehaviour
{
    Animator anim;

    bool walk = true;

    WaitForSeconds waitforTenSeconds;

    private void Start()
    {
        this.anim = GetComponent<Animator>();
        this.waitforTenSeconds = new WaitForSeconds(10f);

        StartCoroutine(Switch());
    }

    IEnumerator Switch()
    {
        while (true)
        {
            StartCoroutine(Walk());
            yield return this.waitforTenSeconds;
            this.walk = false;
            yield return Idle();
            this.walk = true;
            this.anim.SetTrigger("Walk");
        }

    }

    IEnumerator Walk()
    {
        while (this.walk)
        {
            transform.RotateAround(Vector3.zero, Vector3.down, Time.deltaTime * 1.5f);
            yield return null;
        }
    }

    IEnumerator Idle()
    {
        int idleNum = Random.Range(0, 5);
        this.anim.SetTrigger("Idle" + idleNum);
        yield return this.waitforTenSeconds;
    }
}
