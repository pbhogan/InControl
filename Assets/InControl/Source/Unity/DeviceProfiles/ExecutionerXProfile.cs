using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class ExecutionerXProfile : UnityInputDeviceProfile
	{
		public ExecutionerXProfile()
		{
			Name = "Executioner X Controller";
			Meta = "Executioner X Controller";

			SupportedPlatforms = new[] {
				"Windows",
				"OS X"
			};

			JoystickNames = new[] {
				"Zeroplus PS Vibration Feedback Converter",
				"Zeroplus PS Vibration Feedback Converter "
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "3",
					Target = InputControlType.Action1,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "2",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "4",
					Target = InputControlType.Action3,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "1",
					Target = InputControlType.Action4,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "Options",
					Target = InputControlType.Select,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
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
				}
			};
		}
	}
}

