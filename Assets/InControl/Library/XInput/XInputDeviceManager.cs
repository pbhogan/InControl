#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


namespace InControl
{
	public class XInputDeviceManager : InputDeviceManager
	{
		bool[] deviceConnected = new bool[] { false, false, false, false };


		public XInputDeviceManager()
		{
			for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
			{
				devices.Add( new XInputDevice( deviceIndex ) );
			}

			Update( 0, 0.0f );
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
			{
				var device = devices[deviceIndex] as XInputDevice;

				// Unconnected devices won't be updated otherwise, so poll here.
				if (!device.IsConnected)
				{
					device.Update( updateTick, deltaTime );
				}

				if (device.IsConnected != deviceConnected[deviceIndex])
				{
					if (device.IsConnected)
					{
						InputManager.AttachDevice( device );
					}
					else
					{
						InputManager.DetachDevice( device );
					}

					deviceConnected[deviceIndex] = device.IsConnected;
				}
			}
		}


		public static bool CheckPlatformSupport( ICollection<string> errors )
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer &&
			    Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}

			try
			{
				GamePad.GetState( PlayerIndex.One );
			}
			catch (DllNotFoundException e)
			{
				if (errors != null)
				{
					errors.Add( e.Message + ".dll could not be found or is missing a dependency." );
				}
				return false;
			}
			
			return true;
		}


		public static void Enable()
		{
			var errors = new List<string>();
			if (XInputDeviceManager.CheckPlatformSupport( errors ))
			{
				InputManager.HideDevicesWithProfile( typeof(Xbox360WinProfile) );
				InputManager.HideDevicesWithProfile( typeof(XboxOneWinProfile) );
				InputManager.HideDevicesWithProfile( typeof(LogitechF710ModeXWinProfile) );
				InputManager.HideDevicesWithProfile( typeof(LogitechF310ModeXWinProfile) );
				InputManager.AddDeviceManager<XInputDeviceManager>();
			}
			else
			{
				foreach (var error in errors)
				{
					Logger.LogError( error );
				}
			}
		}
	}
}
#endif

