using System;
using UnityEngine;


namespace InControl
{
	public class InputControlMapping
	{
		public class Range
		{
			public static Range Complete = new Range { Minimum = -1.0f, Maximum = 1.0f };
			public static Range Positive = new Range { Minimum =  0.0f, Maximum = 1.0f };
			public static Range Negative = new Range { Minimum = -1.0f, Maximum = 0.0f };

			public float Minimum;
			public float Maximum;
		}


		public InputControlSource Source;
		public InputControlType Target;

		// Invert the final mapped value.
		public bool Invert;

		// Analog values will be multiplied by this number before processing.
		public float Scale = 1.0f;

		// Raw inputs won't be processed except for scaling (mice and trackpads).
		public bool Raw;

		// This is primarily to fix a bug with the wired Xbox controller on Mac.
		public bool IgnoreInitialZeroValue;

		public Range SourceRange = Range.Complete;
		public Range TargetRange = Range.Complete;

		string handle;


		public float MapValue( float value )
		{
			float sourceValue;
			float targetValue;

			if (Raw)
			{
				targetValue = value * Scale;
			}
			else
			{
				// Scale value and clamp to a legal range.
				value = Mathf.Clamp( value * Scale, -1.0f, 1.0f );

				// Values outside of source range are invalid and return zero.
				if (value < SourceRange.Minimum || value > SourceRange.Maximum)
				{
					return 0.0f;
				}

				// Remap from source range to target range.
				sourceValue = Mathf.InverseLerp( SourceRange.Minimum, SourceRange.Maximum, value );
				targetValue = Mathf.Lerp( TargetRange.Minimum, TargetRange.Maximum, sourceValue );
			}

			if (Invert ^ (IsYAxis && InputManager.InvertYAxis))
			{
				targetValue = -targetValue;
			}

			return targetValue;
		}


		public string Handle
		{
			get { return (string.IsNullOrEmpty( handle )) ? Target.ToString() : handle; }
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
}
