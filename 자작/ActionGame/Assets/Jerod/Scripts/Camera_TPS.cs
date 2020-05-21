using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 메인 카메라 클래스
 * 3인칭 시점으로 플레이어를 따라다닌다.
 */
public class Camera_TPS : MonoBehaviour
{
    public GameObject tpsPos;               //메인 카메라 위치 저장 오브젝트
    public GameObject cutScenePos1;         //컷씬 시작 시점 오브젝트
    public GameObject cutScenePos2;         //컷씬 종료 시점 오브젝트
    public GameObject head;                 //캐릭터의 머리 위치 오브젝트

    public bool isLock = false;            //true일 경우 컷씬 카메라 제어
    private float rotx, roty, prey = 0;     //카메라 회전 계산 변수
    private float distance;                 //캐릭터로부터 카메라까지의 거리

    private void Start()
    {
        transform.position = this.tpsPos.transform.position;
        transform.forward = this.tpsPos.transform.forward;

        this.distance = Vector3.Distance(this.head.transform.position, transform.position);
    }

    private void LateUpdate()
    {
        if (!this.isLock) Zoom();

        SetCam();
    }

    //줌 인, 줌 아웃
    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Setting.scrollsens;
        float nextDistance = this.distance - scroll;

        if (nextDistance > 1 && nextDistance < 5) this.distance = nextDistance;
    }

    //메인 카메라 위치, 방향 계산
    void SetCam()
    {
        if(this.isLock)   //컷씬으로 카메라 제약
        {
            transform.position = Vector3.Lerp(transform.position, this.cutScenePos2.transform.position, 3 * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, this.cutScenePos2.transform.forward, 3 * Time.deltaTime);
        }
        else if (!this.isLock)   //카메라 이동 후 방향 전환 계산
        {
            transform.position = this.head.transform.position - transform.forward * this.distance;

            if (Setting.cursorLockOn)  //커서 숨길 경우 카메라 회전 가능
            {
                this.rotx = Input.GetAxis("Mouse X") * Setting.rotSens;
                this.roty = Input.GetAxis("Mouse Y") * Setting.rotSens;

                float nextAngle = transform.localEulerAngles.x - this.roty;
                if (nextAngle > Setting.angleLimit && nextAngle < 360 - Setting.angleLimit) this.roty = 0;
                transform.RotateAround(this.head.transform.position, Vector3.up, this.rotx);
                transform.RotateAround(this.head.transform.position, -transform.right, this.roty);
            }
        }
    }

    public void SetCutScene()
    {
        this.tpsPos.transform.position = transform.position;
        this.tpsPos.transform.forward = transform.forward;
        transform.position = this.cutScenePos1.transform.position;
        transform.forward = this.cutScenePos1.transform.forward;
    }
}