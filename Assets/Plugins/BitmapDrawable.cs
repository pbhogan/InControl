#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using UnityEngine;

namespace Android.Graphics.Drawables
{
    public class BitmapDrawable
    {
        static IntPtr _jcBitmapDrawable = IntPtr.Zero;
        static IntPtr _jmGetBitmap = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static BitmapDrawable()
        {
            try
            {
                {
                    string strName = "android/graphics/drawable/BitmapDrawable";
                    _jcBitmapDrawable = AndroidJNI.FindClass(strName);
                    if (_jcBitmapDrawable != IntPtr.Zero)
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
                    string strMethod = "getBitmap";
                    _jmGetBitmap = AndroidJNI.GetMethodID(_jcBitmapDrawable, strMethod, "()Landroid/graphics/Bitmap;");
                    if (_jmGetBitmap != IntPtr.Zero)
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

        public static explicit operator BitmapDrawable(Drawable drawable)
        {
            BitmapDrawable newDrawable = new BitmapDrawable();
            newDrawable.Instance = drawable.Instance;
            return newDrawable;
        }

        public Bitmap getBitmap()
        {
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmGetBitmap == IntPtr.Zero)
            {
                Debug.LogError("_jmGetBitmap is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetBitmap, new jvalue[0]);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get bitmap");
                return null;
            }
            Bitmap retVal = new Bitmap();
            retVal.Instance = result;
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