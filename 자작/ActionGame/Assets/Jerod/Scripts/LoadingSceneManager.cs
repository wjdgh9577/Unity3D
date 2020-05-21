using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    public GameObject loadingText;
    Text load;

    void Start()
    {
        this.load = this.loadingText.GetComponent<Text>();
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;
            
            if (op.progress < 0.9f)
            {
                this.load.text = "Loading... " + Mathf.RoundToInt(op.progress * 100) + "%";
            }
            else
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
