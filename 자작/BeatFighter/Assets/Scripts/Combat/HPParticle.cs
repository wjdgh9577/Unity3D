using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPParticle : PoolObj
{
    public GameObject Target;
    public float Speed = 100f;
    public float upSpeed = 1f;

    public bool JustOnStart = false;

    public float Alpha = 1f;
    public float FadeSpeed = 1f;

    private TextMesh HPLabel;

    private void Awake()
    {
        if (Target == null)
        {
            Target = Camera.main.gameObject;
        }

        Vector3 dir = Target.transform.position - transform.position;
        Quaternion Rotation = Quaternion.LookRotation(dir);

        gameObject.transform.rotation = Rotation;

        HPLabel = transform.GetChild(0).GetComponent<TextMesh>();
    }

    private void OnEnable()
    {
        upSpeed = 1f;
        Alpha = 1f;
    }

    private void Update()
    {
        if (JustOnStart == false)
        {
            Vector3 dir = Target.transform.position - transform.position;
            Quaternion Rotation = Quaternion.LookRotation(dir);

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Rotation, Speed * Time.deltaTime);
        }

        upSpeed += Time.deltaTime;
        transform.position += Vector3.up * upSpeed * Time.deltaTime;

        Alpha = Mathf.Lerp(Alpha, 0f, FadeSpeed * Time.deltaTime);

        Color CurrentColor = HPLabel.color;
        HPLabel.GetComponent<TextMesh>().color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, Alpha);

        if (Alpha < 0.005f)
        {
            Despawn();
        }
    }

    public void SetMesh(DamageInfo info)
    {
        HPLabel.text = info.judge != JudgeRank.Fail ? info.damage.ToString() : "Miss";
        HPLabel.color = info.judge == JudgeRank.critical ? Color.yellow : Color.red;
    }
}
