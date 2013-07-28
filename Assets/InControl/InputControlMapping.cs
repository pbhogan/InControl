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


		public float MapValue( float value )
		{
			if (Invert ^ (IsYAxis && InputManager.InvertYAxis))
			{
				value = -value;
			}

			return Mathf.Lerp( TargetRange.Min, TargetRange.Max, Mathf.InverseLerp( SourceRange.Min, SourceRange.Max, value ) );
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
