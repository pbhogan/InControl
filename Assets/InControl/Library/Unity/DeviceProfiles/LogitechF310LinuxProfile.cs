using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class LogitechF310LinuxProfile : UnityInputDeviceProfile
	{
		public LogitechF310LinuxProfile()
		{
			Name = "Logitech F310 Controller";
			Meta = "Logitech F310 Controller on Linux";

			SupportedPlatforms = new[] {
				"Linux"
			};

			JoystickNames = new[] {
				"Logitech Gamepad F310"
			};

			MaxUnityVersion = new VersionInfo( 4, 9 );

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button3
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
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlType.Select,
					Source = Button6
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
					Source = Analog3
				},
				new InputControlMapping {
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog4,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog2
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog5
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog6,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog6,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog7,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog7,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				}
			};
		}
	}
}

