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


		public override float GetValue( InputDevice inputDevice )
		{
			var joystickId = (inputDevice as UnityInputDevice).JoystickId;
			var analogKey = GetAnalogKey( joystickId, analogId );
			return Input.GetAxisRaw( analogKey );
		}


		public override bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}


		static void SetupAnalogQueries()
		{
			if (analogQueries == null)
			{			
				analogQueries = new string[ 10, 10 ];
			
				for (int joystickId = 1; joystickId <= 10; joystickId++)
				{
					for (int analogId = 0; analogId < 10; analogId++)
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

