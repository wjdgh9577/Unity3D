using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPortal : MonoBehaviour
{
    public GameObject sceneManager;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //this.sceneManager.GetComponent<TutorialSceneManager>().Portal();
        Quit();
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
