using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Playstation4MacProfile : UnityInputDeviceProfile
	{
		public Playstation4MacProfile()
		{
			Name = "Playstation 4 Controller";
			Meta = "Playstation 4 Controller on Mac";

			SupportedPlatforms = new[]
			{
				"OS X"
			};

			JoystickNames = new[]
			{
				"Sony Computer Entertainment Wireless Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.2f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = "button 1"
				},
				new InputControlButtonMapping()
				{
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = "button 2"
				},
				new InputControlButtonMapping()
				{
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = "button 0"
				},
				new InputControlButtonMapping()
				{
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = "button 3"
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
					Handle = "Start",
					Target = InputControlType.Start,
					Source = "button 13"
				},
				new InputControlButtonMapping()
				{
					Handle = "Options",
					Target = InputControlType.Select,
					Source = "button 9"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = "button 6"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = "button 7"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = "button 10"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
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
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = "analog 6",
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = "analog 6",
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = "analog 7",
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = "analog 7",
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				}
			};
		}
	}
}

