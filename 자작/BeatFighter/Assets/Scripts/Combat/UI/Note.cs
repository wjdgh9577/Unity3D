using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float _speed = 100f;

    private RectTransform rectTransform;
    public float speed { get { return _speed; }}

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 노트의 속도 설정
    /// 기본 속도에 파라미터값이 곱해짐
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        _speed *= speed;
    }

    public void Despawn()
    {
        PoolingManager.Instance.Despawn(gameObject);
    }
}
