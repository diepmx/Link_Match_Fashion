using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetData 
{
    public const int CANVAS_ADJUST = 0;
    public const int GET_KEY_DOWN = 1;
    public const int GET_DEVICE_UUID = 2;
    public const int WEBGL_PASTED = 3;
    public const int CLOSE_POPUP = 4;
    public const int OPEN_POPUP = 5;
    public const int FCM_ON_MESSAGE_RECEIVE = 6;
    public const int SCENE_ACTIVE = 7;
    public const int TWEEN_FOCUS_ACTIVE = 8;
    public const int TWEEN_BLUR_ACTIVE = 9;

    public int id;
    public object data;
    public bool force;

    public string ConvertDataToString()
    {
        return data != null ? (string)data : "";
    }
    public bool ConvertDataToBool()
    {
        return data != null ? (bool)data : false;
    }
    public int ConvertDataToInt()
    {
        return data != null ? (int)data : 0;
    }
    public long ConvertDataToLong()
    {
        return data != null ? (long)data : 0;
    }
    public float ConvertDataToFloat()
    {
        return data != null ? (float)data : 0f;
    }
    public double ConvertDataToDouble()
    {
        return data != null ? (double)data : 0;
    }
}
