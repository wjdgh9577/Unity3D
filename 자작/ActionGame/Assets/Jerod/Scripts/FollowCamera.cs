using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public PlayerInput playerInput;  //플레이어의 Input
    public Transform playerPos;  //플레이어의 Transform
    public Transform camPos;  //메인카메라의 Transform
    public GameManager gameManager;  //게임매니저 오브젝트

    private Vector3 target;  //메인카메라의 타겟
    private Vector3 saveCamForward;  //카메라 정면 저장
    private float distance = 3;  //플레이어로부터 카메라까지 거리
    private float angleLimit = 70f;  //카메라 상하 회전반경 제한
    private float scrollsens = 2.0f;  //스크롤 감도
    private float rotSens = 3.0f;  //카메라, 캐릭터 회전 감도

    private void LateUpdate()
    {
        if (gameManager.cursorLockOn) Zoom();

        SetCam();
        ModifyCam();
    }

    //줌 인, 줌 아웃
    private void Zoom()
    {
        float scroll = playerInput.scroll * scrollsens;
        float nextDistance = distance - scroll;

        if (nextDistance > 1 && nextDistance < 5) distance = nextDistance;
    }

    //메인 카메라 위치, 방향 계산
    private void SetCam()
    {
        target = playerPos.position + new Vector3(0, 2f, 0);
        transform.position = target - transform.forward * distance;
        transform.LookAt(target);
        
        if (playerInput.scrollButtonDown)
        {
            saveCamForward = transform.forward;
        }
        else if (playerInput.scrollButtonUp)
        {
            transform.position = target - saveCamForward * distance;
            transform.LookAt(target);
        }

        if (gameManager.cursorLockOn)  //커서 숨길 경우 카메라 회전 가능
        {
            float rotx = playerInput.horizontalMouse * rotSens;
            float roty = playerInput.verticalMouse * rotSens;
            float nextAngle = transform.localEulerAngles.x - roty;

            if (nextAngle > angleLimit && nextAngle < 360 - angleLimit) roty = 0;
            transform.RotateAround(target, Vector3.up, rotx);
            transform.RotateAround(target, -transform.right, roty);
        }

        camPos.position = transform.position;
        camPos.forward = transform.forward;
    }

    //오브젝트 관통 방지
    private void ModifyCam()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;

        if (Physics.Raycast(target, -transform.forward, out hit, distance + 1))
        {
            if(hit.collider.tag == "Field")
            {
                camPos.position = hit.point + transform.forward;
            }
        }
    }
}
