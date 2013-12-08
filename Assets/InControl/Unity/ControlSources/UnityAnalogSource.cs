using System;
using UnityEngine;


namespace InControl
{
	public class UnityAnalogSource : InputControlSource
	{
		int analogId;


		public UnityAnalogSource( int analogId )
		{
			this.analogId = analogId;
		}


		public override float GetValue( InputDevice inputDevice )
		{
			var joystickId = (inputDevice as UnityInputDevice).JoystickId;
			return Input.GetAxisRaw( "joystick " + joystickId + " analog " + analogId );
		}


		public override bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}
	}
}

