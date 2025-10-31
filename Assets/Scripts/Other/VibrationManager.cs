using System.Runtime.InteropServices;
using UnityEngine;

public static class VibrationManager
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void TriggerHapticFeedback(float intensity);

    public static void Vibrate(float intensity)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer && !PlayerSettingsUI.isVibrationOff)
        {
            TriggerHapticFeedback(intensity);
        }
        else
        {
            Debug.LogWarning("Haptics are only supported on iOS 13.0 or newer.");
        }
    }
#else
    public static void Vibrate(float intensity)
    {
        //if(!PlayerSettingsUI.isVibrationOff)
            //Debug.Log("Haptic feedback is not supported on this platform.");
    }
#endif
}
