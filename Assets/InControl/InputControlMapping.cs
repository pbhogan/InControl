using System;
using UnityEngine;


namespace InControl
{
	public class InputControlMapping
	{
		public string Source;
		public InputControlType Target;

		public bool Button = false; // buttons map onto 0 - 1 value range. TODO: Refactor into Range TargetRange
		public bool Invert = false;

		public enum Range { Complete, Negative, Positive }; 
		public Range Ranged = Range.Complete; // TODO: Rename to SourceRange

		string handle;


		float GetRangedValue( float value )
		{
			if (Ranged == Range.Negative)
			{
				return value < 0.0f ? -value : 0.0f;
			}

			if (Ranged == Range.Complete || value > 0.0f)
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

			var minimum = Button ? 0.0f : -1.0f;
			return Mathf.Lerp( minimum, 1.0f, Mathf.InverseLerp( -1.0f, 1.0f, value ) );
		}


		public bool HasPositiveTargetRange
		{
			get { return Button; }
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
