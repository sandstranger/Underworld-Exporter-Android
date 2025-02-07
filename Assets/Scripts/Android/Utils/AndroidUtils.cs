#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using UnityEngine.Android;
#endif

namespace UnderworldExporter.Game
{
    internal static class AndroidUtils
    {
        //https://stackoverflow.com/a/76256946
        public static void RequestManageAllFilesAccess()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var buildVersion = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                using (var buildCodes = new AndroidJavaClass("android.os.Build$VERSION_CODES"))
                {
                    //Check SDK version > 29
                    if (buildVersion.GetStatic<int>("SDK_INT") > buildCodes.GetStatic<int>("Q"))
                    {
                        using (var environment = new AndroidJavaClass("android.os.Environment"))
                        {
                            //сhecking if permission already exists
                            if (!environment.CallStatic<bool>("isExternalStorageManager"))
                            {
                                using (var settings = new AndroidJavaClass("android.provider.Settings"))
                                {
                                    using (var uri = new AndroidJavaClass("android.net.Uri"))
                                    {
                                        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                                        {
                                            using (var currentActivity =
                                                   unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                                            {
                                                using (var parsedUri =
                                                       uri.CallStatic<AndroidJavaObject>("parse",
                                                           $"package:{Application.identifier}"))
                                                {
                                                    using (var intent = new AndroidJavaObject("android.content.Intent",
                                                               settings.GetStatic<string>(
                                                                   "ACTION_MANAGE_APP_ALL_FILES_ACCESS_PERMISSION"),
                                                               parsedUri))
                                                    {
                                                        currentActivity.Call("startActivity", intent);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                                {
                                    Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                                }
                            }
                        }
                    }
                }
            }
#endif
        }
    }
}