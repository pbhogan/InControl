using System;
using UnityEngine;


namespace InControl
{
	public class UnityButtonSource : InputControlSource
	{
		int buttonId;


		public UnityButtonSource( int buttonId )
		{
			this.buttonId = buttonId;
		}


		public override float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public override bool GetState( InputDevice inputDevice )
		{
			var joystickId = (inputDevice as UnityInputDevice).JoystickId;
			return Input.GetKey( "joystick " + joystickId + " button " + buttonId );
		}
	}
}

