using System;
using UnityEngine;


namespace InControl
{
	public class UnitySimultaneousTwoKeyCodeSource : InputControlSource
	{
		KeyCode keyCode1;
		KeyCode keyCode2;
		
		
		public UnitySimultaneousTwoKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2 )
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
			return Input.GetKey( keyCode1 ) && Input.GetKey( keyCode2 );
		}
	}


	public class UnitySimultaneousThreeKeyCodeSource : InputControlSource
	{
		KeyCode keyCode1;
		KeyCode keyCode2;
		KeyCode keyCode3;
		
		
		public UnitySimultaneousThreeKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3 )
		{
			this.keyCode1 = keyCode1;
			this.keyCode2 = keyCode2;
			this.keyCode3 = keyCode3;
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) && Input.GetKey( keyCode2 ) && Input.GetKey( keyCode3 );
		}
	}


	public class UnitySimultaneousFourKeyCodeSource : InputControlSource
	{
		KeyCode keyCode1;
		KeyCode keyCode2;
		KeyCode keyCode3;
		KeyCode keyCode4;
		
		
		public UnitySimultaneousFourKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3, KeyCode keyCode4 )
		{
			this.keyCode1 = keyCode1;
			this.keyCode2 = keyCode2;
			this.keyCode3 = keyCode3;
			this.keyCode4 = keyCode4;
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) && Input.GetKey( keyCode2 ) && Input.GetKey( keyCode3 ) && Input.GetKey( keyCode4 );
		}
	}


	public class UnitySimultaneousFiveKeyCodeSource : InputControlSource
	{
		KeyCode keyCode1;
		KeyCode keyCode2;
		KeyCode keyCode3;
		KeyCode keyCode4;
		KeyCode keyCode5;


		public UnitySimultaneousFiveKeyCodeSource( KeyCode keyCode1, KeyCode keyCode2, KeyCode keyCode3, KeyCode keyCode4, KeyCode keyCode5 )
		{
			this.keyCode1 = keyCode1;
			this.keyCode2 = keyCode2;
			this.keyCode3 = keyCode3;
			this.keyCode4 = keyCode4;
			this.keyCode5 = keyCode5;
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return Input.GetKey( keyCode1 ) && Input.GetKey( keyCode2 ) && Input.GetKey( keyCode3 ) && Input.GetKey( keyCode4 ) && Input.GetKey( keyCode5 );
		}
	}
}
