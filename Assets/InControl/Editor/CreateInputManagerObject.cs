#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace InControl
{
	internal class CreateInputManagerObject
	{
		[MenuItem("GameObject/Create Other/InControl/Manager")]
		static void Execute()
		{
			MonoBehaviour component;
			if (component = GameObject.FindObjectOfType<InControlManager>())
			{
				Selection.activeGameObject = component.gameObject;

				Debug.LogError( "InControlManager is already setup on selected object." );
				return;
			}

			GameObject gameObject = GameObject.Find( "InControl" ) ?? new GameObject( "InControl" );
			gameObject.AddComponent<InControlManager>();
			Selection.activeGameObject = gameObject;

			Debug.Log( "InControlManager object has been created." );
		}
	}
}
#endif

