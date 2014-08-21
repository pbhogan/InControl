using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
		static List<InputDevice> devices = new List<InputDevice>();
		public static ReadOnlyCollection<InputDevice> Devices;

		public static string Platform { get; private set; }
		public static bool MenuWasPressed { get; private set; }
		public static bool InvertYAxis;

		static bool enableXInput;
		static bool enableOuyaEverywhereInput;
		static bool isSetup;

		static float initialTime;
		static float currentTime;
		static float lastUpdateTime;

		static ulong currentTick;


		public static void Setup()
		{
			if (isSetup)
			{
				return;
			}

			Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

			initialTime = 0.0f;
			currentTime = 0.0f;
			lastUpdateTime = 0.0f;
			currentTick = 0;

			inputDeviceManagers.Clear();
			devices.Clear();
			Devices = new ReadOnlyCollection<InputDevice>( devices );
			activeDevice = InputDevice.Null;

			isSetup = true;

			#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			if (enableXInput)
			{
				XInputDeviceManager.Enable();
			}
			#endif

			#if UNITY_ANDROID && !UNITY_EDITOR
			if (enableOuyaEverywhereInput) {
				OuyaEverywhereDeviceManager.Enable();
			}
			#endif

			AddDeviceManager( new UnityInputDeviceManager() );

			if (OnSetup != null)
			{
				OnSetup.Invoke();
				OnSetup = null;
			}
		}


		public static void Reset()
		{
			OnSetup = null;
			OnUpdate = null;
			OnActiveDeviceChanged = null;
			OnDeviceAttached = null;
			OnDeviceDetached = null;

			inputDeviceManagers.Clear();
			devices.Clear();
			activeDevice = InputDevice.Null;
			
			isSetup = false;
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


		public static void OnApplicationFocus( bool focusState )
		{
			if (!focusState)
			{
				int deviceCount = devices.Count;
				for (int i = 0; i < deviceCount; i++)
				{
					var inputControls = devices[i].Controls;
					var inputControlCount = inputControls.Length;
					for (int j = 0; j < inputControlCount; j++)
					{
						var inputControl = inputControls[j];
						if (inputControl != null)
						{
							inputControl.SetZeroTick();
						}
					}
				}
			}
		}


		public static void OnApplicationPause( bool pauseState ) 
		{
		}


		public static void OnApplicationQuit()
		{
		}


		static void UpdateActiveDevice()
		{
			var lastActiveDevice = ActiveDevice;

			int deviceCount = devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var inputDevice = devices[i];
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
			
			int deviceCount = devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = devices[i];
				device.PreUpdate( currentTick, deltaTime );
			}
		}


		static void UpdateDevices( float deltaTime )
		{
			int deviceCount = devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = devices[i];
				device.Update( currentTick, deltaTime );
			}

			if (OnUpdate != null)
			{
				OnUpdate.Invoke( currentTick, deltaTime );
			}
		}


		static void PostUpdateDevices( float deltaTime )
		{			
			int deviceCount = devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = devices[i];
				
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

			devices.Add( inputDevice );
			devices.Sort( ( d1, d2 ) => d1.SortOrder.CompareTo( d2.SortOrder ) );

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

			devices.Remove( inputDevice );
			devices.Sort( ( d1, d2 ) => d1.SortOrder.CompareTo( d2.SortOrder ) );

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
			#if !UNITY_EDITOR && UNITY_WINRT
			if (type.GetTypeInfo().IsAssignableFrom( typeof( UnityInputDeviceProfile ).GetTypeInfo() ))
			#else
			if (type.IsSubclassOf( typeof( UnityInputDeviceProfile ) ))
			#endif
			{
				UnityInputDeviceProfile.Hide( type );
			}
		}


		static InputDevice DefaultActiveDevice
		{
			get 
			{ 
				return (devices.Count > 0) ? devices[0] : InputDevice.Null; 
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
				enableXInput = value;
			}
		}

		public static bool EnableOuyaEverywhereInput
		{
			get
			{
				return enableOuyaEverywhereInput;
			}
			set
			{
				enableOuyaEverywhereInput = value;
			}
		}
	}
}


