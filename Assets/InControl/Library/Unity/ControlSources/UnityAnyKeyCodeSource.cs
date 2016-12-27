using System;
using UnityEngine;


namespace InControl
{
	public class UnityAnyTwoKeyCodeSource : InputControlSource
	{
		protected KeyCode keyCode1;
		protected KeyCode keyCode2;
		
		
		public UnityAnyTwoKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2 )
		{
			this.keyCode1 = keyCode1;
			this.keyCode2 = keyCode2;
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) || Input.GetKey( keyCode2 );
		}
	}


	public class UnityAnyThreeKeyCodeSource : UnityAnyTwoKeyCodeSource
	{
		protected KeyCode keyCode3;
		
		
		public UnityAnyThreeKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3 ) : base ( keyCode1, keyCode2 )
		{
			this.keyCode3 = keyCode3;
		}

		
		public new bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) || Input.GetKey( keyCode2 ) || Input.GetKey( keyCode3 );
		}
	}


	public class UnityAnyFourKeyCodeSource : UnityAnyThreeKeyCodeSource
	{
		protected KeyCode keyCode4;
		
		
		public UnityAnyFourKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3, KeyCode keyCode4 ) : base ( keyCode1, keyCode2, keyCode3 )
		{
			this.keyCode4 = keyCode4;
		}
		

		public new bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) || Input.GetKey( keyCode2 ) || Input.GetKey( keyCode3 ) || Input.GetKey( keyCode4 );
		}
	}


	public class UnityAnyFiveKeyCodeSource : UnityAnyFourKeyCodeSource
	{
		protected KeyCode keyCode5;


		public UnityAnyFiveKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3, KeyCode keyCode4, KeyCode keyCode5 ) : base ( keyCode1, keyCode2, keyCode3, keyCode4 )
		{
			this.keyCode5 = keyCode5;
		}
		

		public new bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) || Input.GetKey( keyCode2 ) || Input.GetKey( keyCode3 ) || Input.GetKey( keyCode4 ) || Input.GetKey( keyCode5 );
		}
	}
}
