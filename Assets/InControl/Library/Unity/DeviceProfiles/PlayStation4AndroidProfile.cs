using System;


namespace InControl
{
	// @cond nodoc
	// Tested with Samsung Galaxy Note 2 connected by OTG cable.
	[AutoDiscover]
	public class PlayStation4AndroidProfile : UnityInputDeviceProfile
	{
		public PlayStation4AndroidProfile()
		{
			Name = "PlayStation 4 Controller";
			Meta = "PlayStation 4 Controller on Android";

			SupportedPlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"Sony Computer Entertainment Wireless Controller"
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
					Source = Button13
				},
				new InputControlMapping {
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button14
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "Options",
					Target = InputControlType.Select,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button10
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
					Source = Analog13
				},
				new InputControlMapping {
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog14,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog2,
					SourceRange = InputControlMapping.Range.Complete,
					TargetRange = InputControlMapping.Range.Positive,
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog3,
					SourceRange = InputControlMapping.Range.Complete,
					TargetRange = InputControlMapping.Range.Positive,
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog4,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog4,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive,
				}
			};
		}
	}
}

