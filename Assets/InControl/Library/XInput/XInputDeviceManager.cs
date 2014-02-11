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

			RefreshDevices();
		}


		public override void Update( float updateTime, float deltaTime )
        {
            UpdateUnconnectedDevices(updateTime, deltaTime);
			RefreshDevices();
		}


		void RefreshDevices()
		{
			for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
			{
				var device = devices[deviceIndex] as XInputDevice;

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

        void UpdateUnconnectedDevices(float updateTime, float deltaTime)
        {
            for (int deviceIndex = 0; deviceIndex < 4; deviceIndex++)
            {
                var device = devices[deviceIndex] as XInputDevice;

                if (!device.IsConnected)
                    device.Update(updateTime, deltaTime);
            }
        }
	}
}

