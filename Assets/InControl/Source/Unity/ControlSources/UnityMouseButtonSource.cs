using System;
using UnityEngine;


namespace InControl
{
	public class UnityMouseButtonSource : InputControlSource
	{
		int buttonId;


		public UnityMouseButtonSource( int buttonId )
		{
			this.buttonId = buttonId;
		}


		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public bool GetState( InputDevice inputDevice )
		{
			return Input.GetMouseButton( buttonId );
		}
	}
}

