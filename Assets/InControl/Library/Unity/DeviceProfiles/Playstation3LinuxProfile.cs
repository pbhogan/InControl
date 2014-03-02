using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Playstation3LinuxProfile : UnityInputDeviceProfile
	{
		public Playstation3LinuxProfile()
		{
			Name = "Playstation 3 Controller";
			Meta = "Playstation 3 Controller on Linux";

			SupportedPlatforms = new[]
			{
				"Linux"
			};

			JoystickNames = new[]
			{
				"Sony PLAYSTATION(R)3 Controller",
                "SHENGHIC 2009/0708ZXW-V1Inc. PLAYSTATION(R)3Conteroller"
			};

			Sensitivity = 1.0f;
			LowerDeadZone = 0.2f;

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button14
				},
				new InputControlMapping
				{
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button13
				},
				new InputControlMapping
				{
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button15
				},
				new InputControlMapping
				{
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button12
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button4
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button6
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button7
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button5
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button10
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button11
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button3
				},
				new InputControlMapping
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button0
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button8
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button9
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button1
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button2
				},
                new InputControlMapping
                {
                    Handle = "System",
                    Target = InputControlType.System,
                    Source = Button16
                }
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = Analog0
				},
				new InputControlMapping
				{
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = Analog1,
					Invert = true
				},
				new InputControlMapping
				{
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = Analog2
				},
				new InputControlMapping
				{
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog3,
					Invert = true
				}
			};
		}
	}
}

