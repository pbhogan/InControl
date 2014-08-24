#if UNITY_ANDROID && !UNITY_EDITOR
#pragma warning disable 0414
using System;
using UnityEngine;

namespace tv.ouya.sdk
{
    public class OuyaUnityPlugin
    {
        private const string LOG_TAG = "OuyaUnityPlugin";
        private static IntPtr _jcOuyaUnityPlugin = IntPtr.Zero;
        private static IntPtr _jmConstructor = IntPtr.Zero;
        private static IntPtr _jmSetDeveloperId = IntPtr.Zero;
        private static IntPtr _jmUnityInitialized = IntPtr.Zero;
        private static IntPtr _jmGetGameData = IntPtr.Zero;
        private static IntPtr _jmPutGameData = IntPtr.Zero;
        private static IntPtr _jmFetchGamerInfo = IntPtr.Zero;
        private static IntPtr _jmAddGetProduct = IntPtr.Zero;
        private static IntPtr _jmDebugGetProductList = IntPtr.Zero;
        private static IntPtr _jmClearGetProductList = IntPtr.Zero;
        private static IntPtr _jmGetProductsAsync = IntPtr.Zero;
        private static IntPtr _jmRequestPurchaseAsync = IntPtr.Zero;
        private static IntPtr _jmGetReceiptsAsync = IntPtr.Zero;
        private static IntPtr _jmIsRunningOnOUYASupportedHardware = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static OuyaUnityPlugin()
        {
            try
            {
                {
                    string strName = "tv/ouya/sdk/OuyaUnityPlugin";
                    _jcOuyaUnityPlugin = AndroidJNI.FindClass(strName);
                    if (_jcOuyaUnityPlugin != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} class", strName));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} class", strName));
                        return;
                    }
                }

                {
                    string strMethod = "<init>";
                    _jmConstructor = AndroidJNI.GetMethodID(_jcOuyaUnityPlugin, strMethod, "(Landroid/app/Activity;)V");
                    if (_jmConstructor != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "setDeveloperId";
                    _jmSetDeveloperId = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
                    if (_jmSetDeveloperId != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "unityInitialized";
                    _jmUnityInitialized = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmUnityInitialized != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getGameData";
                    _jmGetGameData = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
                    if (_jmGetGameData != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "putGameData";
                    _jmPutGameData = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;Ljava/lang/String;)V");
                    if (_jmPutGameData != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "fetchGamerInfo";
                    _jmFetchGamerInfo = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmFetchGamerInfo != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "addGetProduct";
                    _jmAddGetProduct = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)V");
                    if (_jmAddGetProduct != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "debugGetProductList";
                    _jmDebugGetProductList = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmDebugGetProductList != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "clearGetProductList";
                    _jmClearGetProductList = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmClearGetProductList != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getProductsAsync";
                    _jmGetProductsAsync = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmGetProductsAsync != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
        
                {
                    string strMethod = "requestPurchaseAsync";
                    _jmRequestPurchaseAsync = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
                    if (_jmRequestPurchaseAsync != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
                
                {
                    string strMethod = "getReceiptsAsync";
                    _jmGetReceiptsAsync = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmGetReceiptsAsync != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "isRunningOnOUYASupportedHardware";
                    _jmIsRunningOnOUYASupportedHardware = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()Z");
                    if (_jmIsRunningOnOUYASupportedHardware != IntPtr.Zero)
                    {
                        Debug.Log(string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
            }
            catch (System.Exception)
            {
                Debug.LogError("Exception finding OuyaUnityPlugin class");
            }
        }

        public OuyaUnityPlugin(IntPtr currentActivity)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.Log("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmConstructor == IntPtr.Zero)
            {
                Debug.LogError("_jmConstructor is not initialized");
                return;
            }
            _instance = AndroidJNI.NewObject(_jcOuyaUnityPlugin, _jmConstructor, new jvalue[] { new jvalue() { l = currentActivity } });
        }

        public static void setDeveloperId(string developerId)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmSetDeveloperId == IntPtr.Zero)
            {
                Debug.LogError("_jmSetDeveloperId is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(developerId);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmSetDeveloperId, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static void unityInitialized()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmUnityInitialized == IntPtr.Zero)
            {
                Debug.LogError("_jmUnityInitialized is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmUnityInitialized, new jvalue[0]);
        }

        public static string getGameData(string key)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetGameData == IntPtr.Zero)
            {
                Debug.LogError("_jmGetGameData is not initialized");
                return null;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(key);
            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetGameData, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to getGameData");
                return null;
            }

            String retVal = AndroidJNI.GetStringUTFChars(result);
            return retVal;
        }

        public static void putGameData(string key, string val)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmPutGameData == IntPtr.Zero)
            {
                Debug.LogError("_jmPutGameData is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(key);
            IntPtr arg2 = AndroidJNI.NewStringUTF(val);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmPutGameData, new jvalue[] { new jvalue() { l = arg1 }, new jvalue() { l = arg2 } });
            AndroidJNI.DeleteLocalRef(arg1);
            AndroidJNI.DeleteLocalRef(arg2);
        }

        public static void fetchGamerInfo()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmFetchGamerInfo == IntPtr.Zero)
            {
                Debug.LogError("_jmFetchGamerInfo is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmFetchGamerInfo, new jvalue[0]);
        }

        public static void addGetProduct()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmAddGetProduct == IntPtr.Zero)
            {
                Debug.LogError("_jmAddGetProduct is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmAddGetProduct, new jvalue[0]);
        }

        public static void addGetProduct(string identifier)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmAddGetProduct == IntPtr.Zero)
            {
                Debug.LogError("_jmAddGetProduct is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(identifier);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmAddGetProduct, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static void debugGetProductList()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmDebugGetProductList == IntPtr.Zero)
            {
                Debug.LogError("_jmDebugGetProductList is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmDebugGetProductList, new jvalue[0]);
        }

        public static void clearGetProductList()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmClearGetProductList == IntPtr.Zero)
            {
                Debug.LogError("_jmClearGetProductList is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmClearGetProductList, new jvalue[0]);
        }

        public static void getProductsAsync()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmGetProductsAsync == IntPtr.Zero)
            {
                Debug.LogError("_jmGetProductsAsync is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmGetProductsAsync, new jvalue[0]);
        }

        public static void requestPurchaseAsync(string identifier)
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmRequestPurchaseAsync == IntPtr.Zero)
            {
                Debug.LogError("_jmRequestPurchaseAsync is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(identifier);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmRequestPurchaseAsync, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static void getReceiptsAsync()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmGetReceiptsAsync == IntPtr.Zero)
            {
                Debug.LogError("_jmGetReceiptsAsync is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmGetReceiptsAsync, new jvalue[0]);
        }

        public static bool isRunningOnOUYASupportedHardware()
        {
            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return false;
            }
            if (_jmIsRunningOnOUYASupportedHardware == IntPtr.Zero)
            {
                Debug.LogError("_jmIsRunningOnOUYASupportedHardware is not initialized");
                return false;
            }
            return AndroidJNI.CallStaticBooleanMethod(_jcOuyaUnityPlugin, _jmIsRunningOnOUYASupportedHardware, new jvalue[0]);
        }
    }
}

#endif