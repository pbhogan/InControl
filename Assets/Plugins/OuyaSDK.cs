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
using System.Runtime.InteropServices;

#if UNITY_ANDROID && !UNITY_EDITOR
#pragma warning disable 0414
using com.unity3d.player;
using org.json;
using tv.ouya.console.api;
using tv.ouya.sdk;
#endif
using UnityEngine;

public static class OuyaSDK
{
	public const string VERSION = "1.0.14.1";

	#if UNITY_ANDROID && !UNITY_EDITOR
    
    private static OuyaUnityPlugin m_ouyaUnityPlugin = null;

    static OuyaSDK()
    {
        // attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
        AndroidJNI.AttachCurrentThread();

        m_ouyaUnityPlugin = new OuyaUnityPlugin(UnityPlayer.currentActivity);
    }

    public class NdkWrapper
    {
        [DllImport("lib-ouya-ndk")]
        // EXPORT_API float getAxis(int deviceId, int axis)
        public static extern float getAxis(int deviceId, int axis);

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API bool isPressed(int deviceId, int keyCode)
        public static extern bool isPressed(int deviceId, int keyCode);

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API bool isPressedDown(int deviceId, int keyCode)
        public static extern bool isPressedDown(int deviceId, int keyCode);

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API bool isPressedUp(int deviceId, int keyCode)
        public static extern bool isPressedUp(int deviceId, int keyCode);

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API void clearButtonStates()
        public static extern void clearButtonStates();
    }
#endif

	#if UNITY_ANDROID && !UNITY_EDITOR
	
    public class OuyaInput
    {
        

#region Private API

	
        private static object m_lockObject = new object();
        private static List<Dictionary<int, float>> m_axisStates = new List<Dictionary<int, float>>();
        private static List<Dictionary<int, bool>> m_buttonStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonDownStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonUpStates = new List<Dictionary<int, bool>>();

        static OuyaInput()
        {
            for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
            {
                m_axisStates.Add(new Dictionary<int, float>());
                m_buttonStates.Add(new Dictionary<int, bool>());
                m_buttonDownStates.Add(new Dictionary<int, bool>());
                m_buttonUpStates.Add(new Dictionary<int, bool>());
            }
        }

        private static float GetState(int axis, Dictionary<int, float> dictionary)
        {
            float result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(axis))
                {
                    result = dictionary[axis];
                }
                else
                {
                    result = 0f;
                }
            }
            return result;
        }

        private static bool GetState(int button, Dictionary<int, bool> dictionary)
        {
            bool result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(button))
                {
                    result = dictionary[button];
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private static bool GetState(bool isPressed, int button, Dictionary<int, bool> dictionary)
        {
            bool result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(button))
                {
                    result = isPressed == dictionary[button];
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public static void UpdateInputFrame()
        {
            lock (m_lockObject)
            { 
                for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
                {
                    

#region Track Axis States

	
                    Dictionary<int, float> axisState = m_axisStates[deviceId];
                    axisState[OuyaController.AXIS_LS_X] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_LS_X);
                    axisState[OuyaController.AXIS_LS_Y] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_LS_Y);
                    axisState[OuyaController.AXIS_RS_X] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_RS_X);
                    axisState[OuyaController.AXIS_RS_Y] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_RS_Y);
                    axisState[OuyaController.AXIS_L2] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_L2);
                    axisState[OuyaController.AXIS_R2] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_R2);

                    

#endregion

	
                    

