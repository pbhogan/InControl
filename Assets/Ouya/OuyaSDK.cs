/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// Unity JNI reference: http://docs.unity3d.com/Documentation/ScriptReference/AndroidJNI.html
// JNI Spec: http://docs.oracle.com/javase/1.5.0/docs/guide/jni/spec/jniTOC.html
// Android Plugins: http://docs.unity3d.com/Documentation/Manual/Plugins.html#AndroidPlugins 
// Unity Android Plugin Guide: http://docs.unity3d.com/Documentation/Manual/PluginsForAndroid.html

using System;
using System.Collections.Generic;
using UnityEngine;

public static class OuyaSDK
{
    public const string VERSION = "1.0.6.1";

    /// <summary>
    /// The developer ID assigned by OuyaGameObject
    /// </summary>
    static private string m_developerId = string.Empty;

    /// <summary>
    /// Use legacy input assigned by OuyaGameObject
    /// </summary>
    private static bool m_useLegacyInput = false;

    /// <summary>
    /// Inidicates IAP has been setup and is ready for processing
    /// </summary>
    private static bool m_iapInitComplete = false;

    /// <summary>
    /// Check if the device is an OUYA
    /// </summary>
    /// <returns></returns>
    public static bool IsOUYA()
    {
        if (SystemInfo.deviceModel.ToUpper().Contains("OUYA"))
        {
            return true;
        }

        return false;
    }

    #region Key Codes

    public const int KEYCODE_BUTTON_A = 96;
    public const int KEYCODE_BUTTON_B = 97;
    public const int KEYCODE_BUTTON_X = 99;
    public const int KEYCODE_BUTTON_Y = 100;
    public const int KEYCODE_BUTTON_L1 = 102;
    public const int KEYCODE_BUTTON_L2 = 104;
    public const int KEYCODE_BUTTON_R1 = 103;
    public const int KEYCODE_BUTTON_R2 = 105;
    public const int KEYCODE_BUTTON_L3 = 106;
    public const int KEYCODE_BUTTON_R3 = 107;
    public const int KEYCODE_BUTTON_SYSTEM = 108;
    public const int AXIS_X = 0;
    public const int AXIS_Y = 1;
    public const int AXIS_Z = 11;
    public const int AXIS_RZ = 14;
    public const int KEYCODE_DPAD_UP = 19;
    public const int KEYCODE_DPAD_DOWN = 20;
    public const int KEYCODE_DPAD_LEFT = 21;
    public const int KEYCODE_DPAD_RIGHT = 22;
    public const int KEYCODE_DPAD_CENTER = 23;

    //For EDITOR / PC / MAC
    public const int KEYCODE_BUTTON_SELECT = -1;
    public const int KEYCODE_BUTTON_START = -2;
    public const int KEYCODE_BUTTON_ESCAPE = 41;
    #endregion

    #region OUYA controller

    public const int BUTTON_O = KEYCODE_BUTTON_A;
    public const int BUTTON_U = KEYCODE_BUTTON_X;
    public const int BUTTON_Y = KEYCODE_BUTTON_Y;
    public const int BUTTON_A = KEYCODE_BUTTON_B;

    public const int BUTTON_LB = KEYCODE_BUTTON_L1;
    public const int BUTTON_LT = KEYCODE_BUTTON_L2;
    public const int BUTTON_RB = KEYCODE_BUTTON_R1;
    public const int BUTTON_RT = KEYCODE_BUTTON_R2;
    public const int BUTTON_L3 = KEYCODE_BUTTON_L3;
    public const int BUTTON_R3 = KEYCODE_BUTTON_R3;

    public const int BUTTON_SYSTEM = KEYCODE_BUTTON_SYSTEM;
    // for EDITOR / PC / MAC
    public const int BUTTON_START = KEYCODE_BUTTON_START;
    public const int BUTTON_SELECT = KEYCODE_BUTTON_SELECT;
    public const int BUTTON_ESCAPE = KEYCODE_BUTTON_ESCAPE;
    public const int AXIS_LSTICK_X = AXIS_X;
    public const int AXIS_LSTICK_Y = AXIS_Y;
    public const int AXIS_RSTICK_X = AXIS_Z;
    public const int AXIS_RSTICK_Y = AXIS_RZ;

