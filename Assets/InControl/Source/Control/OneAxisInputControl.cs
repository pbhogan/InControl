using System;
using UnityEngine;


namespace InControl
{
	public class OneAxisInputControl
	{
		public ulong UpdateTick { get; private set; }

		InputControlState thisState;
		InputControlState lastState;


		public void UpdateWithValue( float value, ulong updateTick, float stateThreshold )
		{
			if (UpdateTick > updateTick)
			{
				throw new InvalidOperationException( "A control cannot be updated with an earlier tick." );
			}

			lastState = thisState;

			thisState.Set( value, stateThreshold );

			if (thisState != lastState)
			{
				UpdateTick = updateTick;
			}
		}


		public bool State
		{
			get { return thisState.State; }
		}


		public bool LastState
		{
			get { return lastState.State; }
		}


		public float Value
		{
			get { return thisState.Value; }
		}


		public float LastValue
		{
			get { return lastState.Value; }
		}


		public bool HasChanged
		{
			get { return thisState != lastState; }
		}


		public bool IsPressed
		{
			get { return thisState.State; }
		}


		public bool WasPressed
		{
			get { return thisState && !lastState; }
		}


		public bool WasReleased
		{
			get { return !thisState && lastState; }
		}


		public static implicit operator bool( OneAxisInputControl control )
		{
			return control.State;
		}


		public static implicit operator float( OneAxisInputControl control )
		{
			return control.Value;
		}
	}
}