#region Track Button Up / Down States

	
                    Dictionary<int, bool> buttonState = m_buttonStates[deviceId];
                    Dictionary<int, bool> buttonDownState = m_buttonDownStates[deviceId];
                    Dictionary<int, bool> buttonUpState = m_buttonUpStates[deviceId];

                    buttonState[OuyaController.BUTTON_O] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_O);
                    buttonState[OuyaController.BUTTON_U] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_U);
                    buttonState[OuyaController.BUTTON_Y] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_Y);
                    buttonState[OuyaController.BUTTON_A] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_A);
                    buttonState[OuyaController.BUTTON_L1] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_L1);
                    buttonState[OuyaController.BUTTON_R1] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_R1);
                    buttonState[OuyaController.BUTTON_L3] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_L3);
                    buttonState[OuyaController.BUTTON_R3] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_R3);
                    buttonState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_UP);
                    buttonState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                    buttonState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                    buttonState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                    buttonState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_MENU);

                    buttonDownState[OuyaController.BUTTON_O] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_O);
                    buttonDownState[OuyaController.BUTTON_U] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_U);
                    buttonDownState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_Y);
                    buttonDownState[OuyaController.BUTTON_A] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_A);
                    buttonDownState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_L1);
                    buttonDownState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_R1);
                    buttonDownState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_L3);
                    buttonDownState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_R3);
                    buttonDownState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_UP);
                    buttonDownState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                    buttonDownState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                    buttonDownState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                    buttonDownState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_MENU);

                    buttonUpState[OuyaController.BUTTON_O] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_O);
                    buttonUpState[OuyaController.BUTTON_U] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_U);
                    buttonUpState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_Y);
                    buttonUpState[OuyaController.BUTTON_A] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_A);
                    buttonUpState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L1);
                    buttonUpState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R1);
                    buttonUpState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L3);
                    buttonUpState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R3);
                    buttonUpState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_UP);
                    buttonUpState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                    buttonUpState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                    buttonUpState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                    buttonUpState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_MENU);

                    

#endregion

	
                    //debugOuyaController(deviceId);
                }
            }
        }

        public static void ClearButtonStates()
        {
            NdkWrapper.clearButtonStates();
        }

        private static void debugOuyaController(int deviceId, int button)
        {
            if (GetButtonDown(deviceId, button))
            {
                Debug.Log("Device=" + deviceId + " GetButtonDown: " + button);
            }

            if (GetButtonUp(deviceId, button))
            {
                Debug.Log("Device=" + deviceId + " GetButtonUp: " + button);
            }
        }

        private static void debugOuyaController(int deviceId)
        {
            debugOuyaController(deviceId, OuyaController.BUTTON_O);
            debugOuyaController(deviceId, OuyaController.BUTTON_U);
            debugOuyaController(deviceId, OuyaController.BUTTON_Y);
            debugOuyaController(deviceId, OuyaController.BUTTON_A);
            debugOuyaController(deviceId, OuyaController.BUTTON_L1);
            debugOuyaController(deviceId, OuyaController.BUTTON_R1);
            debugOuyaController(deviceId, OuyaController.BUTTON_L3);
            debugOuyaController(deviceId, OuyaController.BUTTON_R3);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_UP);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_DOWN);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_LEFT);
            debugOuyaController(deviceId, OuyaController.BUTTON_MENU);
        }

        

#endregion

	
        