    public const int BUTTON_DPAD_UP = KEYCODE_DPAD_UP;
    public const int BUTTON_DPAD_RIGHT = KEYCODE_DPAD_RIGHT;
    public const int BUTTON_DPAD_DOWN = KEYCODE_DPAD_DOWN;
    public const int BUTTON_DPAD_LEFT = KEYCODE_DPAD_LEFT;
    public const int BUTTON_DPAD_CENTER = KEYCODE_DPAD_CENTER;

    public enum InputAction
    {
        None,
        GenericMotionEvent,
        KeyDown,
        KeyUp,
        TouchEvent,
        TrackballEvent
    }

    public enum KeyEnum
    {
        NONE = -1,
        BUTTON_O = OuyaSDK.BUTTON_O,
        BUTTON_U = OuyaSDK.BUTTON_U,
        BUTTON_Y = OuyaSDK.KEYCODE_BUTTON_Y,
        BUTTON_A = OuyaSDK.BUTTON_A,
        BUTTON_LB = OuyaSDK.BUTTON_LB,
        BUTTON_LT = OuyaSDK.KEYCODE_BUTTON_L2,
        BUTTON_RB = OuyaSDK.BUTTON_RB,
        BUTTON_RT = OuyaSDK.BUTTON_RT,
        BUTTON_L3 = OuyaSDK.BUTTON_L3,
        BUTTON_R3 = OuyaSDK.BUTTON_R3,
        BUTTON_SYSTEM = OuyaSDK.BUTTON_SYSTEM,
        BUTTON_START = OuyaSDK.BUTTON_START,
        BUTTON_SELECT = OuyaSDK.BUTTON_SELECT,
        BUTTON_ESCAPE = OuyaSDK.BUTTON_ESCAPE,
        AXIS_LSTICK_X = OuyaSDK.AXIS_LSTICK_X,
        AXIS_LSTICK_Y = OuyaSDK.AXIS_LSTICK_Y,
        AXIS_RSTICK_X = OuyaSDK.AXIS_RSTICK_X,
        AXIS_RSTICK_Y = OuyaSDK.AXIS_RSTICK_Y,
        BUTTON_DPAD_UP = OuyaSDK.BUTTON_DPAD_UP,
        BUTTON_DPAD_RIGHT = OuyaSDK.BUTTON_DPAD_RIGHT,
        BUTTON_DPAD_DOWN = OuyaSDK.BUTTON_DPAD_DOWN,
        BUTTON_DPAD_LEFT = OuyaSDK.BUTTON_DPAD_LEFT,
        BUTTON_DPAD_CENTER = OuyaSDK.BUTTON_DPAD_CENTER,

        BUTTON_BACK,

        HARMONIX_ROCK_BAND_GUITAR_GREEN,
        HARMONIX_ROCK_BAND_GUITAR_RED,
        HARMONIX_ROCK_BAND_GUITAR_YELLOW,
        HARMONIX_ROCK_BAND_GUITAR_BLUE,
        HARMONIX_ROCK_BAND_GUITAR_ORANGE,
        HARMONIX_ROCK_BAND_GUITAR_LOWER,
        HARMONIX_ROCK_BAND_GUITAR_WHAMMI,
        HARMONIX_ROCK_BAND_GUITAR_PICKUP,
        HARMONIX_ROCK_BAND_GUITAR_STRUM,

        HARMONIX_ROCK_BAND_DRUMKIT_GREEN,
        HARMONIX_ROCK_BAND_DRUMKIT_RED,
        HARMONIX_ROCK_BAND_DRUMKIT_YELLOW,
        HARMONIX_ROCK_BAND_DRUMKIT_BLUE,
        HARMONIX_ROCK_BAND_DRUMKIT_ORANGE,
        HARMONIX_ROCK_BAND_DRUMKIT_A,
        HARMONIX_ROCK_BAND_DRUMKIT_B,
        HARMONIX_ROCK_BAND_DRUMKIT_X,
        HARMONIX_ROCK_BAND_DRUMKIT_Y,
    }

