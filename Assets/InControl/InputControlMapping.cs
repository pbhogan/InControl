using System;
using UnityEngine;


namespace InControl
{
	public class InputControlMapping
	{
		public class Range
		{
			public static Range Complete = new Range() { Min = -1.0f, Max = +1.0f };
			public static Range Positive = new Range() { Min = +0.0f, Max = +1.0f };
			public static Range Negative = new Range() { Min = -1.0f, Max = -0.0f };

			public float Min;
			public float Max;
		}


		public string Source;
		public InputControlType Target;

		public bool Invert = false;

		public Range SourceRange = Range.Complete;
		public Range TargetRange = Range.Complete;

		string handle;


		float GetRangedValue( float value )
		{
			if (SourceRange == Range.Negative)
			{
				return value < 0.0f ? -value : 0.0f;
			}

			if (SourceRange == Range.Complete || value > 0.0f)
			{
				return value;
			}

			return 0.0f;
		}


		public float MapValue( float value )
		{
			if (Invert)
			{
				value = -value;
			}

			if (IsYAxis && InputManager.InvertYAxis)
			{
				value = -value;
			}

			value = GetRangedValue( value );

			return Mathf.Lerp( TargetRange.Min, TargetRange.Max, Mathf.InverseLerp( -1.0f, 1.0f, value ) );
		}


		public bool HasPositiveTargetRange
		{
			get { return TargetRange == Range.Positive; }
		}


		public string Handle
		{
			get { return (handle == null || handle == "") ? Target.ToString() : handle; }
			set { handle = value; }
		}


		bool IsYAxis 
		{
			get 
			{ 
				return Target == InputControlType.LeftStickY   || 
					   Target == InputControlType.RightStickY; 
			}
		}
	}


	// TODO: Refactor to eliminate InputControlAnalogMapping.
	public class InputControlAnalogMapping : InputControlMapping
	{
		public InputControlAnalogMapping() 
		{
		}


		public InputControlAnalogMapping( int index )
		{
			Handle = "Analog " + index;
			Source = "analog " + index;
			Target = (InputControlType) index;
		}
	}


	// TODO: Refactor to eliminate InputControlButtonMapping.
	public class InputControlButtonMapping : InputControlMapping
	{
		public InputControlButtonMapping() 
		{
		}


		public InputControlButtonMapping( int index )
		{
			Handle = "Button " + index;
			Source = "button " + index;
			Target = (InputControlType) index;
		}
	}
}
