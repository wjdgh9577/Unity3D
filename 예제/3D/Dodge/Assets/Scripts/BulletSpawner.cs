using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;

    void Start()
    {
        this.timeAfterSpawn = 0;
        this.spawnRate = Random.Range(this.spawnRateMin, this.spawnRateMax);
        this.target = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        this.timeAfterSpawn += Time.deltaTime;
        
        if (this.timeAfterSpawn >= this.spawnRate)
        {
            this.timeAfterSpawn = 0;
            
            GameObject bullet = Instantiate(this.bullet, transform.position, transform.rotation);
            bullet.transform.LookAt(this.target);
            
            this.spawnRate = Random.Range(this.spawnRateMin, this.spawnRateMax);
        }
    }
}
