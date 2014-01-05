using System;
using UnityEngine;


namespace InControl
{
	public class UnityButtonSource : InputControlSource
	{
		int buttonId;
		static string[,] buttonQueries;


		public UnityButtonSource( int buttonId )
		{
			this.buttonId = buttonId;
			SetupButtonQueries();
		}


		public override float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public override bool GetState( InputDevice inputDevice )
		{
			var joystickId = (inputDevice as UnityInputDevice).JoystickId;
			var buttonKey = GetButtonKey( joystickId, buttonId );
			return Input.GetKey( buttonKey );
		}


		static void SetupButtonQueries()
		{
			if (buttonQueries == null)
			{			
				buttonQueries = new string[ 10, 20 ];
				
				for (int joystickId = 1; joystickId <= 10; joystickId++)
				{
					for (int buttonId = 0; buttonId < 20; buttonId++)
					{
						buttonQueries[ joystickId - 1, buttonId ] = "joystick " + joystickId + " button " + buttonId;
					}
				}
			}
		}
		
		
		static string GetButtonKey( int joystickId, int buttonId )
		{
			return buttonQueries[ joystickId - 1, buttonId ];
		}
	}
}

