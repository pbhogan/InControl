using System;


namespace InControl
{
	public interface InputControlSource
	{
		float GetValue( InputDevice inputDevice );
		bool  GetState( InputDevice inputDevice );
	}
}

