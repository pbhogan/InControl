using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Playstation3WinProfile : UnityInputDeviceProfile
	{
		public Playstation3WinProfile()
		{
			Name = "Playstation 3 Controller";
			Meta = "Playstation 3 Controller on Windows (via MotioninJoy Gamepad Tool)";

			SupportedPlatforms = new[]
			{
				"Windows"
			};

			JoystickNames = new[]
			{
				"MotioninJoy Virtual Game Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.2f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button2
				},
				new InputControlButtonMapping()
				{
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlButtonMapping()
				{
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button3
				},
				new InputControlButtonMapping()
				{
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button0
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button7
				},
				new InputControlButtonMapping()
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button8
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button9
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button10
				},
				new InputControlButtonMapping()
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button11
				},
				new InputControlButtonMapping()
				{
					Handle = "System",
					Target = InputControlType.System,
					Source = Button12
				}
			};

			AnalogMappings = new[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = Analog0
				},
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = Analog1,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = Analog2
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog5,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog8,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog8,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog9,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlAnalogMapping()
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog9,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Tilt X",
					Target = InputControlType.TiltX,
					Source = Analog3
				},
				new InputControlAnalogMapping()
				{
					Handle = "Tilt Y",
					Target = InputControlType.TiltY,
					Source = Analog4
				}
			};
		}
	}
}