    public enum AxisEnum
    {
        NONE = -1,
        AXIS_LSTICK_X,
        AXIS_LSTICK_Y,
        AXIS_RSTICK_X,
        AXIS_RSTICK_Y,
        AXIS_LTRIGGER,
        AXIS_RTRIGGER,
    }

    public enum OuyaPlayer
    {
        player1=1,
        player2=2,
        player3=3,
        player4=4,
        player5=5,
        player6=6,
        player7=7,
        player8=8,
        player9=9,
        player10=10,
        player11=11,
        none=0,
    }

    #endregion

    /// <summary>
    /// Listener for the input buttons
    /// </summary>
    private static OuyaSDK.InputButtonListener<OuyaSDK.InputButtonEvent> m_inputButtonListener = null;
    public static OuyaSDK.InputButtonListener<OuyaSDK.InputButtonEvent> getInputButtonListener()
    {
        return m_inputButtonListener;
    }
    public static void registerInputButtonListener(OuyaSDK.InputButtonListener<OuyaSDK.InputButtonEvent> inputButtonListener)
    {
        m_inputButtonListener = inputButtonListener;
    }

    /// <summary>
    /// Listener for the input axis
    /// </summary>
    private static OuyaSDK.InputAxisListener<OuyaSDK.InputAxisEvent> m_inputAxisListener = null;
    public static OuyaSDK.InputAxisListener<OuyaSDK.InputAxisEvent> getInputAxisListener()
    {
        return m_inputAxisListener;
    }
    public static void registerInputAxisListener(OuyaSDK.InputAxisListener<OuyaSDK.InputAxisEvent> inputAxisListener)
    {
        m_inputAxisListener = inputAxisListener;
    }

    /// <summary>
    /// Initialized by OuyaGameObject
    /// </summary>
    /// <param name="developerId"></param>
    public static void initialize(string developerId, bool useLegacyInput)
    {
        m_developerId = developerId;
        OuyaSDK.OuyaJava.JavaSetDeveloperId();

        m_useLegacyInput = useLegacyInput;
        if (m_useLegacyInput)
        {
            OuyaSDK.OuyaJava.JavaUseLegacyInput();
        }

        OuyaSDK.OuyaJava.JavaUnityInitialized();
    }

    /// <summary>
    /// Access the developer id
    /// </summary>
    /// <returns></returns>
    public static string getDeveloperId()
    {
        return m_developerId;
    }

    public static bool isIAPInitComplete()
    {
        return m_iapInitComplete;
    }

    public static void setIAPInitComplete()
    {
        m_iapInitComplete = true;
    }

    #region Mirror Java API

    public class GenericListener<T>
    {
        public delegate void SuccessDelegate(T data);

        public SuccessDelegate onSuccess = null;

        public delegate void FailureDelegate(int errorCode, String errorMessage);

        public FailureDelegate onFailure = null;
    }

    public class CancelIgnoringIapResponseListener<T> : GenericListener<T>
    {
    }

    public static void fetchGamerUUID()
    {
        OuyaSDK.OuyaJava.JavaFetchGamerUUID();
    }

    public static void requestProductList(List<Purchasable> purchasables)
    {
        foreach (Purchasable purchasable in purchasables)
        {
            //Debug.Log(string.Format("Unity Adding: {0}", purchasable.getProductId()));
            OuyaSDK.OuyaJava.JavaAddGetProduct(purchasable);
        }

        OuyaSDK.OuyaJava.JavaGetProductsAsync();
    }

    public static void requestPurchase(Purchasable purchasable)
	{
        OuyaSDK.OuyaJava.JavaRequestPurchaseAsync(purchasable);
    }

    public static void requestReceiptList()
    {
        OuyaSDK.OuyaJava.JavaGetReceiptsAsync();
    }

    public class InputButtonListener<T> : GenericListener<T>
    {
    }

    public class InputAxisListener<T> : GenericListener<T>
    {
    }

    public class DeviceListener<T> : GenericListener<T>
    {
    }


    #endregion

    #region Data containers

    public class Purchasable
    {
        public string productId = string.Empty;
        public Purchasable(string productId)
        {
            this.productId = productId;
        }
        public string getProductId()
        {
            return productId;
        }
        public static implicit operator string(Purchasable rhs)
        {
            return rhs.getProductId();
        }
        public static implicit operator Purchasable(string rhs)
        {
            return new Purchasable(rhs);
        }
    }

