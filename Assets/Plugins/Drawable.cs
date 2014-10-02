#if UNITY_ANDROID && !UNITY_EDITOR
#pragma warning disable 0414

using Android.Runtime;
using System;
using UnityEngine;

namespace Android.Graphics.Drawables
{
    public class Drawable
    {
        static IntPtr _jcDrawable = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static Drawable()
        {
        }

        public static Drawable GetObject(IntPtr instance)
        {
            Drawable result = new Drawable();
            result._instance = instance;
            return result;
        }

        public IntPtr Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}

#endif