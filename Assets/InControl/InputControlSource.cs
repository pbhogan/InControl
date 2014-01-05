using System;


namespace InControl
{
	public abstract class InputControlSource
	{
		public abstract float GetValue( InputDevice inputDevice );
		public abstract bool  GetState( InputDevice inputDevice );
	}
}

