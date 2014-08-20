#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using UnityEngine;

namespace java.io
{
    public class ByteArrayOutputStream
    {
        static IntPtr _jcByteArrayOutputStream = IntPtr.Zero;
        static IntPtr _jmClose = IntPtr.Zero;
        static IntPtr _jmConstructor = IntPtr.Zero;
        static IntPtr _jmSize = IntPtr.Zero;
        static IntPtr _jmToByteArray = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static ByteArrayOutputStream()
        {
            try
            {
                {
                    string strName = "java/io/ByteArrayOutputStream";
                    _jcByteArrayOutputStream = AndroidJNI.FindClass(strName);
                    if (_jcByteArrayOutputStream != IntPtr.Zero)
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
                    _jmConstructor = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()V");
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
                    string strMethod = "close";
                    _jmClose = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()V");
                    if (_jmClose != IntPtr.Zero)
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
                    string strMethod = "size";
                    _jmSize = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()I");
                    if (_jmSize != IntPtr.Zero)
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
                    string strMethod = "toByteArray";
                    _jmToByteArray = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()[B");
                    if (_jmToByteArray != IntPtr.Zero)
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
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
            }
        }

        public ByteArrayOutputStream()
        {
            _instance = AndroidJNI.AllocObject(_jcByteArrayOutputStream);
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("Failed to allocate ByteArrayOutputStream");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmConstructor, new jvalue[0]);

            //Debug.Log("Allocated ByteArrayOutputStream");
        }

        public void close()
        {
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmClose, new jvalue[0]);
        }

        public int size()
        {
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return 0;
            }

            int result = AndroidJNI.CallIntMethod(_instance, _jmSize, new jvalue[0]);
            Debug.Log(string.Format("ByteArrayOutputStream.size() == {0}", result));
            return result;
        }

        public byte[] toByteArray()
        {
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmToByteArray == IntPtr.Zero)
            {
                Debug.LogError("_jmToByteArray is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmToByteArray, new jvalue[0]);
            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get byte array");
                return null;
            }
            int count = AndroidJNI.GetArrayLength(result);
            byte[] retVal = new byte[count];
            for (int index = 0; index < count; ++index)
            {
                retVal[index] = AndroidJNI.GetByteArrayElement(result, index);
            }
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public IntPtr Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}

#endif