    [Serializable]
    public class Product
    {
        public string identifier = string.Empty;
        public string name = string.Empty;
        public int priceInCents = 0;

        public string getIdentifier()
        {
            return identifier;
        }

        public string getName()
        {
            return name;
        }

        public int getPriceInCents()
        {
            return priceInCents;
        }

        public void setIdentifier(string identifier)
        {
            this.identifier = identifier;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setPriceInCents(int priceInCents)
        {
            this.priceInCents = priceInCents;
        }
    }

    [Serializable]
    public class Receipt
    {
        public string identifier = string.Empty;
        public int priceInCents = 0;
        public DateTime purchaseDate = DateTime.MinValue;
        public DateTime generatedDate = DateTime.MinValue;

        public string getIdentifier()
        {
            return identifier;
        }

        public int getPriceInCents()
        {
            return priceInCents;
        }

        public DateTime getPurchaseDate()
        {
            return purchaseDate;
        }

        public DateTime getGeneratedDate()
        {
            return generatedDate;
        }

        public void setIdentifier(string identifier)
        {
            this.identifier = identifier;
        }

        public void setPriceInCents(int priceInCents)
        {
            this.priceInCents = priceInCents;
        }

        public void setPurchaseDate(DateTime date)
        {
            purchaseDate = date;
        }

        public void setGeneratedDate(DateTime date)
        {
            generatedDate = date;
        }
    }

    public class InputButtonEvent
    {
        public InputButtonEvent(InputAction inputAction, KeyEnum keyCode, OuyaPlayer player)
        {
            m_inputAction = inputAction;
            m_keyCode = keyCode;
            m_player = player;
        }

        private InputAction m_inputAction = InputAction.None;
        public InputAction getKeyAction()
        {
            return m_inputAction;
        }

        private KeyEnum m_keyCode = 0;
        public KeyEnum getKeyCode()
        {
            return m_keyCode;
        }

        private OuyaPlayer m_player;
        public OuyaPlayer getPlayer()
        {
            return m_player;
        }
    }

    public class InputAxisEvent
    {
        private int m_keyCode;

        public InputAxisEvent(InputAction inputAction, OuyaSDK.AxisEnum axisEnum, float axis, OuyaPlayer player)
        {
            m_inputAction = inputAction;
            m_axisCode = axisEnum;
            m_axis = axis;
            m_player = player;
        }

        private InputAction m_inputAction = InputAction.None;
        public InputAction getInputAction()
        {
            return m_inputAction;
        }

        private AxisEnum m_axisCode = AxisEnum.NONE;
        public AxisEnum getAxisCode()
        {
            return m_axisCode;
        }

        private float m_axis = 0f;
        public float getAxis()
        {
            return m_axis;
        }

        private OuyaPlayer m_player;
        public OuyaPlayer getPlayer()
        {
            return m_player;
        }
    }

    #endregion

    #region Menu Button Up Listeners

    public interface IMenuButtonUpListener
    {
        void OuyaMenuButtonUp();
    }
    private static List<IMenuButtonUpListener> m_menuButtonUpListeners = new List<IMenuButtonUpListener>();
    public static List<IMenuButtonUpListener> getMenuButtonUpListeners()
    {
        return m_menuButtonUpListeners;
    }
    public static void registerMenuButtonUpListener(IMenuButtonUpListener listener)
    {
        if (!m_menuButtonUpListeners.Contains(listener))
        {
            m_menuButtonUpListeners.Add(listener);
        }
    }
    public static void unregisterMenuButtonUpListener(IMenuButtonUpListener listener)
    {
        if (m_menuButtonUpListeners.Contains(listener))
        {
            m_menuButtonUpListeners.Remove(listener);
        }
    }

    #endregion

    #region Menu Appearing Listeners

