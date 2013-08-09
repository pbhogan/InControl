using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class OuyaProfile : UnityInputDeviceProfile
	{
		public OuyaProfile()
		{
			Name = "OUYA Controller";
			Meta = "OUYA Controller on OUYA";

			SupportedPlatforms = new[]
			{
				"OUYA"
			};

			JoystickNames = new[]
			{
				"OUYA Game Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.3f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "O",
					Target = InputControlType.Action1,
					Source = "button 0"
				},
				new InputControlButtonMapping()
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = "button 3"
				},
				new InputControlButtonMapping()
				{
					Handle = "U",
					Target = InputControlType.Action3,
					Source = "button 1"
				},
				new InputControlButtonMapping()
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = "button 2"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = "button 4"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = "button 5"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = "button 6"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = "button 7"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = "button 8"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = "button 9"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = "button 10"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = "button 11"
				}
			};

			AnalogMappings = new InputControlAnalogMapping[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = "analog 0"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = "analog 1",
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = "analog 2"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = "analog 3",
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = "analog 4"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = "analog 5"
				}
			};
		}
	}
}

