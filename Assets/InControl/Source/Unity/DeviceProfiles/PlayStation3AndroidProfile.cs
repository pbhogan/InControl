using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class PlayStation3AndroidProfile : UnityInputDeviceProfile
	{
		public PlayStation3AndroidProfile()
		{
			Name = "PlayStation 3 Controller";
			Meta = "PlayStation 3 Controller on Android";

			SupportedPlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"", // Yes, really.
				"PLAYSTATION(R)3 Controller",
				"SHENGHIC 2009/0708ZXW-V1Inc. PLAYSTATION(R)3Conteroller", // Not a typo.
				"Sony PLAYSTATION(R)3 Controller"
			};

			LastResortRegex = "PLAYSTATION(R)3";

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button1
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
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "System",
					Target = InputControlType.System,
					Source = KeyCodeButton( KeyCode.Menu )
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
					SourceRange = InputControlMapping.Range.Negative,
					TargetRange = InputControlMapping.Range.Negative,
					Invert = true
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog5,
					SourceRange = InputControlMapping.Range.Positive,
					TargetRange = InputControlMapping.Range.Positive
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog6
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog7
				}
			};
		}
	}
}

