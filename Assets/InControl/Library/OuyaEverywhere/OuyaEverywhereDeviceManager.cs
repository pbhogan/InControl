#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuyaEverywhereHelpers;


namespace InControl
{
	public class OuyaEverywhereDeviceManager : InputDeviceManager
	{
		bool[] deviceConnected = new bool[] { false, false, false, false };


		public OuyaEverywhereDeviceManager()
		{
			for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
			{
				devices.Add( new OuyaEverywhereDevice( deviceIndex ) );
			}

			Update( 0, 0.0f );
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
			{
				var device = devices[deviceIndex] as OuyaEverywhereDevice;

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
			// TODO: evaluate the need for this function
			return true;
		}


		public static void Enable()
		{
			var errors = new List<string>();
			if (OuyaEverywhereDeviceManager.CheckPlatformSupport( errors ))
			{
				InputManager.HideDevicesWithProfile( typeof( OuyaProfile ) );
				InputManager.AddDeviceManager( new OuyaEverywhereDeviceManager() );
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

