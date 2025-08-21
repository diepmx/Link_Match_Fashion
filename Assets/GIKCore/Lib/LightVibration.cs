using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVibration : MonoBehaviour
{
    /// <summary>
    /// Rung nhẹ với thời gian rất ngắn
    /// </summary>
    public void VibrateLight()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (vibrator != null)
            {
                if (AndroidVersion() >= 26) // Android 8.0+
                {
                    // VibrationEffect.createOneShot(milliseconds, amplitude)
                    AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                        "createOneShot", 40L, 50 // 40ms, độ mạnh 50/255
                    );
                    vibrator.Call("vibrate", effect);
                }
                else
                {
                    vibrator.Call("vibrate", 40L); // 40ms cho máy Android cũ
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Vibrate error: " + e.Message);
        }
#endif
    }

    private int AndroidVersion()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION");
        return version.GetStatic<int>("SDK_INT");
#else
        return 0;
#endif
    }
}
