 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public Transform ParentTM { get { return transform.parent.transform; } }
    [System.NonSerialized]
    public GameObject prefeb;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OnSpawn()
    {
        AudioManager.OnEffectSoundChange += SetAudioVolume;
    }

    public void Position(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void LocalPosition(float x, float y, float z)
    {
        transform.localPosition = new Vector3(x, y, z);
    }

    public virtual void Despawn()
    {
        AudioManager.OnEffectSoundChange -= SetAudioVolume;
        PoolingManager.Instance.Despawn(this);
    }

    public void SetAudioVolume()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = AudioManager.Instance.GetEffectSoundDegree();
        }
    }
}
