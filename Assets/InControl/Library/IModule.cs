using System;
using UnityEngine;


namespace InControl
{
	public interface IModule
	{
		void Update( ulong updateTick, float deltaTime );
	}
}

