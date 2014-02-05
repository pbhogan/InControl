using System;


namespace InControl
{
	public class InputControl
	{
		public static readonly InputControl Null = new InputControl( "NullInputControl", "" );

		public string Handle { get; private set; }
		public string Target { get; private set; }

		public float UpdateTime { get; private set; }

		InputControlState thisState;
		InputControlState lastState;


		public InputControl( string handle, string target )
		{
			Handle = handle;
			Target = target;
		}


		public void UpdateWithState( bool state, float updateTime )
		{
			if (IsNull)
			{
				throw new InvalidOperationException( "A null control cannot be updated." );
			}

			lastState = thisState;

			if (thisState != state)
			{
				UpdateTime = updateTime;
				thisState.Set( state );
			}
		}


		public void UpdateWithValue( float value, float updateTime )
		{
			if (IsNull)
			{
				throw new InvalidOperationException( "A null control cannot be updated." );
			}

			lastState = thisState;

			if (thisState != value)
			{
				UpdateTime = updateTime;
				thisState.Set( value );
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


		public bool IsNull
		{
			get { return this == Null; }
		}


		public bool IsNotNull
		{
			get { return this != Null; }
		}


		public override string ToString()
		{
			return string.Format( "[InputControl: Handle={0}, Value={1}]", Handle, Value );
		}


		public static implicit operator bool( InputControl control )
		{
			return control.State;
		}


		public static implicit operator float( InputControl control )
		{
			return control.Value;
		}
	}
}