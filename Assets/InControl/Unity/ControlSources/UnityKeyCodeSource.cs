using System;
using UnityEngine;


namespace InControl
{
	public class UnityKeyCodeSource : InputControlSource
	{
		KeyCode keyCode;


		public UnityKeyCodeSource( KeyCode keyCode )
		{
			this.keyCode = keyCode;
		}


		public override float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public override bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode );
		}
	}
}

