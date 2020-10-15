using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투시 플레이어에게 적용되는 Input
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public LayerMask layerMask;
    public float maxDistance = 30f;
    private RaycastHit hit;
    private PlayerChar player;

    private void Awake()
    {
        player = GetComponent<PlayerChar>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, maxDistance))
            {
                layerMask = hit.transform.gameObject.layer;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, maxDistance))
            {
                if (layerMask.value == LayerMask.NameToLayer("Enemy"))
                {
                    BaseChar target = hit.transform.GetComponent<MobChar>();
                    if (!target.isDead) player.SetTarget(target);
                }
            }
        }
    }
}
