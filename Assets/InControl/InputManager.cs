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
		public delegate void DeviceEventHandler( InputDevice device );
		public static event DeviceEventHandler OnDeviceAttached;
		public static event DeviceEventHandler OnDeviceDetached;
		public static event DeviceEventHandler OnActiveDeviceChanged;

		static List<InputDeviceManager> inputDeviceManagers = new List<InputDeviceManager>();

		static InputDevice activeDevice = InputDevice.Null;
		public static List<InputDevice> Devices = new List<InputDevice>();

		public static string Platform { get; private set; }

		static bool invertYAxis; // default to y-axis up.
		static bool isSetup;

		static float initialTime;
		static float currentTime;


		public static void Setup()
		{
			isSetup = false;

			Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

			invertYAxis = false; // default to y-axis up.

			initialTime = 0.0f;
			currentTime = 0.0f;

			inputDeviceManagers.Clear();
			Devices.Clear();
			activeDevice = InputDevice.Null;

			OnDeviceAttached = null;
			OnDeviceDetached = null;
			OnActiveDeviceChanged = null;

			isSetup = true;

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

			UpdateCurrentTime();
			UpdateDeviceManagers();
			UpdateActiveDevice();
		}


		static void UpdateActiveDevice()
		{
			var lastActiveDevice = ActiveDevice;

			foreach (var inputDevice in Devices)
			{
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
			inputDeviceManager.Update( currentTime );
		}


		static void UpdateCurrentTime()
		{
			// Have to do this hack since Time.realtimeSinceStartup 
			// is not updated until AFTER Awake().
			if (initialTime == 0.0f)
			{
				initialTime = Time.realtimeSinceStartup;
			}

			currentTime = Math.Max( 0.0f, Time.realtimeSinceStartup - initialTime );
		}


		static void UpdateDeviceManagers()
		{
			foreach (var inputDeviceManager in inputDeviceManagers)
			{
				inputDeviceManager.Update( currentTime );
			}
		}


		public static void AttachDevice( InputDevice inputDevice )
		{
			AssertIsSetup();

			Devices.Add( inputDevice );

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

			if (ActiveDevice == inputDevice)
			{
				ActiveDevice = InputDevice.Null;
			}

			if (OnDeviceDetached != null)
			{
				OnDeviceDetached( inputDevice );
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
				AssertIsSetup();
				invertYAxis = value; 
			}
		}
	}
}


