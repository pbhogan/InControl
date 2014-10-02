using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class LogitechRumblePad2WinProfile : UnityInputDeviceProfile
	{
		public LogitechRumblePad2WinProfile()
		{
			Name = "Logitech RumblePad 2 Controller";
			Meta = "Logitech RumblePad 2 Controller on Windows";

			SupportedPlatforms = new[] {
				"Windows"
			};

			JoystickNames = new[] {
				"Logitech Rumblepad 2 USB"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "1",
					Target = InputControlType.Action3,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "2",
					Target = InputControlType.Action1,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "3",
					Target = InputControlType.Action2,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "4",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "9",
					Target = InputControlType.Back,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "10",
					Target = InputControlType.Start,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button7
				}
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = Analog0
				},
				new InputControlMapping {
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = Analog1,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = Analog2
				},
				new InputControlMapping {
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog3,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog4,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog4

				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog5,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog5
				},
			};
		}
	}
}

