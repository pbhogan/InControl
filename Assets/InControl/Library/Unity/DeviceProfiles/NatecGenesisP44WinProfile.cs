using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class NatecGenesisP44Profile : UnityInputDeviceProfile
	{
		public NatecGenesisP44Profile()
		{
			Name = "Natec Genesis P44 Controller";
			Meta = "Natec Genesis P44 Controller on Windows";

			SupportedPlatforms = new[] {
				"Windows"
			};

			JoystickNames = new[] {
				"EX10 GAMEPAD"
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
					Handle = "L",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "R",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "LL",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "RR",
					Target = InputControlType.RightTrigger,
					Source = Button7
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
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = Button12
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
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				}
			};
		}
	}
}

