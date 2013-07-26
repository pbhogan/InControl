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

		private static InputDevice activeDevice = InputDevice.Null;
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

				AutoDiscoverDeviceProfiles();
				RefreshDevices( false );

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
			var lastActiveDevice = ActiveDevice;

			foreach (var inputDevice in Devices)
			{
				inputDevice.Update( updateTime );

				if (ActiveDevice == InputDevice.Null || 
				    inputDevice.UpdateTime > ActiveDevice.UpdateTime)
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

			if (ActiveDevice == InputDevice.Null)
			{
				ActiveDevice = DefaultActiveDevice;
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

			var keyboardDevice = new UnityInputDevice( config );
			Devices.Add( keyboardDevice );

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
				deviceProfile = new UnityUnknownDeviceProfile( unityJoystickName );
				deviceProfiles.Add( deviceProfile );
			}
			else
			{
				deviceProfile = matchedDeviceProfile;
			}


			foreach (var device in Devices)
		    {
				var unityDevice = device as UnityInputDevice;
				if (unityDevice != null && unityDevice.IsConfiguredWith( deviceProfile, unityJoystickId ))
				{
					LogInfo( "Device \"" + unityJoystickName + "\" is already configured with " + deviceProfile.Name );
					return;
				}
			}

			var inputDevice = new UnityInputDevice( deviceProfile, unityJoystickId );
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
				var unityDevice = Devices[i] as UnityInputDevice;

				if (unityDevice == null || unityDevice.Profile.IsNotJoystick)
				{
					continue;
				}

				// TODO: This should all obviously be inside some sort of UnityInputDeviceManager.
				if (joystickNames.Length < unityDevice.UnityJoystickId || 
					!unityDevice.Profile.HasJoystickName( joystickNames[unityDevice.UnityJoystickId - 1] ))
				{
					if (ActiveDevice == unityDevice)
					{
						ActiveDevice = DefaultActiveDevice;
					}

					Devices.Remove( unityDevice );

					if (notify && OnDeviceDetached != null)
					{
						OnDeviceDetached( unityDevice );
					}

					LogInfo( "Detached device: " + unityDevice.Profile.Name );
				}
			}
		}


		static void AutoDiscoverDeviceProfiles()
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) 
			{
				if (type.GetCustomAttributes( typeof(AutoDiscover), true ).Length > 0) 
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


		static InputDevice DefaultActiveDevice
		{
			get { return (Devices.Count > 0) ? Devices[0] : InputDevice.Null; }
		}


		public static string JoystickHash
		{
			get
			{
				var joystickNames = Input.GetJoystickNames();
				return joystickNames.Length + ": " + String.Join( ", ", joystickNames );
			}
		}


		public static InputDevice ActiveDevice 
		{ 
			get { return activeDevice; }
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


