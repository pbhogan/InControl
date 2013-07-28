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

		static bool invertYAxis = false; // default to y-axis up.
		static bool isSetup = false;


		public static void Setup()
		{
			if (!isSetup)
			{
				Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

				inputDeviceManagers.Add( new UnityInputDeviceManager() );
				UpdateDeviceManagers( 0.0f );

				isSetup = true;
			}
		}


		public static void Update()
		{
			if (!isSetup)
			{
				Setup();
			}

			Update( Time.time );
		}


		static void Update( float updateTime )
		{
			UpdateDeviceManagers( updateTime );
			UpdateActiveDevice();
		}


		static void UpdateActiveDevice()
		{
			var lastActiveDevice = ActiveDevice;

			foreach (var inputDevice in Devices)
			{
				if (ActiveDevice == InputDevice.Null || inputDevice.UpdateTime > ActiveDevice.UpdateTime)
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


		static void UpdateDeviceManagers( float updateTime )
		{
			foreach (var inputDeviceManager in inputDeviceManagers)
			{
				inputDeviceManager.Update( updateTime );
			}
		}


		public static void AttachDevice( InputDevice inputDevice )
		{
			Devices.Add( inputDevice );

			if (isSetup)
			{
				if (OnDeviceAttached != null)
				{
					OnDeviceAttached( inputDevice );
				}
			}
		}


		public static void DetachDevice( InputDevice inputDevice )
		{
			Devices.Remove( inputDevice );

			if (ActiveDevice == inputDevice)
			{
				ActiveDevice = InputDevice.Null;
			}

			if (isSetup)
			{
				if (OnDeviceDetached != null)
				{
					OnDeviceDetached( inputDevice );
				}
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
			set { invertYAxis = value; }
		}
	}
}


