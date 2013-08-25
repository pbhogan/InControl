using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Xbox360MacProfile : UnityInputDeviceProfile
	{
		public Xbox360MacProfile()
		{
			Name = "XBox 360 Controller";
			Meta = "XBox 360 Controller on Mac";

			SupportedPlatforms = new[]
			{
				"OS X"
			};

			JoystickNames = new[]
			{
				"",
				"Microsoft Wireless 360 Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.3f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = "button 16"
				},
				new InputControlButtonMapping()
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = "button 17"
				},
				new InputControlButtonMapping()
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = "button 18"
				},
				new InputControlButtonMapping()
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = "button 19"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = "button 5"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = "button 6"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = "button 7"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = "button 8"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = "button 13"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = "button 14"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = "button 11"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = "button 12"
				},
				new InputControlButtonMapping()
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = "button 9"
				},
				new InputControlButtonMapping()
				{
					Handle = "Back",
					Target = InputControlType.Select,
					Source = "button 10"
				},
				new InputControlButtonMapping()
				{
					Handle = "System",
					Target = InputControlType.System,
					Source = "button 15"
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
					Source = "analog 4",
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = "analog 5",
					TargetRange = InputControlMapping.Range.Positive
				}
			};
		}
	}
}

