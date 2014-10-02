#if UNITY_ANDROID && !UNITY_EDITOR

using java.io;
using System;
using System.IO;
using UnityEngine;

namespace Android.Graphics
{
    public class Bitmap
    {
        public class CompressFormat
        {
            static IntPtr _jcCompressFormat = IntPtr.Zero;
            static IntPtr _jfPNG = IntPtr.Zero;
            private IntPtr _instance = IntPtr.Zero;

            static CompressFormat()
            {
                try
                {
                    {
                        string strName = "android/graphics/Bitmap$CompressFormat";
                        _jcCompressFormat = AndroidJNI.FindClass(strName);
                        if (_jcCompressFormat != IntPtr.Zero)
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
                        string strField = "PNG";
                        _jfPNG = AndroidJNI.GetStaticFieldID(_jcCompressFormat, strField, "Landroid/graphics/Bitmap$CompressFormat;");
                        if (_jfPNG != IntPtr.Zero)
                        {
                            Debug.Log(string.Format("Found {0} field", strField));
                        }
                        else
                        {
                            Debug.LogError(string.Format("Failed to find {0} field", strField));
                            return;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
                }
            }

            public static CompressFormat GetObject(IntPtr instance)
            {
                CompressFormat result = new CompressFormat();
                result._instance = instance;
                return result;
            }

            public static Bitmap.CompressFormat PNG
            {
                get
                {
                    if (_jcCompressFormat == IntPtr.Zero)
                    {
                        Debug.LogError("_jcCompressFormat is not initialized");
                        return null;
                    }
                    IntPtr result = AndroidJNI.GetStaticObjectField(_jcCompressFormat, _jfPNG);
                    if (result == IntPtr.Zero)
                    {
                        Debug.LogError("Failed to get PNG field");
                        return null;
                    }
                    return CompressFormat.GetObject(result);
                }
            }

            public IntPtr Instance
            {
                get { return _instance; }
                set { _instance = value; }
            }
        }

        static IntPtr _jcBitmap = IntPtr.Zero;
        static IntPtr _jmCompress = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static Bitmap()
        {
            try
            {
                {
                    string strName = "android/graphics/Bitmap";
                    _jcBitmap = AndroidJNI.FindClass(strName);
                    if (_jcBitmap != IntPtr.Zero)
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
                    string strMethod = "compress";
                    _jmCompress = AndroidJNI.GetMethodID(_jcBitmap, strMethod, "(Landroid/graphics/Bitmap$CompressFormat;ILjava/io/OutputStream;)Z");
                    if (_jmCompress != IntPtr.Zero)
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

        public void compress(Bitmap.CompressFormat format, int quality, ByteArrayOutputStream stream)
        {
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }
            if (_jmCompress == IntPtr.Zero)
            {
                Debug.LogError("_jmCompress is not initialized");
                return;
            }
            AndroidJNI.CallVoidMethod(_instance, _jmCompress, new jvalue[] { new jvalue() { l = format.Instance }, new jvalue() { i = quality }, new jvalue() { l = stream.Instance } });
        }

        public IntPtr Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}

#endif