    public interface IMenuAppearingListener
    {
        void OuyaMenuAppearing();
    }
    private static List<IMenuAppearingListener> m_menuAppearingListeners = new List<IMenuAppearingListener>();
    public static List<IMenuAppearingListener> getMenuAppearingListeners()
    {
        return m_menuAppearingListeners;
    }
    public static void registerMenuAppearingListener(IMenuAppearingListener listener)
    {
        if (!m_menuAppearingListeners.Contains(listener))
        {
            m_menuAppearingListeners.Add(listener);
        }
    }
    public static void unregisterMenuAppearingListener(IMenuAppearingListener listener)
    {
        if (m_menuAppearingListeners.Contains(listener))
        {
            m_menuAppearingListeners.Remove(listener);
        }
    }

    #endregion

    #region Pause Listeners

    public interface IPauseListener
    {
        void OuyaOnPause();
    }
    private static List<IPauseListener> m_pauseListeners = new List<IPauseListener>();
    public static List<IPauseListener> getPauseListeners()
    {
        return m_pauseListeners;
    }
    public static void registerPauseListener(IPauseListener listener)
    {
        if (!m_pauseListeners.Contains(listener))
        {
            m_pauseListeners.Add(listener);
        }
    }
    public static void unregisterPauseListener(IPauseListener listener)
    {
        if (m_pauseListeners.Contains(listener))
        {
            m_pauseListeners.Remove(listener);
        }
    }

    #endregion

    #region Resume Listeners

    public interface IResumeListener
    {
        void OuyaOnResume();
    }
    private static List<IResumeListener> m_resumeListeners = new List<IResumeListener>();
    public static List<IResumeListener> getResumeListeners()
    {
        return m_resumeListeners;
    }
    public static void registerResumeListener(IResumeListener listener)
    {
        if (!m_resumeListeners.Contains(listener))
        {
            m_resumeListeners.Add(listener);
        }
    }
    public static void unregisterResumeListener(IResumeListener listener)
    {
        if (m_resumeListeners.Contains(listener))
        {
            m_resumeListeners.Remove(listener);
        }
    }

    #endregion

    #region Fetch Gamer UUID Listener

    public interface IFetchGamerUUIDListener
    {
        void OuyaFetchGamerUUIDOnSuccess(string gamerUUID);
        void OuyaFetchGamerUUIDOnFailure(int errorCode, string errorMessage);
        void OuyaFetchGamerUUIDOnCancel();
    }
    private static List<IFetchGamerUUIDListener> m_fetchGamerUUIDListeners = new List<IFetchGamerUUIDListener>();
    public static List<IFetchGamerUUIDListener> getFetchGamerUUIDListeners()
    {
        return m_fetchGamerUUIDListeners;
    }
    public static void registerFetchGamerUUIDListener(IFetchGamerUUIDListener listener)
    {
        if (!m_fetchGamerUUIDListeners.Contains(listener))
        {
            m_fetchGamerUUIDListeners.Add(listener);
        }
    }
    public static void unregisterFetchGamerUUIDListener(IFetchGamerUUIDListener listener)
    {
        if (m_fetchGamerUUIDListeners.Contains(listener))
        {
            m_fetchGamerUUIDListeners.Remove(listener);
        }
    }

    #endregion

    #region Get GetProducts Listeners

    public interface IGetProductsListener
    {
        void OuyaGetProductsOnSuccess(List<OuyaSDK.Product> products);
        void OuyaGetProductsOnFailure(int errorCode, string errorMessage);
        void OuyaGetProductsOnCancel();
    }
    private static List<IGetProductsListener> m_getProductsListeners = new List<IGetProductsListener>();
    public static List<IGetProductsListener> getGetProductsListeners()
    {
        return m_getProductsListeners;
    }
    public static void registerGetProductsListener(IGetProductsListener listener)
    {
        if (!m_getProductsListeners.Contains(listener))
        {
            m_getProductsListeners.Add(listener);
        }
    }
    public static void unregisterGetProductsListener(IGetProductsListener listener)
    {
        if (m_getProductsListeners.Contains(listener))
        {
            m_getProductsListeners.Remove(listener);
        }
    }

    #endregion

    #region Purchase Listener

