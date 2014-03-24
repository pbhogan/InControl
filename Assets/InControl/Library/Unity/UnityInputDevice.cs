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


		public override void Update( ulong updateTick, float deltaTime )
		{
			if (Profile == null)
			{
				return;
			}

			var analogMappingCount = Profile.AnalogMappings.Length;

			// Preprocess all analog values. This is done so
			// stick axes can refer to their obverse axes later.
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var analogValue = analogMapping.Source.GetValue( this );

				if (analogMapping.IgnoreInitialZeroValue &&
				    Analogs[i].UpdateTick == 0 &&
				    Mathf.Abs(analogValue) < Mathf.Epsilon)
				{
					continue;
				}

				Analogs[i].RawValue = analogValue;
				Analogs[i].PreValue = analogMapping.MapValue( analogValue );
			}

			// Do final processing for analogs values.
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];

				if (!analogMapping.Raw)
				{
					var analogValue = Analogs[i].PreValue;

					if (analogMapping.IgnoreInitialZeroValue &&
					    Analogs[i].UpdateTick == 0 &&
					    Mathf.Abs(analogValue) < Mathf.Epsilon)
					{
						continue;
					}

					// Axes with obverse axes (like sticks) should use circular deadzones to avoid snapping.
					var obverseTarget = analogMapping.Obverse;
					if (obverseTarget.HasValue)
					{
						var obverseControl = GetControl( obverseTarget );
						analogValue = ApplyCircularDeadZone( analogValue, obverseControl.PreValue );
					}
					else
					{
						analogValue = ApplyDeadZone( analogValue );
					}

					// Apply smoothing.
					analogValue = SmoothAnalogValue( analogValue, Analogs[i].LastValue, deltaTime );

					Analogs[i].UpdateWithValue( analogValue, updateTick );
				}
				else
				{
					Analogs[i].UpdateWithValue( Analogs[i].RawValue, updateTick );
				}
			}

			// Buttons are easy: just update the control state.
			var buttonMappingCount = Profile.ButtonMappings.Length;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonMapping = Profile.ButtonMappings[i];
				var buttonState = buttonMapping.Source.GetState( this );

				Buttons[i].UpdateWithState( buttonState, updateTick );
			}
		}


		float ApplyDeadZone( float value )
		{
			if (Profile.IsJoystick)
			{
				// Apply dead zones.
				return Mathf.InverseLerp( Profile.LowerDeadZone, Profile.UpperDeadZone, Mathf.Abs( value ) ) * Mathf.Sign( value );
			}

			return value;
		}


		float ApplyCircularDeadZone( float axisValue1, float axisValue2 )
		{
			var axisVector = new Vector2( axisValue1, axisValue2 );
			var magnitude = Mathf.InverseLerp( Profile.LowerDeadZone, Profile.UpperDeadZone, axisVector.magnitude );
			return (axisVector.normalized * magnitude).x;
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


		public override bool IsKnown
		{
			get { return Profile.IsKnown; }
		}
	}
}

