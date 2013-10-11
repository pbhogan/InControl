#if UNITY_EDITOR
using System;
using UnityEditor;
using InControl;


namespace InControl
{
	public class EditorMenu
	{
		[MenuItem("Edit/Project Settings/InControl/Generate InputManager Asset")]
		static void SetupInputManagerAsset()
		{
			var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0];
			var serializedObject = new SerializedObject( inputManagerAsset );
			var axisArray = serializedObject.FindProperty( "m_Axes" );

			axisArray.arraySize = 100;
			serializedObject.ApplyModifiedProperties();

			int axisIndex = 0;
			for (int joystick = 1; joystick <= 10; joystick++)
			{
				for (int analog = 0; analog <= 9; analog++)
				{
					var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );

					GetChildProperty( axis, "m_Name" ).stringValue = string.Format( "joystick {0} analog {1}", joystick, analog );
					GetChildProperty( axis, "descriptiveName" ).stringValue = "";
					GetChildProperty( axis, "descriptiveNegativeName" ).stringValue = "";
					GetChildProperty( axis, "negativeButton" ).stringValue = "";
					GetChildProperty( axis, "positiveButton" ).stringValue = "";
					GetChildProperty( axis, "altNegativeButton" ).stringValue = "";
					GetChildProperty( axis, "altPositiveButton" ).stringValue = "";
					GetChildProperty( axis, "gravity" ).floatValue = 10.0f;
					GetChildProperty( axis, "dead" ).floatValue = 0.001f;
					GetChildProperty( axis, "sensitivity" ).floatValue = 1.0f;
					GetChildProperty( axis, "snap" ).boolValue = false;
					GetChildProperty( axis, "invert" ).boolValue = false;
					GetChildProperty( axis, "type" ).intValue = 2;
					GetChildProperty( axis, "axis" ).intValue = analog;
					GetChildProperty( axis, "joyNum" ).intValue = joystick;
				}
			}

			serializedObject.ApplyModifiedProperties();

			EditorUtility.DisplayDialog( "Success", "InputManager asset has been initialized.", "OK" );
		}


		static SerializedProperty GetChildProperty( SerializedProperty parent, string name )
		{
			SerializedProperty child = parent.Copy();
			child.Next( true );

			do
			{
				if (child.name == name)
				{
					return child;
				}
			}
			while (child.Next( false ));

			return null;
		}
	}
}
#endif

