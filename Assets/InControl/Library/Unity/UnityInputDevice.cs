using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class UnityInputDevice : InputDevice
	{
		public const int MaxDevices = 10;
		public const int MaxButtons = 20;
		public const int MaxAnalogs = 20;

		public int JoystickId { get; private set; }
		public UnityInputDeviceProfile Profile { get; protected set; }


		public UnityInputDevice( UnityInputDeviceProfile profile, int joystickId )
			: base( profile.Name, profile.AnalogMappings.Length, profile.ButtonMappings.Length )
		{
			Initialize( profile, joystickId );
		}


		public UnityInputDevice( UnityInputDeviceProfile profile )
			: base( profile.Name, profile.AnalogMappings.Length, profile.ButtonMappings.Length )
		{
			Initialize( profile, 0 );
		}


		void Initialize( UnityInputDeviceProfile profile, int joystickId )
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
				SortOrder = 100 + joystickId;
				Meta += " [id: " + joystickId + "]";
			}
		}


		public override void Update( float updateTime, float deltaTime )
		{
			if (Profile == null)
			{
				return;
			}

			var analogMappingCount = Profile.AnalogMappings.Length;
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var unityValue = analogMapping.Source.GetValue( this );

				if (!analogMapping.Raw)
				{

					if (analogMapping.TargetRangeIsNotComplete &&
						Mathf.Abs(unityValue) < Mathf.Epsilon &&
						Analogs[i].UpdateTime < Mathf.Epsilon)
					{
						// Ignore initial input stream for triggers, because they report
						// zero incorrectly until the value changes for the first time.
						// Example: wired Xbox controller on Mac.
						continue;
					}

					unityValue = ApplyDeadZone( unityValue );
					unityValue = analogMapping.MapValue( unityValue );
					unityValue = SmoothAnalogValue( unityValue, Analogs[i].LastValue, deltaTime );

					Analogs[i].UpdateWithValue( unityValue, updateTime );
				}
				else
				{
					Analogs[i].UpdateWithValue( unityValue, updateTime );
				}
			}

			var buttonMappingCount = Profile.ButtonMappings.Length;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonMapping = Profile.ButtonMappings[i];
				var buttonState = buttonMapping.Source.GetState( this );

				Buttons[i].UpdateWithState( buttonState, updateTime );
			}
		}


		float ApplyDeadZone( float value )
		{
			// Make sure the value is sane.
			value = Mathf.Clamp( value, -1.0f, 1.0f );

			if (Profile.IsJoystick)
			{
				// Apply dead zones.
				return Mathf.InverseLerp( Profile.LowerDeadZone, Profile.UpperDeadZone, Mathf.Abs( value ) ) * Mathf.Sign( value );
			}

			return value;
		}


		float SmoothAnalogValue( float thisValue, float lastValue, float deltaTime )
		{
			if (Profile.IsJoystick)
			{
				// 1.0f and above is instant (no smoothing).
				if (Mathf.Approximately( Profile.Sensitivity, 1.0f ))
				{
					return thisValue;
				}

				// Apply sensitivity (how quickly the value adapts to changes).
				var maxDelta = deltaTime * Profile.Sensitivity * 100.0f;

				// Snap to zero when changing direction quickly.
				if (Mathf.Sign( lastValue ) != Mathf.Sign( thisValue ))
				{
					lastValue = 0.0f;
				}

				return Mathf.MoveTowards( lastValue, thisValue, maxDelta );
			}

			return thisValue;
		}


		public bool IsConfiguredWith( UnityInputDeviceProfile deviceProfile, int joystickId )
		{
			return Profile == deviceProfile && JoystickId == joystickId;
		}


		public override bool IsSupportedOnThisPlatform
		{
			get { return Profile.IsSupportedOnThisPlatform; }
		}
	}
}
