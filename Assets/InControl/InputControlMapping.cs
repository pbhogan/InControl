using System;
using UnityEngine;


namespace InControl
{
	public class InputControlMapping
	{
		protected string handle;
		protected string source;
		protected string target;
		protected bool   analog = false; // analogs have float value
		protected bool   button = false; // buttons have 0 - 1 value range
		protected bool   invert = false; // invert value
		protected string ranged = "";

		protected enum SourceRange { None = 0, Negative = -1, Positive = +1 }; 
		protected SourceRange Ranged = SourceRange.None;

		public InputControlType Target { get; protected set; }

		bool isYAxis = false;


		[TinyJSON.Load]
		protected void OnLoad()
		{
			if (handle == null || handle == "")
			{
				handle = target;
			}

			SetupRangedSource();

			Target = (InputControlType) Enum.Parse( typeof(InputControlType), target );

			isYAxis = Target == InputControlType.LeftStickY || Target == InputControlType.RightStickY;
		}


		void SetupRangedSource()
		{
			if (ranged == "-")
			{
				Ranged = SourceRange.Negative;
				return;
			}

			if (ranged == "+")
			{
				Ranged = SourceRange.Positive;
				return;
			}
		}


		public string Handle
		{
			get { return handle; }
		}


		public string Source
		{
			get { return source; }
		}


		public bool IsAnalog 
		{ 
			get { return analog; }
		}


		public bool IsButton
		{
			get { return button; }
		}


		float GetRangedValue( float value )
		{
			if (Ranged == SourceRange.Negative)
			{
				return value < 0.0f ? -value : 0.0f;
			}

			if (Ranged == SourceRange.None || value > 0.0f)
			{
				return value;
			}

			return 0.0f;
		}


		public float MapValue( float value )
		{
			if (invert)
			{
				value = -value;
			}

			if (InputManager.InvertYAxis && isYAxis)
			{
				value = -value;
			}

			value = GetRangedValue( value );

			var minimum = button ? 0.0f : -1.0f;
			return Mathf.Lerp( minimum, 1.0f, Mathf.InverseLerp( -1.0f, 1.0f, value ) );
		}
	}


	public class InputControlAnalogMapping : InputControlMapping
	{
		public InputControlAnalogMapping() 
		{
			analog = true;
		}


		public InputControlAnalogMapping( int index )
		{
			handle = "Analog " + index;
			source = "analog " + index;
			target = "Analog" + index;
			Target = (InputControlType) index;
		}
	}


	public class InputControlButtonMapping : InputControlMapping
	{
		public InputControlButtonMapping() 
		{
			analog = false;
		}


		public InputControlButtonMapping( int index )
		{
			handle = "Button " + index;
			source = "button " + index;
			target = "Button" + index;
			Target = (InputControlType) index;
		}
	}
}
