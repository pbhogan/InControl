using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace InControl
{
	public class UnityInputDeviceManager : InputDeviceManager
	{
		float deviceRefreshTimer = 0.0f;
		const float deviceRefreshInterval = 1.0f;

		List<UnityInputDeviceProfile> deviceProfiles = new List<UnityInputDeviceProfile>();
		bool keyboardDevicesAttached = false;
		string joystickHash = "";


		public UnityInputDeviceManager()
		{
			AutoDiscoverDeviceProfiles();
			RefreshDevices();
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			deviceRefreshTimer += deltaTime;
			if (string.IsNullOrEmpty( joystickHash ) || deviceRefreshTimer >= deviceRefreshInterval)
			{
				deviceRefreshTimer = 0.0f;

				if (joystickHash != JoystickHash)
				{
					Logger.LogInfo( "Change in Unity attached joysticks detected; refreshing device list." );
					RefreshDevices();
				}
			}
		}


		void RefreshDevices()
		{
			AttachKeyboardDevices();
			DetectAttachedJoystickDevices();
			DetectDetachedJoystickDevices();
			joystickHash = JoystickHash;
		}


		void AttachDevice( UnityInputDevice device )
		{
			devices.Add( device );
			InputManager.AttachDevice( device );
		}


		void AttachKeyboardDevices()
		{
			int deviceProfileCount = deviceProfiles.Count;
			for (int i = 0; i < deviceProfileCount; i++)
			{
				var deviceProfile = deviceProfiles[i];
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
			AttachDevice( keyboardDevice );

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
			if (unityJoystickName == "WIRED CONTROLLER" ||
			    unityJoystickName == " WIRED CONTROLLER")
			{
				// Ignore Steam controller for now.
				return;
			}

			if (unityJoystickName.IndexOf( "webcam", StringComparison.OrdinalIgnoreCase ) != -1)
			{
				// Unity thinks some webcams are joysticks. >_<
				return;
			}

			// PS4 controller works properly as of Unity 4.5
			if (InputManager.UnityVersion <= new VersionInfo( 4, 5 ))
			{
				if (Application.platform == RuntimePlatform.OSXEditor ||
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer)
				{
					if (unityJoystickName == "Unknown Wireless Controller")
					{
						// Ignore PS4 controller in Bluetooth mode on Mac since it connects but does nothing.
						return;
					}
				}
			}

			var matchedDeviceProfile = deviceProfiles.Find( config => config.HasJoystickName( unityJoystickName ) );

			if (matchedDeviceProfile == null)
			{
				matchedDeviceProfile = deviceProfiles.Find( config => config.HasLastResortRegex( unityJoystickName ) );
			}

			UnityInputDeviceProfile deviceProfile = null;

			if (matchedDeviceProfile == null)
			{
				deviceProfile = new UnityUnknownDeviceProfile( unityJoystickName );
				deviceProfiles.Add( deviceProfile );
			}
			else
			{
				deviceProfile = matchedDeviceProfile;
			}

			int deviceCount = devices.Count;
			for (int i = 0; i < deviceCount; i++)
			{
				var device = devices[i];
				var unityDevice = device as UnityInputDevice;
				if (unityDevice != null && unityDevice.IsConfiguredWith( deviceProfile, unityJoystickId ))
				{
					Logger.LogInfo( "Device \"" + unityJoystickName + "\" is already configured with " + deviceProfile.Name );
					return;
				}
			}

			if (!deviceProfile.IsHidden)
			{
				var joystickDevice = new UnityInputDevice( deviceProfile, unityJoystickId );
				AttachDevice( joystickDevice );

				if (matchedDeviceProfile == null)
				{
					Logger.LogWarning( "Device " + unityJoystickId + " with name \"" + unityJoystickName + "\" does not match any known profiles." );
				}
				else
				{
					Logger.LogInfo( "Device " + unityJoystickId + " matched profile " + deviceProfile.GetType().Name + " (" + deviceProfile.Name + ")" );
				}
			}
			else
			{
				Logger.LogInfo( "Device " + unityJoystickId + " matching profile " + deviceProfile.GetType().Name + " (" + deviceProfile.Name + ")" + " is hidden and will not be attached." );
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
				    !inputDevice.Profile.HasJoystickOrRegexName( joystickNames[inputDevice.JoystickId - 1] ))
				{
					devices.Remove( inputDevice );
					InputManager.DetachDevice( inputDevice );

					Logger.LogInfo( "Detached device: " + inputDevice.Profile.Name );
				}
			}
		}


		void AutoDiscoverDeviceProfiles()
		{
			foreach (var typeName in UnityInputDeviceProfileList.Profiles)
			{				
				var deviceProfile = (UnityInputDeviceProfile) Activator.CreateInstance( Type.GetType( typeName ) );
				if (deviceProfile.IsSupportedOnThisPlatform)
				{
					// Logger.LogInfo( "Found profile: " + deviceProfile.GetType().Name + " (" + deviceProfile.Name + ")" );
					deviceProfiles.Add( deviceProfile );
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

