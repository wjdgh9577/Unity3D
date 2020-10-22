using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : PoolObj
{
    private float defaultSpeed = 100f;
    private float speed;

    private float spawnTime;
    public float judgeTime;

    public RectTransform TM;
    private RectTransform judgeTM;

    public void Initialize(float speed, float judgeTime, RectTransform judgeTM)
    {
        if (this.TM == null) this.TM = GetComponent<RectTransform>();

        this.speed = this.defaultSpeed * speed;
        this.judgeTime = judgeTime;
        this.judgeTM = judgeTM;
        Update();
    }

    private void Update()
    {
        this.TM.anchoredPosition = new Vector2(0, this.judgeTM.anchoredPosition.y + (this.judgeTime - Time.time) * this.speed);
    }

    public override void Despawn()
    {
        base.Despawn();
    }
}
