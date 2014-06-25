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
		public static readonly VersionInfo Version = VersionInfo.InControlVersion();
		public static readonly VersionInfo UnityVersion = VersionInfo.UnityVersion();

		public static event Action OnSetup;
		public static event Action<ulong,float> OnUpdate;
		public static event Action<InputDevice> OnDeviceAttached;
		public static event Action<InputDevice> OnDeviceDetached;
		public static event Action<InputDevice> OnActiveDeviceChanged;

		static List<InputDeviceManager> inputDeviceManagers = new List<InputDeviceManager>();

		static InputDevice activeDevice = InputDevice.Null;
		public static List<InputDevice> Devices = new List<InputDevice>();

		public static string Platform { get; private set; }
		public static bool MenuWasPressed { get; private set; }
		public static bool InvertYAxis;

		static bool enableXInput;
		static bool isSetup;

		static float initialTime;
		static float currentTime;
		static float lastUpdateTime;

		static ulong currentTick;


		public static void Setup()
		{
			Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

			initialTime = 0.0f;
			currentTime = 0.0f;
			lastUpdateTime = 0.0f;

			currentTick = 0;

			inputDeviceManagers.Clear();
			Devices.Clear();
			activeDevice = InputDevice.Null;

			isSetup = true;

			#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			if (enableXInput)
			{
				XInputDeviceManager.Enable();
			}
			#endif

			AddDeviceManager( new UnityInputDeviceManager() );

			if (OnSetup != null)
			{
				OnSetup.Invoke();
				OnSetup = null;
			}
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
			if (OnSetup != null)
			{
				OnSetup.Invoke();
				OnSetup = null;
			}

			currentTick++;
			UpdateCurrentTime();
			var deltaTime = currentTime - lastUpdateTime;

			UpdateDeviceManagers( deltaTime);

			PreUpdateDevices( deltaTime );
			UpdateDevices( deltaTime );
			PostUpdateDevices( deltaTime );

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


		static void UpdateDeviceManagers( float deltaTime )
		{
			int inputDeviceManagerCount = inputDeviceManagers.Count;
			for (int i = 0; i < inputDeviceManagerCount; i++)
			{
				var inputDeviceManager = inputDeviceManagers[i];
				inputDeviceManager.Update( currentTick, deltaTime );
			}
		}


		static void PreUpdateDevices( float deltaTime )
		{
			MenuWasPressed = false;
			
			int deviceCount = Devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = Devices[i];
				device.PreUpdate( currentTick, deltaTime );
			}
		}


		static void UpdateDevices( float deltaTime )
		{
			int deviceCount = Devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = Devices[i];
				device.Update( currentTick, deltaTime );
			}

			if (OnUpdate != null)
			{
				OnUpdate.Invoke( currentTick, deltaTime );
			}
		}


		static void PostUpdateDevices( float deltaTime )
		{			
			int deviceCount = Devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = Devices[i];
				
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
			get 
			{ 
				return (Devices.Count > 0) ? Devices[0] : InputDevice.Null; 
			}
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


		public static bool EnableXInput
		{
			get 
			{ 
				return enableXInput; 
			}

			set
			{
				if (isSetup)
				{
					throw new Exception( "InputManager.EnableXInput must be set before calling InputManager.Setup()." );
				}
				enableXInput = value;
			}
		}
	}
}


