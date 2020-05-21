using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Setting
{
    public static float rotSens = 3.0f;  //카메라, 캐릭터 회전 감도
    public static float scrollsens = 2.0f;  //스크롤 감도
    public static float angleLimit = 70f;  //카메라 상하 회전반경 제한 
    public static bool cursorLockOn = true;  //커서 락상태
    public static float playTime;   //플레이타임
    public static int frame = 60;
}
