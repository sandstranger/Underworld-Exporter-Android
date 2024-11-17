#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif
public static class DirectoryPicker 
{
    public static void PickDirectory()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass directoryPicker = new AndroidJavaClass("com.directorypicker.DirectoryPicker"))
                {
                    directoryPicker.CallStatic("PickDirectory", currentActivity);
                }
            }
        }
#endif        
    }
}
