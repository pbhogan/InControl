#if UNITY_ANDROID && !UNITY_EDITOR

//#define VERBOSE_LOGGING

using System;
#if VERBOSE_LOGGING
using System.Reflection;
#endif
using UnityEngine;

namespace org.json
{
    public class JSONObject : IDisposable
    {
        private const string LOG_TAG = "JSONObject";

        private static IntPtr _jcJsonObject = IntPtr.Zero;
        private static IntPtr _jmConstructor = IntPtr.Zero;
        private static IntPtr _jmGetDouble = IntPtr.Zero;
        private static IntPtr _jmGetInt = IntPtr.Zero;
        private static IntPtr _jmGetJsonArray = IntPtr.Zero;
        private static IntPtr _jmGetJsonObject = IntPtr.Zero;
        private static IntPtr _jmGetString = IntPtr.Zero;
        private static IntPtr _jmHas = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static JSONObject()
        {
            try
            {
                {
                    string strName = "org/json/JSONObject";
                    _jcJsonObject = AndroidJNI.FindClass(strName);
                    if (_jcJsonObject != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcJsonObject = AndroidJNI.NewGlobalRef(_jcJsonObject);
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
                    string strMethod = "<init>";
                    _jmConstructor = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)V");
                    if (_jmConstructor != IntPtr.Zero)
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
                    string strMethod = "getDouble";
                    _jmGetDouble = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)D");
                    if (_jmGetDouble != IntPtr.Zero)
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
                    _jmGetInt = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)I");
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
                    string strMethod = "getJSONArray";
                    _jmGetJsonArray = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONArray;");
                    if (_jmGetJsonArray != IntPtr.Zero)
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
                    string strMethod = "getJSONObject";
                    _jmGetJsonObject = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONObject;");
                    if (_jmGetJsonObject != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                        //_jmGetJsonObject = AndroidJNI.NewGlobalRef(_jmGetJsonObject);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getString";
                    _jmGetString = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
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
                    string strMethod = "has";
                    _jmHas = AndroidJNI.GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Z");
                    if (_jmHas != IntPtr.Zero)
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

        public JSONObject(IntPtr instance)
        {
            _instance = instance;
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public JSONObject(string buffer)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return;
            }
            if (_jmConstructor == IntPtr.Zero)
            {
                Debug.LogError("_jmConstructor is not initialized");
                return;
            }

            _instance = AndroidJNI.AllocObject(_jcJsonObject);
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("Failed to allocate JSONObject");
                return;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(buffer);
            AndroidJNI.CallVoidMethod(_instance, _jmConstructor, new jvalue[] { new jvalue() { l = arg1 }});
            AndroidJNI.DeleteLocalRef(arg1);
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

        public double getDouble(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmGetDouble == IntPtr.Zero)
            {
                Debug.LogError("_jmGetDouble is not initialized");
                return 0;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            double result = AndroidJNI.CallDoubleMethod(_instance, _jmGetDouble, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
            return result;
        }

        public int getInt(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmGetInt == IntPtr.Zero)
            {
                Debug.LogError("_jmGetInt is not initialized");
                return 0;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            int result = AndroidJNI.CallIntMethod(_instance, _jmGetInt, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
            return result;
        }

        public org.json.JSONArray getJSONArray(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jmGetJsonArray is not initialized");
                return null;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetJsonArray, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get JSONArray");
                return null;
            }

            org.json.JSONArray retVal = new JSONArray(result);
            return retVal;
        }

        public JSONObject getJSONObject(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jmGetJsonObject is not initialized");
                return null;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetJsonObject, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get JSONObject");
                return null;
            }

            JSONObject retVal = new JSONObject(result);
            return retVal;
        }

        public string getString(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetString == IntPtr.Zero)
            {
                Debug.LogError("_jmGetString is not initialized");
                return null;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetString, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get String");
                return null;
            }

            return AndroidJNI.GetStringUTFChars(result);
        }

        public bool has(string name)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return false;
            }
            if (_jmHas == IntPtr.Zero)
            {
                Debug.LogError("_jmHas is not initialized");
                return false;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(name);
            bool result = AndroidJNI.CallBooleanMethod(_instance, _jmHas, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            return result;
        }
    }
}

#endif