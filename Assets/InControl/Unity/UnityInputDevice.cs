using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class UnityInputDevice : InputDevice
	{
		public int UnityJoystickId { get; private set; }


		public UnityInputDevice( InputDeviceProfile deviceProfile, int unityJoystickId = 0 )
			: base( deviceProfile )
		{
			UnityJoystickId = unityJoystickId;

			if (unityJoystickId != 0)
			{
				Meta += " [id: " + unityJoystickId + "]";
			}
		}


		protected override float GetAnalogValue( string source )
		{
			if (source == "")
			{
				return 0.0f;
			}

			if (Profile.IsJoystick)
			{
				source = "joystick " + UnityJoystickId + " " + source;
				return Input.GetAxisRaw( source );
			}
			else
			{
				string[] axisStringSplit = source.Split( ' ' );

				int axisVal = 0;

				if (Input.GetKey( axisStringSplit[0] ))
				{
					axisVal--;
				}

				if (Input.GetKey( axisStringSplit[1] ))
				{
					axisVal++;
				}

				return axisVal;
			}
		}


		protected override bool GetButtonState( string source )
		{		
			if (source == "")
			{
				return false;
			}

			if (Profile.IsJoystick)
			{
				source = "joystick " + UnityJoystickId + " " + source;
			}

			return Input.GetKey( source );
		}


		public bool IsConfiguredWith( InputDeviceProfile deviceProfile, int unityJoystickId )
		{
			return Profile == deviceProfile && UnityJoystickId == unityJoystickId;
		}
	}
}
