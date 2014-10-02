using System;
using UnityEngine;


namespace InControl
{
	public class UnityAnalogSource : InputControlSource
	{
		int analogId;
		static string[,] analogQueries;


		public UnityAnalogSource( int analogId )
		{
			this.analogId = analogId;
			SetupAnalogQueries();
		}


		public float GetValue( InputDevice inputDevice )
		{
			var joystickId = (inputDevice as UnityInputDevice).JoystickId;
			var analogKey = GetAnalogKey( joystickId, analogId );
			return Input.GetAxisRaw( analogKey );
		}


		public bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}


		static void SetupAnalogQueries()
		{
			if (analogQueries == null)
			{			
				analogQueries = new string[ UnityInputDevice.MaxDevices, UnityInputDevice.MaxAnalogs ];
			
				for (int joystickId = 1; joystickId <= UnityInputDevice.MaxDevices; joystickId++)
				{
					for (int analogId = 0; analogId < UnityInputDevice.MaxAnalogs; analogId++)
					{
						analogQueries[ joystickId - 1, analogId ] = "joystick " + joystickId + " analog " + analogId;
					}
				}
			}
		}


		static string GetAnalogKey( int joystickId, int analogId )
		{
			return analogQueries[ joystickId - 1, analogId ];
		}
	}
}

