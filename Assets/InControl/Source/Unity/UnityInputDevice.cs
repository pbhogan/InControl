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

		internal int JoystickId { get; private set; }
		public UnityInputDeviceProfile Profile { get; protected set; }


		public UnityInputDevice( UnityInputDeviceProfile profile, int joystickId )
			: base( profile.Name )
		{
			Initialize( profile, joystickId );
		}


		public UnityInputDevice( UnityInputDeviceProfile profile )
			: base( profile.Name )
		{
			Initialize( profile, 0 );
		}


		void Initialize( UnityInputDeviceProfile profile, int joystickId )
		{
			Profile = profile;
			Meta = Profile.Meta;

			var analogMappingCount = Profile.AnalogCount;
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var analogControl = AddControl( analogMapping.Target, analogMapping.Handle );

				analogControl.Sensitivity = Profile.Sensitivity;
				analogControl.UpperDeadZone = Profile.UpperDeadZone;
				analogControl.LowerDeadZone = Profile.LowerDeadZone;
			}

			var buttonMappingCount = Profile.ButtonCount;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonMapping = Profile.ButtonMappings[i];
				AddControl( buttonMapping.Target, buttonMapping.Handle );
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

			// Preprocess all analog values.
			var analogMappingCount = Profile.AnalogCount;
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var targetControl = GetControl( analogMapping.Target );

				var analogValue = analogMapping.Source.GetValue( this );

				if (analogMapping.IgnoreInitialZeroValue &&
				    targetControl.IsOnZeroTick &&
				    Mathf.Abs(analogValue) < Mathf.Epsilon)
				{
					targetControl.RawValue = null;
					targetControl.PreValue = null;
				}
				else
				{		
					var mappedValue = analogMapping.MapValue( analogValue );

					// TODO: This can surely be done in a more elegant fashion.
					if (analogMapping.Raw)
					{
						targetControl.RawValue = Combine( targetControl.RawValue, mappedValue );
					}
					else
					{
						targetControl.PreValue = Combine( targetControl.PreValue, mappedValue );
					}
				}
			}


			// Buttons are easy: just update the control state.
			var buttonMappingCount = Profile.ButtonCount;
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonMapping = Profile.ButtonMappings[i];
				var buttonState = buttonMapping.Source.GetState( this );

				UpdateWithState( buttonMapping.Target, buttonState, updateTick );
			}
		}


		float Combine( float? value1, float value2 )
		{
			if (value1.HasValue)
			{
				return Mathf.Abs( value1.Value ) > Mathf.Abs( value2 ) ? value1.Value : value2;
			}
			else
			{
				return value2;
			}
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

