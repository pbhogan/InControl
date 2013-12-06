using System;
using UnityEngine;


namespace InControl
{
	public abstract class InputControlSource
	{
		public InputControlSource()
		{
		}

		public abstract float GetValue( int deviceId = 0 );
		public abstract bool GetState( int deviceId = 0 );
	}


	public class UnityAnalogInputControlSource : InputControlSource
	{
		int analogId;
		
		
		public UnityAnalogInputControlSource( int analogId )
		{
			this.analogId = analogId;
		}
		
		
		public override float GetValue( int deviceId )
		{
			return Input.GetAxisRaw( "joystick " + deviceId + " analog " + analogId );
		}


		public override bool GetState( int deviceId = 0 )
		{
			return !Mathf.Approximately( GetValue( deviceId ), 0.0f );
		}
	}


	public class UnityButtonInputControlSource : InputControlSource
	{
		int buttonId;
		
		
		public UnityButtonInputControlSource( int buttonId )
		{
			this.buttonId = buttonId;
		}
		
		
		public override float GetValue( int deviceId )
		{
			return GetState( deviceId ) ? 1.0f : 0.0f;
		}


		public override bool GetState( int deviceId )
		{
			return Input.GetKey( "joystick " + deviceId + " button " + buttonId );
		}
	}


	public class UnityKeyCodeButtonInputControlSource : InputControlSource
	{
		KeyCode keyCode;
		
		
		public UnityKeyCodeButtonInputControlSource( KeyCode keyCode )
		{
			this.keyCode = keyCode;
		}
		
		
		public override float GetValue( int deviceId )
		{
			return GetState( deviceId ) ? 1.0f : 0.0f;
		}
		
		
		public override bool GetState( int deviceId )
		{
			return Input.GetKey( keyCode );
		}
	}


	public class UnityKeyCodeAxisInputControlSource : InputControlSource
	{
		KeyCode negativeKeyCode;
		KeyCode positiveKeyCode;
		
		
		public UnityKeyCodeAxisInputControlSource( KeyCode negativeKeyCode, KeyCode positiveKeyCode )
		{
			this.negativeKeyCode = negativeKeyCode;
			this.positiveKeyCode = positiveKeyCode;
		}
		
		
		public override float GetValue( int deviceId )
		{
			int axisValue = 0;
			
			if (Input.GetKey( negativeKeyCode ))
			{
				axisValue--;
			}
			
			if (Input.GetKey( positiveKeyCode ))
			{
				axisValue++;
			}
			
			return axisValue;
		}
		
		
		public override bool GetState( int deviceId )
		{
			return !Mathf.Approximately( GetValue( deviceId ), 0.0f );
		}
	}
}


