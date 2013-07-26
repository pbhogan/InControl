using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Playstation3OuyaProfile : UnityInputDeviceProfile
	{
		public Playstation3OuyaProfile()
		{
			Name = "Playstation 3 Controller";
			Meta = "Playstation 3 Controller on OUYA";

			SupportedPlatforms = new[]
			{
				"OUYA"
			};

			JoystickNames = new[]
			{
				"PLAYSTATION(R)3 Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.2f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = "button 14"
				},
				new InputControlButtonMapping()
				{
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = "button 13"
				},
				new InputControlButtonMapping()
				{
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = "button 15"
				},
				new InputControlButtonMapping()
				{
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = "button 12"
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = "button 4"
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
					Source = "button 5"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = "button 10"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = "button 11"
				},
				new InputControlButtonMapping()
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = "button 3"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = "button 1"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = "button 2"
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

