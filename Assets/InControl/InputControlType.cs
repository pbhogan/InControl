using System;


namespace InControl
{
	public enum InputControlType
	{
		// Standardized.
		//
		LeftStickX,
		LeftStickY,
		LeftStickButton,

		RightStickX,
		RightStickY,
		RightStickButton,

		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,

		Action1,
		Action2,
		Action3,
		Action4,

		LeftTrigger,
		RightTrigger,
		
		LeftBumper,
		RightBumper,


		// Not standardized, but provided for convenience.
		//
		Back,
		Start,
		Select,
		System,
		TiltX,
		TiltY,
		TiltZ,


		// Not standardized.
		//
		Analog0,
		Analog1,
		Analog2,
		Analog3,
		Analog4,
		Analog5,
		Analog6,
		Analog7,
		Analog8,
		Analog9,

		Button0,
		Button1,
		Button2,
		Button3,
		Button4,
		Button5,
		Button6,
		Button7,
		Button8,
		Button9,
		Button10,
		Button11,
		Button12,
		Button13,
		Button14,
		Button15,
		Button16,
		Button17,
		Button18,
		Button19,

		// Internal. Must be last.
		//
		Count 
	}
}

