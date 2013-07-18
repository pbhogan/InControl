using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TinyJSON;
using UnityEngine;


namespace InControl
{
	public class InputManager
	{
		public delegate void DeviceEventHandler( InputDevice device );

		public static event DeviceEventHandler OnDeviceAttached;
		public static event DeviceEventHandler OnDeviceDetached;
		public static event DeviceEventHandler OnActiveDeviceChanged;

		public static InputDevice ActiveDevice { get; private set; }
		public static List<InputDevice> Devices = new List<InputDevice>();

		public static int NumInputControlTypes { get; private set; }
		public static string Platform { get; private set; }
		public static bool InvertYAxis { get; set; }

		static List<InputDeviceProfile> inputDeviceProfiles = new List<InputDeviceProfile>();
		static bool keyboardDevicesAttached = false;
		static string joystickHash = "";

		static bool isSetup = false;


		public static void Setup()
		{
			if (!isSetup)
			{
				Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();
				InvertYAxis = true; // default to y-axis points up.

				NumInputControlTypes = (int) InputControlType.Count + 1;

				LoadDeviceProfiles();
				RefreshDevices( false );

				isSetup = true;
			}
		}


		public static void Update()
		{
			Update( Time.time );
		}


		static void Update( float updateTime )
		{
			var lastActiveDevice = ActiveDevice;

			foreach (var inputDevice in Devices)
			{
				inputDevice.Update( updateTime );

				if (ActiveDevice == null || inputDevice.UpdateTime > ActiveDevice.UpdateTime)
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

			if (joystickHash != GetJoystickHash())
			{
				RefreshDevices();
			}
		}


		public static void RefreshDevices( bool notify = true )
		{
			AttachKeyboardDevices();

			DetectAttachedJoystickDevices( notify );
			DetectDetachedJoystickDevices( notify );

			if (ActiveDevice == null)
			{
				ActiveDevice = GetDefaultActiveDevice();
			}
		}


		static void AttachKeyboardDevices()
		{
			foreach (var config in inputDeviceProfiles)
			{
				if (!config.IsJoystick && config.IsSupportedOnThisPlatform)
				{
					AttachKeyboardDeviceWithConfig( config );
				}	
			}
		}


		static void AttachKeyboardDeviceWithConfig( InputDeviceProfile config )
		{		
			if (keyboardDevicesAttached)
			{
				return;
			}

			Devices.Add( new InputDevice( config, 0 ) );

			keyboardDevicesAttached = true;
		}


		static void DetectAttachedJoystickDevices( bool notify )
		{
			string[] joystickNames = Input.GetJoystickNames();
			for (int i = 0; i < joystickNames.Length; i++)
			{
				var joystickName = joystickNames[i];
				var matchedConfig = inputDeviceProfiles.Find( config => config.HasJoystickName( joystickName ) );

				if (matchedConfig == null)
				{
					matchedConfig = new InputDeviceProfile( joystickName );
					inputDeviceProfiles.Add( matchedConfig );
				}

				foreach (var device in Devices)
				{
					if (device.Profile == matchedConfig && device.UnityJoystickId - 1 == i)
					{
						// already configured
						return;
					}
				}

				var inputDevice = new InputDevice( matchedConfig, i + 1 );
				Devices.Add( inputDevice );

				if (notify && OnDeviceAttached != null)
				{
					OnDeviceAttached( inputDevice );
				}
			}

			joystickHash = GetJoystickHash();
		}


		static void DetectDetachedJoystickDevices( bool notify )
		{
			var joystickNames = Input.GetJoystickNames();

			for (int i = Devices.Count - 1; i >= 0; i--)
			{
				InputDevice inputDevice = Devices[i];

				if (!inputDevice.Profile.IsJoystick)
				{
					continue;
				}

				if (joystickNames.Length < inputDevice.UnityJoystickId || 
					!inputDevice.Profile.HasJoystickName( joystickNames[inputDevice.UnityJoystickId - 1] ))
				{
					if (ActiveDevice == inputDevice)
					{
						ActiveDevice = GetDefaultActiveDevice();
					}

					Devices.Remove( inputDevice );

					if (notify && OnDeviceDetached != null)
					{
						OnDeviceDetached( inputDevice );
					}
				}
			}
		}


		static void LoadDeviceProfiles()
		{
			foreach (TextAsset textAsset in Resources.LoadAll( "InputDeviceProfiles", typeof( TextAsset ) ))
			{
				var proxyObject = JSON.Load( textAsset.text ) as ProxyObject;

				if (proxyObject == null)
				{
					Debug.LogError( textAsset.name + " is not a valid JSON file." );
				}

				var deviceProfile = proxyObject.Make<InputDeviceProfile>();
						
				if (deviceProfile.IsSupportedOnThisPlatform)
				{
					inputDeviceProfiles.Add( deviceProfile );
				}
			}
		}


		static InputDevice GetDefaultActiveDevice()
		{
			return Devices.Count > 0 ? Devices[0] : null;
		}


		static string GetJoystickHash()
		{
			var joystickNames = Input.GetJoystickNames();
			return joystickNames.Length + ":" + String.Join( ",", joystickNames );
		}


		/*
		public static Vector2 GetMousePosition()
		{
			var camera = Futile.instance.camera;
			var screenPos = Input.mousePosition;
			screenPos.z = -camera.transform.position.z;
			var worldPos = (Vector2) camera.ScreenToWorldPoint( screenPos );

			return worldPos; // + offset;
		}
		*/
	}
}


