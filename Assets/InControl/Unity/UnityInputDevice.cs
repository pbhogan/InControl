using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class UnityInputDevice : InputDevice
	{
		public int JoystickId { get; private set; }


		public UnityInputDevice( UnityInputDeviceProfile profile, int joystickId = 0 )
			: base( profile.Name, profile.AnalogMappings.Length, profile.ButtonMappings.Length )
		{
			Profile = profile;

			Meta = Profile.Meta;

			foreach (var analogMapping in Profile.AnalogMappings)
			{
				AddAnalogControl( analogMapping.Target, analogMapping.Handle );
			}

			foreach (var buttonMapping in Profile.ButtonMappings)
			{
				AddButtonControl( buttonMapping.Target, buttonMapping.Handle );
			}

			JoystickId = joystickId;

			if (joystickId != 0)
			{
				Meta += " [id: " + joystickId + "]";
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
				source = "joystick " + JoystickId + " " + source;
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
				source = "joystick " + JoystickId + " " + source;
			}

			return Input.GetKey( source );
		}


		public bool IsConfiguredWith( UnityInputDeviceProfile deviceProfile, int joystickId )
		{
			return Profile == deviceProfile && JoystickId == joystickId;
		}
	}
}
