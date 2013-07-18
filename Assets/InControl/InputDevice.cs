using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace InControl
{
	public class InputDevice
	{
		public int UnityJoystickId { get; private set; }
		public InputDeviceProfile Profile { get; private set; }	
		public string Name { get; private set; }
		public string Meta { get; private set; }
		public float UpdateTime { get; private set; }
		public InputControl[] Analogs { get; private set; }
		public InputControl[] Buttons { get; private set; }

		InputControl[] controlTable;


		public InputDevice( InputDeviceProfile deviceProfile, int unityJoystickId = 0 )
		{
			UnityJoystickId = unityJoystickId;
			Profile = deviceProfile;

			Name = deviceProfile.Name;

			Meta = deviceProfile.Meta;
			if (unityJoystickId != 0)
			{
				Meta += " [id: " + unityJoystickId + "]";
			}

			controlTable = new InputControl[ InputManager.NumInputControlTypes ];

			var analogMappingCount = Profile.AnalogMappings.GetLength( 0 );
			Analogs = new InputControl[ analogMappingCount ];
			for (int i = 0; i < analogMappingCount; i++)
			{
				Analogs[i] = new InputControl( Profile.AnalogMappings[i].Handle );
				var targetControl = (int) Profile.AnalogMappings[i].Target;
				controlTable[targetControl] = Analogs[i];
			}

			var buttonMappingCount = Profile.ButtonMappings.GetLength( 0 );
			Buttons = new InputControl[ buttonMappingCount ];
			for (int i = 0; i < buttonMappingCount; i++)
			{
				Buttons[i] = new InputControl( Profile.ButtonMappings[i].Handle );
				var targetControl = (int) Profile.ButtonMappings[i].Target;
				controlTable[targetControl] = Buttons[i];
			}

			Update( 0.0f );
		}


		public InputControl GetControl( Enum inputAnalog )
		{
			int controlIndex = Convert.ToInt32( inputAnalog );
			return controlTable[ controlIndex ];
		}


		public void Update( float updateTime )
		{
			bool stateChanged = false;
		
			var analogMappingCount = Profile.AnalogMappings.GetLength( 0 );
			for (int i = 0; i < analogMappingCount; i++)
			{
				var analogMapping = Profile.AnalogMappings[i];
				var unityValue = GetUnityAnalogValue( analogMapping.Source );

				if (analogMapping.IsButton && unityValue == 0.0f && Analogs[i].UpdateTime == 0.0f)
				{
					// Ignore initial input stream for triggers, because they report 
					// zero incorrectly until the value changes for the first time.
					// Example: wired Xbox controller on Mac.
					continue;
				}
				
				var smoothValue = SmoothAnalogValue( unityValue, Analogs[i].LastValue );
				var mappedValue = analogMapping.MapValue( smoothValue );
				Analogs[i].UpdateWithValue( mappedValue, updateTime );
				stateChanged = stateChanged || Analogs[i].HasChanged;
			}

			var buttonMappingCount = Profile.ButtonMappings.GetLength( 0 );
			for (int i = 0; i < buttonMappingCount; i++)
			{
				var buttonSource = Profile.ButtonMappings[i].Source;
				Buttons[i].UpdateWithState( GetUnityButtonState( buttonSource ), updateTime );
				stateChanged = stateChanged || Buttons[i].HasChanged;
			}

			if (stateChanged)
			{
				UpdateTime = updateTime;
			}
		}

		
		float SmoothAnalogValue( float thisValue, float lastValue )
		{
			if (Profile.IsJoystick)
			{
				// Apply dead zone.
				thisValue = Mathf.InverseLerp( Profile.DeadZone, 1.0f, Mathf.Abs( thisValue ) ) * Mathf.Sign( thisValue );

				// Apply sensitivity (how quickly the value adapts to changes).
				float maxDelta = Time.deltaTime * Profile.Sensitivity * 100.0f;
				
				// Move faster towards zero when changing direction.
				if (Mathf.Sign( lastValue ) != Mathf.Sign( thisValue )) 
				{
				   maxDelta *= 2;
				}
				
				return Mathf.Clamp( lastValue + Mathf.Clamp( thisValue - lastValue, -maxDelta, maxDelta ), -1.0f, 1.0f );
			}
			else
			{
				return thisValue;
			}
		}


		float GetUnityAnalogValue( string source )
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


		bool GetUnityButtonState( string source )
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
	}
}
