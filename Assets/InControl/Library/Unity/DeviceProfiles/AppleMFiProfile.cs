using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class AppleMFiProfile : UnityInputDeviceProfile
	{
		public AppleMFiProfile()
		{
			Name = "Apple MFi Controller";
			Meta = "Apple MFi Controller on iOS";

			SupportedPlatforms = new[] {
				"iPhone"
			};

			LastResortRegex = ""; // Match anything.

			LowerDeadZone = 0.05f;
			UpperDeadZone = 0.95f;

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button14
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button13
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button15
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Pause",
					Target = InputControlType.Pause,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button11
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
				}
			};
		}
	}
}

