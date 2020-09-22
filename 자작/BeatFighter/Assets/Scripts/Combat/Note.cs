using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private RectTransform rectTransform;
    private float speed;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        speed = GameManager.Instance.noteSpeed;
    }

    void FixedUpdate()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - speed);
    }
}