#region Public API

	
        public static bool IsControllerConnected(int playerNum)
        {
            if (playerNum >= 0 &&
                null != OuyaSDK.Joysticks &&
                playerNum < OuyaSDK.Joysticks.Length)
            {
                return (null != OuyaSDK.Joysticks[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static float GetAxis(int playerNum, int axis)
        {
            if (playerNum >= 0 &&
                null != m_axisStates &&
                playerNum < m_axisStates.Count)
            {
                return GetState(axis, m_axisStates[playerNum]);
            }
            else
            {
                return 0f;
            }
        }

        public static float GetAxisRaw(int playerNum, int axis)
        {
            if (playerNum >= 0 &&
                null != m_axisStates &&
                playerNum < m_axisStates.Count)
            {
                return GetState(axis, m_axisStates[playerNum]);
            }
            else
            {
                return 0f;
            }
        }

        public static bool GetButton(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonStates &&
                playerNum < m_buttonStates.Count)
            {
                return GetState(button, m_buttonStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static bool GetButtonDown(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonDownStates &&
                playerNum < m_buttonDownStates.Count)
            {
                return GetState(button, m_buttonDownStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static bool GetButtonUp(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonUpStates &&
                playerNum < m_buttonUpStates.Count)
            {
                return GetState(button, m_buttonUpStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        

#endregion

    }

#endif

	#if UNITY_ANDROID && !UNITY_EDITOR
    /// <summary>
    /// Cache joysticks
    /// </summary>
    public static string[] Joysticks = null;

    /// <summary>
    /// Query joysticks every N seconds
    /// </summary>
    private static DateTime m_timerJoysticks = DateTime.MinValue;

    private static string getDeviceName(int deviceId)
    {
        OuyaController ouyaController = OuyaController.getControllerByPlayer(deviceId);
        if (null != ouyaController)
        {
            return ouyaController.getDeviceName();
        }
        return null;
    }
#endif

	/// <summary>
	/// Update joysticks with a timer
	/// </summary>
	public static void UpdateJoysticks()
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		return;
#else
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

            string[] joysticks = null;
            List<string> devices = new List<string>();
            for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
            {
                string deviceName = getDeviceName(deviceId);
                //Debug.Log(string.Format("Device={0} name={1}", deviceId, deviceName));
                devices.Add(deviceName);
            }
            joysticks = devices.ToArray();

            // look for changes
            bool detectedChange = false;

            if (null == Joysticks)
            {
                detectedChange = true;
            }
            else if (joysticks.Length != Joysticks.Length)
            {
                detectedChange = true;
            }
            else
            {
                for (int index = 0; index < joysticks.Length; ++index)
                {
                    if (joysticks[index] != Joysticks[index])
                    {
                        detectedChange = true;
                        break;
                    }
                }
            }

            Joysticks = joysticks;

            if (detectedChange)
            {
                foreach (OuyaSDK.IJoystickCalibrationListener listener in OuyaSDK.getJoystickCalibrationListeners())
                {
                    //Debug.Log("OuyaGameObject: Invoke OuyaOnJoystickCalibration");
                    listener.OuyaOnJoystickCalibration();
                }
            }
        }
#endif
	}

	/// <summary>
	/// The developer ID assigned by OuyaGameObject
	/// </summary>
	static private string m_developerId = string.Empty;

	/// <summary>
	/// Inidicates IAP has been setup and is ready for processing
	/// </summary>
	private static bool m_iapInitComplete = false;

	/// <summary>
	/// Initialized by OuyaGameObject
	/// </summary>
	/// <param name="developerId"></param>
	public static void initialize( string developerId )
	{
		m_developerId = developerId;

#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.setDeveloperId(developerId);
        OuyaUnityPlugin.unityInitialized();
#endif
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

	public static void fetchGamerInfo()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.fetchGamerInfo();
#endif
	}

	public static void putGameData( string key, string val )
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.putGameData(key, val);
#endif
	}

	public static string getGameData( string key )
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.getGameData(key);
#else
		return String.Empty;
#endif
	}

	public static void requestProductList( List<Purchasable> purchasables )
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        foreach (Purchasable purchasable in purchasables)
        {
            //Debug.Log(string.Format("Unity Adding: {0}", purchasable.getProductId()));
            OuyaUnityPlugin.addGetProduct(purchasable.productId);
        }
        OuyaUnityPlugin.getProductsAsync();
#endif
	}

	public static void requestPurchase( Purchasable purchasable )
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.requestPurchaseAsync(purchasable.productId);
#endif
	}

	public static void requestReceiptList()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.getReceiptsAsync();
#endif
	}

	public static bool isRunningOnOUYASupportedHardware()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.isRunningOnOUYASupportedHardware();
#else
		return false;
#endif
	}

	#endregion

	#region Data containers

	[Serializable]
	public class GamerInfo
	{
		public string uuid = string.Empty;
		public string username = string.Empty;

		public static GamerInfo Parse( string jsonData )
		{
			GamerInfo result = new GamerInfo();
#if UNITY_ANDROID && !UNITY_EDITOR
            //Debug.Log(jsonData);
            using (JSONObject json = new JSONObject(jsonData))
            {
                if (json.has("uuid"))
                {
                    result.uuid = json.getString("uuid");
                }
                if (json.has("username"))
                {
                    result.username = json.getString("username");
                }
            }
#endif
			return result;
		}
	}

	[Serializable]
	public class Purchasable
	{
		public string productId = string.Empty;
	}

	[Serializable]
	public class Product
	{
		public string currencyCode = string.Empty;
		public string description = string.Empty;
		public string identifier = string.Empty;
		public float localPrice = 0f;
		public string name = string.Empty;
		public float originalPrice = 0f;
		public float percentOff = 0f;
		public string developerName = string.Empty;

		public static Product Parse( string jsonData )
		{
			Product result = new Product();
#if UNITY_ANDROID && !UNITY_EDITOR
            //Debug.Log(jsonData);
            using (JSONObject json = new JSONObject(jsonData))
            {
                if (json.has("currencyCode"))
                {
                    result.currencyCode = json.getString("currencyCode");
                }
                if (json.has("description"))
                {
                    result.description = json.getString("description");
                }
                if (json.has("identifier"))
                {
                    result.identifier = json.getString("identifier");
                }
                if (json.has("localPrice"))
                {
                    result.localPrice = (float) json.getDouble("localPrice");
                }
                if (json.has("name"))
                {
                    result.name = json.getString("name");
                }
                if (json.has("originalPrice"))
                {
                    result.originalPrice = (float) json.getDouble("originalPrice");
                }
                if (json.has("percentOff"))
                {
                    result.percentOff = (float) json.getDouble("percentOff");
                }
                if (json.has("developerName"))
                {
                    result.developerName = json.getString("developerName");
                }
            }
#endif
			return result;
		}
	}

	[Serializable]
	public class Receipt
	{
		public string currency = string.Empty;
		public string gamer = string.Empty;
		public DateTime generatedDate = DateTime.MinValue;
		public string identifier = string.Empty;
		public float localPrice = 0f;
		public int priceInCents = 0;
		public DateTime purchaseDate = DateTime.MinValue;
		public string uuid = string.Empty;

		public static Receipt Parse( string jsonData )
		{
			Receipt result = new Receipt();
#if UNITY_ANDROID && !UNITY_EDITOR
            //Debug.Log(jsonData);
            using (JSONObject json = new JSONObject(jsonData))
            {
                if (json.has("identifier"))
                {
                    result.identifier = json.getString("identifier");
                }
                if (json.has("purchaseDate"))
                {
                    DateTime date;
                    DateTime.TryParse(json.getString("purchaseDate"), out date);
                    result.purchaseDate = date;
                }
                if (json.has("gamer"))
                {
                    result.gamer = json.getString("gamer");
                }
                if (json.has("localPrice"))
                {
                    result.localPrice = (float) json.getDouble("localPrice");
                }
                if (json.has("uuid"))
                {
                    result.uuid = json.getString("uuid");
                }
                if (json.has("localPrice"))
                {
                    result.localPrice = (float) json.getDouble("localPrice");
                }
                if (json.has("currency"))
                {
                    result.currency = json.getString("currency");
                }
                if (json.has("generatedDate"))
                {
                    DateTime date;
                    DateTime.TryParse(json.getString("generatedDate"), out date);
                    result.generatedDate = date;
                }
            }
#endif
			return result;
		}
	}

	#endregion

	#region Joystick Callibration Listeners

	public interface IJoystickCalibrationListener
	{
		void OuyaOnJoystickCalibration();
	}
	private static List<IJoystickCalibrationListener> m_joystickCalibrationListeners = new List<IJoystickCalibrationListener>();
	public static List<IJoystickCalibrationListener> getJoystickCalibrationListeners()
	{
		return m_joystickCalibrationListeners;
	}
	public static void registerJoystickCalibrationListener( IJoystickCalibrationListener listener )
	{
		if (!m_joystickCalibrationListeners.Contains( listener ))
		{
			m_joystickCalibrationListeners.Add( listener );
		}
	}
	public static void unregisterJoystickCalibrationListener( IJoystickCalibrationListener listener )
	{
		if (m_joystickCalibrationListeners.Contains( listener ))
		{
			m_joystickCalibrationListeners.Remove( listener );
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
	public static void registerMenuAppearingListener( IMenuAppearingListener listener )
	{
		if (!m_menuAppearingListeners.Contains( listener ))
		{
			m_menuAppearingListeners.Add( listener );
		}
	}
	public static void unregisterMenuAppearingListener( IMenuAppearingListener listener )
	{
		if (m_menuAppearingListeners.Contains( listener ))
		{
			m_menuAppearingListeners.Remove( listener );
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
	public static void registerPauseListener( IPauseListener listener )
	{
		if (!m_pauseListeners.Contains( listener ))
		{
			m_pauseListeners.Add( listener );
		}
	}
	public static void unregisterPauseListener( IPauseListener listener )
	{
		if (m_pauseListeners.Contains( listener ))
		{
			m_pauseListeners.Remove( listener );
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
	public static void registerResumeListener( IResumeListener listener )
	{
		if (!m_resumeListeners.Contains( listener ))
		{
			m_resumeListeners.Add( listener );
		}
	}
	public static void unregisterResumeListener( IResumeListener listener )
	{
		if (m_resumeListeners.Contains( listener ))
		{
			m_resumeListeners.Remove( listener );
		}
	}

	#endregion

	#region Fetch Gamer UUID Listener

	public interface IFetchGamerInfoListener
	{
		void OuyaFetchGamerInfoOnSuccess( string uuid, string username );
		void OuyaFetchGamerInfoOnFailure( int errorCode, string errorMessage );
		void OuyaFetchGamerInfoOnCancel();
	}
	private static List<IFetchGamerInfoListener> m_FetchGamerInfoListeners = new List<IFetchGamerInfoListener>();
	public static List<IFetchGamerInfoListener> getFetchGamerInfoListeners()
	{
		return m_FetchGamerInfoListeners;
	}
	public static void registerFetchGamerInfoListener( IFetchGamerInfoListener listener )
	{
		if (!m_FetchGamerInfoListeners.Contains( listener ))
		{
			m_FetchGamerInfoListeners.Add( listener );
		}
	}
	public static void unregisterFetchGamerInfoListener( IFetchGamerInfoListener listener )
	{
		if (m_FetchGamerInfoListeners.Contains( listener ))
		{
			m_FetchGamerInfoListeners.Remove( listener );
		}
	}

	#endregion

	#region Get GetProducts Listeners

	public interface IGetProductsListener
	{
		void OuyaGetProductsOnSuccess( List<OuyaSDK.Product> products );
		void OuyaGetProductsOnFailure( int errorCode, string errorMessage );
		void OuyaGetProductsOnCancel();
	}
	private static List<IGetProductsListener> m_getProductsListeners = new List<IGetProductsListener>();
	public static List<IGetProductsListener> getGetProductsListeners()
	{
		return m_getProductsListeners;
	}
	public static void registerGetProductsListener( IGetProductsListener listener )
	{
		if (!m_getProductsListeners.Contains( listener ))
		{
			m_getProductsListeners.Add( listener );
		}
	}
	public static void unregisterGetProductsListener( IGetProductsListener listener )
	{
		if (m_getProductsListeners.Contains( listener ))
		{
			m_getProductsListeners.Remove( listener );
		}
	}

	#endregion

	#region Purchase Listener

	public interface IPurchaseListener
	{
		void OuyaPurchaseOnSuccess( OuyaSDK.Product product );
		void OuyaPurchaseOnFailure( int errorCode, string errorMessage );
		void OuyaPurchaseOnCancel();
	}
	private static List<IPurchaseListener> m_purchaseListeners = new List<IPurchaseListener>();
	public static List<IPurchaseListener> getPurchaseListeners()
	{
		return m_purchaseListeners;
	}
	public static void registerPurchaseListener( IPurchaseListener listener )
	{
		if (!m_purchaseListeners.Contains( listener ))
		{
			m_purchaseListeners.Add( listener );
		}
	}
	public static void unregisterPurchaseListener( IPurchaseListener listener )
	{
		if (m_purchaseListeners.Contains( listener ))
		{
			m_purchaseListeners.Remove( listener );
		}
	}

	#endregion

	#region Get GetReceipts Listeners

	public interface IGetReceiptsListener
	{
		void OuyaGetReceiptsOnSuccess( List<Receipt> receipts );
		void OuyaGetReceiptsOnFailure( int errorCode, string errorMessage );
		void OuyaGetReceiptsOnCancel();
	}
	private static List<IGetReceiptsListener> m_getReceiptsListeners = new List<IGetReceiptsListener>();
	public static List<IGetReceiptsListener> getGetReceiptsListeners()
	{
		return m_getReceiptsListeners;
	}
	public static void registerGetReceiptsListener( IGetReceiptsListener listener )
	{
		if (!m_getReceiptsListeners.Contains( listener ))
		{
			m_getReceiptsListeners.Add( listener );
		}
	}
	public static void unregisterGetReceiptsListener( IGetReceiptsListener listener )
	{
		if (m_getReceiptsListeners.Contains( listener ))
		{
			m_getReceiptsListeners.Remove( listener );
		}
	}

	#endregion
}