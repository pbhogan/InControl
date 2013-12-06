using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	[AutoDiscover]
	public class OuyaProfile : UnityInputDeviceProfile
	{
		public OuyaProfile()
		{
			Name = "OUYA Controller";
			Meta = "OUYA Controller on OUYA";

			SupportedPlatforms = new[]
			{
				"OUYA",
				"Windows"
			};

			JoystickNames = new[]
			{
				"OUYA Game Controller"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.3f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "O",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlButtonMapping()
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = Button3
				},
				new InputControlButtonMapping()
				{
					Handle = "U",
					Target = InputControlType.Action3,
					Source = Button1
				},
				new InputControlButtonMapping()
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button2
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button6
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button7
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button8
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button9
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button10
				},
				new InputControlButtonMapping()
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button11
				},
				new InputControlButtonMapping()
				{
					Handle = "System",
					Target = InputControlType.System,
					Source = KeyCodeButton( KeyCode.Menu )
				}
			};

			AnalogMappings = new[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = Analog0
				},
				new InputControlAnalogMapping()
				{
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = Analog1,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = Analog2
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog3,
					Invert = true
				},
				new InputControlAnalogMapping()
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog4
				},
				new InputControlAnalogMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog5
				}
			};
		}
	}
}

