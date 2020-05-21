using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip breatheSound;
    public AudioClip headspringSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip jabSound;
    public AudioClip hikickSound;
    public AudioClip risingpunchSound;
    public AudioClip spinkickSound;
    public AudioClip chargingSound;
    public AudioClip screwkickSound;
    
    AudioSource aud;

    void Awake()
    {
        this.aud = GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        this.aud.PlayOneShot(this.jumpSound);
    }

    public void BreatheSound()
    {
        this.aud.PlayOneShot(this.breatheSound);
    }

    public void HeadspringSound()
    {
        this.aud.PlayOneShot(this.headspringSound);
    }

    public void HitSound()
    {
        this.aud.PlayOneShot(this.hitSound);
    }

    public void DieSound()
    {
        this.aud.PlayOneShot(this.dieSound);
    }

    public void JabSound()
    {
        this.aud.PlayOneShot(this.jabSound);
    }

    public void HikickSound()
    {
        this.aud.PlayOneShot(this.hikickSound);
    }

    public void RisingpunchSound()
    {
        this.aud.PlayOneShot(this.risingpunchSound);
    }

    public void SpinkickSound()
    {
        this.aud.PlayOneShot(this.spinkickSound);
    }

    public void ChargingSound()
    {
        this.aud.PlayOneShot(this.chargingSound);
    }

    public void ScrewkickSound()
    {
        this.aud.PlayOneShot(this.screwkickSound);
    }
}
