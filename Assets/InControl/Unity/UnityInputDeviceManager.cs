using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace InControl
{
	public class UnityInputDeviceManager : InputDeviceManager
	{
		List<UnityInputDeviceProfile> deviceProfiles = new List<UnityInputDeviceProfile>();
		bool keyboardDevicesAttached = false;
		string joystickHash = "";


		public UnityInputDeviceManager()
		{
			AutoDiscoverDeviceProfiles();
			RefreshDevices();
		}


		public override void Update( float updateTime, float deltaTime )
		{
			foreach (var device in devices)
			{
				device.Update( updateTime, deltaTime );
			}

			if (joystickHash != JoystickHash)
			{
				Logger.LogInfo( "Change in Unity attached joysticks detected; refreshing device list." );
				RefreshDevices();
			}
		}


		void RefreshDevices()
		{
			AttachKeyboardDevices();
			DetectAttachedJoystickDevices();
			DetectDetachedJoystickDevices();
			joystickHash = JoystickHash;
		}


		void AttachKeyboardDevices()
		{
			foreach (var deviceProfile in deviceProfiles)
			{
				if (deviceProfile.IsNotJoystick && deviceProfile.IsSupportedOnThisPlatform)
				{
					AttachKeyboardDeviceWithConfig( deviceProfile );
				}	
			}
		}


		void AttachKeyboardDeviceWithConfig( UnityInputDeviceProfile config )
		{		
			if (keyboardDevicesAttached)
			{
				return;
			}

			var keyboardDevice = new UnityInputDevice( config );
			devices.Add( keyboardDevice );

			InputManager.AttachDevice( keyboardDevice );

			keyboardDevicesAttached = true;
		}


		void DetectAttachedJoystickDevices()
		{
			try
			{
				var joystickNames = Input.GetJoystickNames();
				for (int i = 0; i < joystickNames.Length; i++)
				{
					DetectAttachedJoystickDevice( i + 1, joystickNames[i] );
				}
			}
			catch (Exception e)
			{
				Logger.LogError( e.Message );
				Logger.LogError( e.StackTrace );
			}
		}


		void DetectAttachedJoystickDevice( int unityJoystickId, string unityJoystickName )
		{
			var matchedDeviceProfile = deviceProfiles.Find( config => config.HasJoystickName( unityJoystickName ) );
			UnityInputDeviceProfile deviceProfile = null;

			if (matchedDeviceProfile == null)
			{
				deviceProfile = new UnknownDeviceProfile( unityJoystickName );
				deviceProfiles.Add( deviceProfile );
			}
			else
			{
				deviceProfile = matchedDeviceProfile;
			}


			foreach (var device in devices)
			{
				var unityDevice = device as UnityInputDevice;
				if (unityDevice != null && unityDevice.IsConfiguredWith( deviceProfile, unityJoystickId ))
				{
					Logger.LogInfo( "Device \"" + unityJoystickName + "\" is already configured with " + deviceProfile.Name );
					return;
				}
			}

			var inputDevice = new UnityInputDevice( deviceProfile, unityJoystickId );
			devices.Add( inputDevice );

			InputManager.AttachDevice( inputDevice );

			if (matchedDeviceProfile == null)
			{
				Logger.LogWarning( "Attached device has no matching profile: \"" + unityJoystickName + "\"" );
			}
			else
			{
				Logger.LogInfo( "Attached device \"" + unityJoystickName + "\" matched profile: " + deviceProfile.Name );
			}
		}


		void DetectDetachedJoystickDevices()
		{
			var joystickNames = Input.GetJoystickNames();

			for (int i = devices.Count - 1; i >= 0; i--)
			{
				var inputDevice = devices[i] as UnityInputDevice;

				if (inputDevice.Profile.IsNotJoystick)
				{
					continue;
				}

				if (joystickNames.Length < inputDevice.JoystickId || 
				    !inputDevice.Profile.HasJoystickName( joystickNames[inputDevice.JoystickId - 1] ))
				{
					devices.Remove( inputDevice );

					InputManager.DetachDevice( inputDevice );

					Logger.LogInfo( "Detached device: " + inputDevice.Profile.Name );
				}
			}
		}


		void AutoDiscoverDeviceProfiles()
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) 
			{
				if (type.GetCustomAttributes( typeof(AutoDiscover), true ).Length > 0) 
				{
					var deviceProfile = (UnityInputDeviceProfile) Activator.CreateInstance( type );

					if (deviceProfile.IsSupportedOnThisPlatform)
					{
						Logger.LogInfo( "Adding profile: " + type.Name + " (" + deviceProfile.Name + ")" );
						deviceProfiles.Add( deviceProfile );
					}
					else
					{
						Logger.LogInfo( "Ignored profile: " + type.Name + " (" + deviceProfile.Name + ")" );
					}
				}
			}
		}


		static string JoystickHash
		{
			get
			{
				var joystickNames = Input.GetJoystickNames();
				return joystickNames.Length + ": " + String.Join( ", ", joystickNames );
			}
		}
	}
}

