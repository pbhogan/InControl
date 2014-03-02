using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class Xbox360LinuxProfile : UnityInputDeviceProfile
	{
		public Xbox360LinuxProfile()
		{
			Name = "XBox 360 Controller";
			Meta = "XBox 360 Controller on Linux";

			SupportedPlatforms = new[]
			{
				"Linux"
			};

			JoystickNames = new[]
			{
				"Controller (XBOX 360 For Windows)",
				"Controller (XBOX 360 Wireless Receiver for Windows)",
				"XBOX 360 For Windows (Controller)",
				"Controller (Gamepad F310)",
				"Controller (MLG GamePad for Xbox 360)",
				"Controller (Gamepad for Xbox 360)",
				"Controller (Rock Candy Gamepad for Xbox 360)"
			};

			RegexName = "360";

			Sensitivity = 1.0f;
			LowerDeadZone = 0.2f;

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button9
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button10
				},
                new InputControlMapping
                {
                    Handle = "DPad Left",
                    Target = InputControlType.DPadLeft,
                    Source = Button11,
                    Invert = true
                },
                new InputControlMapping
                {
                    Handle = "DPad Right",
                    Target = InputControlType.DPadRight,
                    Source = Button12,
                },
                new InputControlMapping
                {
                    Handle = "DPad Up",
                    Target = InputControlType.DPadUp,
                    Source = Button13,
                    Invert = true
                },
                new InputControlMapping
                {
                    Handle = "DPad Down",
                    Target = InputControlType.DPadDown,
                    Source = Button14,
                },
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Select,
					Source = Button6
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button7
				},
                new InputControlMapping
                {
                    Handle = "System",
                    Target = InputControlType.System,
                    Source = Button8
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
					Source = Analog3
				},
				new InputControlMapping
				{
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog4,
					Invert = true
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog2
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog5
				}
			};
		}
	}
}

