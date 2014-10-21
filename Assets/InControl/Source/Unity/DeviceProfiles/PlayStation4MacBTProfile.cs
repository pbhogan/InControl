using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class PlayStation4MacBTProfile : UnityInputDeviceProfile
	{
		public PlayStation4MacBTProfile()
		{
			Name = "PlayStation 4 Controller";
			Meta = "PlayStation 4 Controller on Mac";

			SupportedPlatforms = new[] {
				"OS X"
			};

			JoystickNames = new[] {
				"Unknown Wireless Controller" // Sigh.
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Triangle",
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
					Handle = "System",
					Target = InputControlType.System,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "Options",
					Target = InputControlType.Select,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Share",
					Target = InputControlType.Share,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "TouchPad Button",
					Target = InputControlType.TouchPadTap,
					Source = Button13
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
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog4,
					TargetRange = InputControlMapping.Range.Positive,
					IgnoreInitialZeroValue = true
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog5,
					TargetRange = InputControlMapping.Range.Positive,
					IgnoreInitialZeroValue = true
				},

				// OS X 10.9
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog10,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog10,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog11,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog11,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},

				// OS X 10.10
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
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog7,
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
			};
		}
	}
}

