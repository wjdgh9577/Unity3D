using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform mapRoot;
    public GameObject prefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SetNewPrefab());
    }

    IEnumerator SetNewPrefab()
    {
        while (true)
        {
            GameObject field = Instantiate(prefab) as GameObject;
            field.transform.SetParent(mapRoot);
            field.GetComponent<FieldMovement>().SetPosition(5f, -0.5f);
            //Invoke("field.GetComponent<FieldMovement>().Destroy", 5f);
            yield return new WaitForSeconds(2f);
        }

    }
}
