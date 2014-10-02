#if UNITY_ANDROID && !UNITY_EDITOR

//#define VERBOSE_LOGGING

using System;
using System.Reflection;
using UnityEngine;

namespace org.json
{
    public class JSONArray : IDisposable
    {
        private const string LOG_TAG = "JSONObject";

        private static IntPtr _jcJsonArray = IntPtr.Zero;
        private static IntPtr _jmGetInt;
        private static IntPtr _jmGetJsonObject;
        private static IntPtr _jmGetString;
        private static IntPtr _jmLength;
        private IntPtr _instance = IntPtr.Zero;

        static JSONArray()
        {
            try
            {
                {
                    string strName = "org/json/JSONArray";
                    _jcJsonArray = AndroidJNI.FindClass(strName);
                    if (_jcJsonArray != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcJsonArray = AndroidJNI.NewGlobalRef(_jcJsonArray);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} class", strName));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
            }
        }

        private static void JNIFind()
        {
            try
            {
                {
                    string strMethod = "getJSONObject";
                    _jmGetJsonObject = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)Lorg/json/JSONObject;");
                    if (_jmGetJsonObject != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getInt";
                    _jmGetInt = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)I");
                    if (_jmGetInt != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getString";
                    _jmGetString = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)Ljava/lang/String;");
                    if (_jmGetString != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "length";
                    _jmLength = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "()I");
                    if (_jmLength != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
            }
        }

        public JSONArray(IntPtr instance)
        {
            _instance = instance;
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public void Dispose()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            if (_instance != IntPtr.Zero)
            {
                AndroidJNI.DeleteLocalRef(_instance);
                _instance = IntPtr.Zero;
            }
        }

        public int length()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmLength == IntPtr.Zero)
            {
                Debug.LogError("_jmLength is not initialized");
                return 0;
            }

            int result = AndroidJNI.CallIntMethod(_instance, _jmLength, new jvalue[0]);
            return result;
        }

        public org.json.JSONObject getJSONObject(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jmGetJsonObject is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetJsonObject, new jvalue[] { new jvalue() { i = arg1 } });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get JSONObject");
                return null;
            }

            org.json.JSONObject retVal = new JSONObject(result);
            return retVal;
        }

        public int getInt(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmGetInt == IntPtr.Zero)
            {
                Debug.LogError("_jmGetInt is not initialized");
                return 0;
            }

            int arg1 = index;
            int result = AndroidJNI.CallIntMethod(_instance, _jmGetInt, new jvalue[] { new jvalue() { i = arg1 } });
            return result;
        }

        public string getString(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetString == IntPtr.Zero)
            {
                Debug.LogError("_jmGetString is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetString, new jvalue[] { new jvalue() { i = arg1 } });

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get String");
                return null;
            }

            return AndroidJNI.GetStringUTFChars(result);
        }
    }
}

#endif