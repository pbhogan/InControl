using System;
using UnityEngine;


namespace InControl
{
	// This is kind of "beta"... while it works on iOS, gyro controls are
	// inconsistent and are usually fine tuned to the games that use them
	// which is somewhat beyond the scope of this project. But, if you 
	// are curious how to go about it, here you go.
	//
	public class UnityGyroAxisSource : InputControlSource
	{
		public enum GyroAxis
		{
			X = 0,
			Y = 1,
		}

		int axis;

		static Quaternion zeroAttitude;

		
		public UnityGyroAxisSource( GyroAxis axis )
		{
			this.axis = (int) axis;
			Calibrate();
		}
		
		
		public float GetValue( InputDevice inputDevice )
		{
			return GetAxis()[ (int) axis ];
		}
		
		
		public bool GetState( InputDevice inputDevice )
		{
			return !Mathf.Approximately( GetValue( inputDevice ), 0.0f );
		}


		static Quaternion GetAttitude()
		{
			return Quaternion.Inverse( zeroAttitude ) * Input.gyro.attitude;
		}


		static Vector3 GetAxis()
		{
			var gv = GetAttitude() * Vector3.forward;
			var gx = ApplyDeadZone( Mathf.Clamp( gv.x, -1.0f, 1.0f ) );
			var gy = ApplyDeadZone( Mathf.Clamp( gv.y, -1.0f, 1.0f ) );
			return new Vector3( gx, gy );
		}


		static float ApplyDeadZone( float value )
		{
			return Mathf.InverseLerp( 0.05f, 1.0f, Mathf.Abs( value ) ) * Mathf.Sign( value );
		}


		public static void Calibrate()
		{
			zeroAttitude = Input.gyro.attitude;
		}
	}
}

