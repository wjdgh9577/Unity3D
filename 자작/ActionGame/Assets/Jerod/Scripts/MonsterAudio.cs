using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    public AudioClip monsterHitSound;
    public AudioClip monsterDieSound;

    AudioSource aud;

    void Awake()
    {
        this.aud = GetComponent<AudioSource>();
    }

    public void MonsterHitSound()
    {
        this.aud.PlayOneShot(this.monsterHitSound);
    }

    public void MonsterDieSound()
    {
        this.aud.PlayOneShot(this.monsterDieSound);
    }
}
