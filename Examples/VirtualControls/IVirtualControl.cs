using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	public interface IVirtualControl
	{
		void Update( IEnumerable<FTouch> touches, InputDevice device, float updateTime );
	}
}
