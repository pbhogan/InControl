using System;
using UnityEngine;


namespace InControl
{
	public class UnityMouseAxisSource : InputControlSource
	{
		string mouseAxisQuery;


		public UnityMouseAxisSource( string axis )
		{
			this.mouseAxisQuery = "mouse " + axis;
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return Input.GetAxisRaw( mouseAxisQuery );
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}
	}
}