    public interface IPurchaseListener
    {
        void OuyaPurchaseOnSuccess(OuyaSDK.Product product);
        void OuyaPurchaseOnFailure(int errorCode, string errorMessage);
        void OuyaPurchaseOnCancel();
    }
    private static List<IPurchaseListener> m_purchaseListeners = new List<IPurchaseListener>();
    public static List<IPurchaseListener> getPurchaseListeners()
    {
        return m_purchaseListeners;
    }
    public static void registerPurchaseListener(IPurchaseListener listener)
    {
        if (!m_purchaseListeners.Contains(listener))
        {
            m_purchaseListeners.Add(listener);
        }
    }
    public static void unregisterPurchaseListener(IPurchaseListener listener)
    {
        if (m_purchaseListeners.Contains(listener))
        {
            m_purchaseListeners.Remove(listener);
        }
    }

    #endregion

    #region Get GetReceipts Listeners

    public interface IGetReceiptsListener
    {
        void OuyaGetReceiptsOnSuccess(List<Receipt> receipts);
        void OuyaGetReceiptsOnFailure(int errorCode, string errorMessage);
        void OuyaGetReceiptsOnCancel();
    }
    private static List<IGetReceiptsListener> m_getReceiptsListeners = new List<IGetReceiptsListener>();
    public static List<IGetReceiptsListener> getGetReceiptsListeners()
    {
        return m_getReceiptsListeners;
    }
    public static void registerGetReceiptsListener(IGetReceiptsListener listener)
    {
        if (!m_getReceiptsListeners.Contains(listener))
        {
            m_getReceiptsListeners.Add(listener);
        }
    }
    public static void unregisterGetReceiptsListener(IGetReceiptsListener listener)
    {
        if (m_getReceiptsListeners.Contains(listener))
        {
            m_getReceiptsListeners.Remove(listener);
        }
    }

    #endregion

    #region Java Interface

    public class OuyaJava
    {
        private const string JAVA_CLASS = "tv.ouya.sdk.OuyaUnityPlugin";

        public static void JavaInit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
            AndroidJNI.AttachCurrentThread();

            // first we try to find our main activity..
            IntPtr cls_Activity = AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
            IntPtr fid_Activity = AndroidJNI.GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
            IntPtr obj_Activity = AndroidJNI.GetStaticObjectField(cls_Activity, fid_Activity);
            Debug.Log("obj_Activity = " + obj_Activity);

            // create a JavaClass object...
            IntPtr cls_JavaClass = AndroidJNI.FindClass("tv/ouya/sdk/OuyaUnityPlugin");
            IntPtr mid_JavaClass = AndroidJNI.GetMethodID(cls_JavaClass, "<init>", "(Landroid/app/Activity;)V");
            IntPtr obj_JavaClass = AndroidJNI.NewObject(cls_JavaClass, mid_JavaClass, new jvalue[] { new jvalue() { l = obj_Activity } });
            Debug.Log("JavaClass object = " + obj_JavaClass);
#endif
        }

        public static void JavaSetDeveloperId()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaSetDeveloperId");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic<String>("setDeveloperId", new object[] { m_developerId + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaSetDeveloperId exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaUseLegacyInput()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("useLegacyInput");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("useLegacyInput");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaUseLegacyInput exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaUnityInitialized()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaUnityInitialized");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("unityInitialized");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaUnityInitialized exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaSetResolution(string resolutionId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaSetResolution");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("setResolution", resolutionId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaSetResolution exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaFetchGamerUUID()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaFetchGamerUUID", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("fetchGamerUUID");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaFetchGamerUUID exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaAddGetProduct(Purchasable purchasable)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaAddGetProduct", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("addGetProduct", purchasable.getProductId());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaAddGetProduct exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaDebugGetProductList()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaDebugGetProductList", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("debugGetProductList");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaDebugGetProductList exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaClearGetProductList()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaClearGetProductList", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("clearGetProductList");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaClearGetProductList exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaGetProductsAsync()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("OuyaSDK.JavaGetProductsAsync");

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("getProductsAsync");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaGetProductsAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaRequestPurchaseAsync(Purchasable purchasable)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("JavaRequestPurchaseAsync purchasable: {0}", purchasable.getProductId()));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic<String>("requestPurchaseAsync", new object[] { purchasable.getProductId() + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaRequestPurchaseAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaGetReceiptsAsync()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("OuyaSDK.JavaGetReceiptsAsync");

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("getReceiptsAsync");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaGetReceiptsAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }
    }

    #endregion
}