using System;
using UnityEngine;


namespace InControl
{
	public class UnityMouseAxisSource : InputControlSource
	{
		string axis; 


		public UnityMouseAxisSource( string axis )
		{
			this.axis = axis;
		}
		
		
		public override float GetValue( InputDevice inputDevice )
		{
			return Input.GetAxisRaw( "mouse " + axis );
		}
		
		
		public override bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}
	}
}

