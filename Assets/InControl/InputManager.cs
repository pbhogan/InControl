using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace InControl
{
	public enum LogMessageType
	{
		Info,
		Warning,
		Error
	}


	public struct LogMessage
	{
		public string text;
		public LogMessageType type;
	}


	public class InputManager
	{
		public delegate void DeviceEventHandler( InputDevice device );
		public delegate void LogMessageHandler( LogMessage message );

		public static event DeviceEventHandler OnDeviceAttached;
		public static event DeviceEventHandler OnDeviceDetached;
		public static event DeviceEventHandler OnActiveDeviceChanged;

		public static event LogMessageHandler OnLogMessage;

		public static InputDevice ActiveDevice { get; private set; }
		public static List<InputDevice> Devices = new List<InputDevice>();

		public static int NumInputControlTypes { get; private set; }
		public static string Platform { get; private set; }

		static List<InputDeviceProfile> deviceProfiles = new List<InputDeviceProfile>();
		static bool keyboardDevicesAttached = false;
		static string joystickHash = "";
		static bool invertYAxis = false; // default to y-axis up.
		static bool isSetup = false;


		public static void Setup()
		{
			if (!isSetup)
			{
				Platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

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

			if (joystickHash != JoystickHash)
			{
				LogInfo( "Change in Unity attached joysticks detected; refreshing device list." );
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
			foreach (var config in deviceProfiles)
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
			try
			{
				var joystickNames = Input.GetJoystickNames();
				for (int i = 0; i < joystickNames.Length; i++)
				{
					DetectAttachedJoystickDevice( i + 1, joystickNames[i], notify );
				}
			}
			catch (Exception e)
			{
				LogError( e.Message );
				LogError( e.StackTrace );
			}

			joystickHash = JoystickHash;
		}


		static void DetectAttachedJoystickDevice( int unityJoystickId, string unityJoystickName, bool notify )
		{
			var matchedDeviceProfile = deviceProfiles.Find( config => config.HasJoystickName( unityJoystickName ) );
			InputDeviceProfile deviceProfile = null;

			if (matchedDeviceProfile == null)
			{
				deviceProfile = new InputDeviceProfile( unityJoystickName );
				deviceProfiles.Add( deviceProfile );
			}
			else
			{
				deviceProfile = matchedDeviceProfile;
			}

			if (Devices.Any( device => device.IsConfiguredWith( deviceProfile, unityJoystickId ) ))
		    {
				LogInfo( "Device \"" + unityJoystickName + "\" is already configured with " + deviceProfile.Name );
				return;
			}

			var inputDevice = new InputDevice( deviceProfile, unityJoystickId );
			Devices.Add( inputDevice );

			if (notify && OnDeviceAttached != null)
			{
				OnDeviceAttached( inputDevice );
			}

			if (matchedDeviceProfile == null)
			{
				LogWarning( "Attached device has no matching profile: \"" + unityJoystickName + "\"" );
			}
			else
			{
				LogInfo( "Attached device \"" + unityJoystickName + "\" matched profile: " + deviceProfile.Name );
			}
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

					LogInfo( "Detached device: " + inputDevice.Profile.Name );
				}
			}
		}


		static void LoadDeviceProfiles()
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) 
			{
				if (type.GetCustomAttributes( typeof(DeviceProfile), true ).Length > 0) 
				{
					var deviceProfile = (InputDeviceProfile) Activator.CreateInstance( type );

					if (deviceProfile.IsSupportedOnThisPlatform)
					{
						LogInfo( "Adding profile: " + type.Name + " (" + deviceProfile.Name + ")" );
						deviceProfiles.Add( deviceProfile );
					}
					else
					{
						LogInfo( "Ignored profile: " + type.Name + " (" + deviceProfile.Name + ")" );
					}
				}
			}
		}


		static InputDevice GetDefaultActiveDevice()
		{
			return Devices.Count > 0 ? Devices[0] : null;
		}


		public static string JoystickHash
		{
			get
			{
				var joystickNames = Input.GetJoystickNames();
				return joystickNames.Length + ": " + String.Join( ", ", joystickNames );
			}
		}


		static void LogInfo( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Info };
				OnLogMessage( logMessage );
			}
		}


		static void LogWarning( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Warning };
				OnLogMessage( logMessage );
			}
		}


		static void LogError( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Error };
				OnLogMessage( logMessage );
			}
		}


		public static bool InvertYAxis 
		{ 
			get { return invertYAxis; } 
			set { invertYAxis = value; }
		}
	}
}


