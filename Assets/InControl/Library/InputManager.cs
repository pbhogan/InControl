using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace InControl
{
	public class InputManager
	{
		public static readonly VersionInfo Version = new VersionInfo();

		public delegate void DeviceEventHandler( InputDevice device );
		public static event DeviceEventHandler OnDeviceAttached;
		public static event DeviceEventHandler OnDeviceDetached;
		public static event DeviceEventHandler OnActiveDeviceChanged;

		static List<InputDeviceManager> inputDeviceManagers = new List<InputDeviceManager>();

		static InputDevice activeDevice = InputDevice.Null;
		public static List<InputDevice> Devices = new List<InputDevice>();

		public static string Platform { get; private set; }

		static bool invertYAxis = false; // default to y-axis up.
		static bool enableXInput = false;
		static bool isSetup = false;

		static float initialTime;
		static float currentTime;
		static float lastUpdateTime;

		static ulong currentTick;


		public static void Setup()
		{
			isSetup = false;

			Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

			initialTime = 0.0f;
			currentTime = 0.0f;
			lastUpdateTime = 0.0f;

			currentTick = 0;

			inputDeviceManagers.Clear();
			Devices.Clear();
			activeDevice = InputDevice.Null;

			OnDeviceAttached = null;
			OnDeviceDetached = null;
			OnActiveDeviceChanged = null;

			isSetup = true;

			#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			if (enableXInput)
			{
				if (Application.platform == RuntimePlatform.WindowsPlayer ||
					Application.platform == RuntimePlatform.WindowsEditor)
				{
					HideDevicesWithProfile( typeof( Xbox360WinProfile ) );
					HideDevicesWithProfile( typeof( LogitechF710ModeXWinProfile ) );
					InputManager.AddDeviceManager( new XInputDeviceManager() );
				}
			}
			#endif

			AddDeviceManager( new UnityInputDeviceManager() );
		}


		static void AssertIsSetup()
		{
			if (!isSetup)
			{
				throw new Exception( "InputManager is not initialized. Call InputManager.Setup() first." );
			}
		}


		public static void Update()
		{
			AssertIsSetup();

			currentTick++;

			UpdateCurrentTime();
			UpdateDeviceManagers();
			UpdateDevices();
			UpdateActiveDevice();

			lastUpdateTime = currentTime;
		}


		static void UpdateActiveDevice()
		{
			var lastActiveDevice = ActiveDevice;

			int deviceCount = Devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var inputDevice = Devices[i];
				if (ActiveDevice == InputDevice.Null ||
					inputDevice.LastChangedAfter( ActiveDevice ))
				{
					ActiveDevice = inputDevice;
				}
			}

			if (lastActiveDevice != ActiveDevice)
			{
				if (OnActiveDeviceChanged != null)
				{
					OnActiveDeviceChanged( ActiveDevice );
				}
			}
		}


		public static void AddDeviceManager( InputDeviceManager inputDeviceManager )
		{
			AssertIsSetup();

			inputDeviceManagers.Add( inputDeviceManager );
			inputDeviceManager.Update( currentTick, currentTime - lastUpdateTime );
		}


		static void UpdateCurrentTime()
		{
			// Have to do this hack since Time.realtimeSinceStartup is not set until AFTER Awake().
			if (initialTime < float.Epsilon)
			{
				initialTime = Time.realtimeSinceStartup;
			}

			currentTime = Mathf.Max( 0.0f, Time.realtimeSinceStartup - initialTime );
		}


		static void UpdateDeviceManagers()
		{
			var deltaTime = currentTime - lastUpdateTime;

			int inputDeviceManagerCount = inputDeviceManagers.Count;
			for (int i = 0; i < inputDeviceManagerCount; i++)
			{
				var inputDeviceManager = inputDeviceManagers[i];
				inputDeviceManager.Update( currentTick, deltaTime );
			}
		}


		static void UpdateDevices()
		{
			var deltaTime = currentTime - lastUpdateTime;

			MenuWasPressed = false;

			int deviceCount = Devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = Devices[i];

				device.PreUpdate( currentTick, deltaTime );
				device.Update( currentTick, deltaTime );
				device.PostUpdate( currentTick, deltaTime );

				if (device.MenuWasPressed)
				{
					MenuWasPressed = true;
				}
			}
		}


		public static void AttachDevice( InputDevice inputDevice )
		{
			AssertIsSetup();

			if (!inputDevice.IsSupportedOnThisPlatform)
			{
				return;
			}

			Devices.Add( inputDevice );
			Devices.Sort( ( d1, d2 ) => d1.SortOrder.CompareTo( d2.SortOrder ) );

			if (OnDeviceAttached != null)
			{
				OnDeviceAttached( inputDevice );
			}

			if (ActiveDevice == InputDevice.Null)
			{
				ActiveDevice = inputDevice;
			}
		}


		public static void DetachDevice( InputDevice inputDevice )
		{
			AssertIsSetup();

			Devices.Remove( inputDevice );
			Devices.Sort( ( d1, d2 ) => d1.SortOrder.CompareTo( d2.SortOrder ) );

			if (ActiveDevice == inputDevice)
			{
				ActiveDevice = InputDevice.Null;
			}

			if (OnDeviceDetached != null)
			{
				OnDeviceDetached( inputDevice );
			}
		}


		public static void HideDevicesWithProfile( Type type )
		{
			if (type.IsSubclassOf( typeof( UnityInputDeviceProfile ) ))
			{
				UnityInputDeviceProfile.Hide( type );
			}
		}


		static InputDevice DefaultActiveDevice
		{
			get { return (Devices.Count > 0) ? Devices[0] : InputDevice.Null; }
		}


		public static InputDevice ActiveDevice
		{
			get
			{
				return (activeDevice == null) ? InputDevice.Null : activeDevice;
			}

			private set
			{
				activeDevice = (value == null) ? InputDevice.Null : value;
			}
		}


		public static bool InvertYAxis
		{
			get { return invertYAxis; }
			set
			{
				if (isSetup)
				{
					throw new Exception( "InputManager.InvertYAxis must be set before calling InputManager.Setup()." );
				}
				invertYAxis = value;
			}
		}


		public static bool EnableXInput
		{
			get { return enableXInput; }
			set
			{
				if (isSetup)
				{
					throw new Exception( "InputManager.EnableXInput must be set before calling InputManager.Setup()." );
				}
				enableXInput = value;
			}
		}


		public static bool MenuWasPressed
		{
			get;
			private set;
		}
	}
}


