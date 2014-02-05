#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


namespace InControl
{
	[InitializeOnLoad]
	public class InputManagerAssetGenerator
	{
		static InputManagerAssetGenerator()
		{
			CheckInputManagerAsset();
		}


		[MenuItem("Edit/Project Settings/InControl/Generate InputManager Asset")]
		static void GenerateInputManagerAsset()
		{
			ActuallyGenerateInputManagerAsset();
			EditorUtility.DisplayDialog( "Success", "InputManager asset has been generated.", "OK" );
		}


		static void ActuallyGenerateInputManagerAsset()
		{
			var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0];
			var serializedObject = new SerializedObject( inputManagerAsset );
			var axisArray = serializedObject.FindProperty( "m_Axes" );
			
			axisArray.arraySize = (UnityInputDevice.MaxDevices * UnityInputDevice.MaxAnalogs) + 3;
			serializedObject.ApplyModifiedProperties();
			
			int axisIndex = 0;
			for (int joystick = 1; joystick <= UnityInputDevice.MaxDevices; joystick++)
			{
				for (int analog = 0; analog < UnityInputDevice.MaxAnalogs; analog++)
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
			
			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				GetChildProperty( axis, "m_Name" ).stringValue = "mouse x";
				GetChildProperty( axis, "descriptiveName" ).stringValue = "";
				GetChildProperty( axis, "descriptiveNegativeName" ).stringValue = "";
				GetChildProperty( axis, "negativeButton" ).stringValue = "";
				GetChildProperty( axis, "positiveButton" ).stringValue = "";
				GetChildProperty( axis, "altNegativeButton" ).stringValue = "";
				GetChildProperty( axis, "altPositiveButton" ).stringValue = "";
				GetChildProperty( axis, "gravity" ).floatValue = 10.0f;
				GetChildProperty( axis, "dead" ).floatValue = 0.001f;
				GetChildProperty( axis, "sensitivity" ).floatValue = 0.1f;
				GetChildProperty( axis, "snap" ).boolValue = false;
				GetChildProperty( axis, "invert" ).boolValue = false;
				GetChildProperty( axis, "type" ).intValue = 1;
				GetChildProperty( axis, "axis" ).intValue = 0;
				GetChildProperty( axis, "joyNum" ).intValue = 0;
			}
			
			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				GetChildProperty( axis, "m_Name" ).stringValue = "mouse y";
				GetChildProperty( axis, "descriptiveName" ).stringValue = "";
				GetChildProperty( axis, "descriptiveNegativeName" ).stringValue = "";
				GetChildProperty( axis, "negativeButton" ).stringValue = "";
				GetChildProperty( axis, "positiveButton" ).stringValue = "";
				GetChildProperty( axis, "altNegativeButton" ).stringValue = "";
				GetChildProperty( axis, "altPositiveButton" ).stringValue = "";
				GetChildProperty( axis, "gravity" ).floatValue = 10.0f;
				GetChildProperty( axis, "dead" ).floatValue = 0.001f;
				GetChildProperty( axis, "sensitivity" ).floatValue = 0.1f;
				GetChildProperty( axis, "snap" ).boolValue = false;
				GetChildProperty( axis, "invert" ).boolValue = false;
				GetChildProperty( axis, "type" ).intValue = 1;
				GetChildProperty( axis, "axis" ).intValue = 1;
				GetChildProperty( axis, "joyNum" ).intValue = 0;
			}
			
			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				GetChildProperty( axis, "m_Name" ).stringValue = "mouse z";
				GetChildProperty( axis, "descriptiveName" ).stringValue = "";
				GetChildProperty( axis, "descriptiveNegativeName" ).stringValue = "";
				GetChildProperty( axis, "negativeButton" ).stringValue = "";
				GetChildProperty( axis, "positiveButton" ).stringValue = "";
				GetChildProperty( axis, "altNegativeButton" ).stringValue = "";
				GetChildProperty( axis, "altPositiveButton" ).stringValue = "";
				GetChildProperty( axis, "gravity" ).floatValue = 10.0f;
				GetChildProperty( axis, "dead" ).floatValue = 0.001f;
				GetChildProperty( axis, "sensitivity" ).floatValue = 0.1f;
				GetChildProperty( axis, "snap" ).boolValue = false;
				GetChildProperty( axis, "invert" ).boolValue = false;
				GetChildProperty( axis, "type" ).intValue = 1;
				GetChildProperty( axis, "axis" ).intValue = 2;
				GetChildProperty( axis, "joyNum" ).intValue = 0;
			}
			
			serializedObject.ApplyModifiedProperties();
			
			AssetDatabase.Refresh();
		}


		static void CheckInputManagerAsset()
		{
			if (!InputManagerAssetIsOK())
			{
				ActuallyGenerateInputManagerAsset();
				Debug.LogWarning( "Automatically configured ProjectSettings/InputManager.asset for InControl. You can do this manually at any time via 'Edit > Project Settings > InControl > Generate InputManager Asset'" );
			}
		}


		static bool InputManagerAssetIsOK()
		{
			var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0];
			var serializedObject = new SerializedObject( inputManagerAsset );
			var axisArray = serializedObject.FindProperty( "m_Axes" );
			
			if (axisArray.arraySize != (UnityInputDevice.MaxDevices * UnityInputDevice.MaxAnalogs) + 3)
			{
				return false;
			}
			
			int axisIndex = 0;
			for (int joystick = 1; joystick <= UnityInputDevice.MaxDevices; joystick++)
			{
				for (int analog = 0; analog < UnityInputDevice.MaxAnalogs; analog++)
				{
					var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
					
					if (GetChildProperty( axis, "m_Name" ).stringValue != string.Format( "joystick {0} analog {1}", joystick, analog ))
					{
						return false;
					}
				}
			}
			
			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				if (GetChildProperty( axis, "m_Name" ).stringValue != "mouse x")
				{
					return false;
				}
			}

			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				if (GetChildProperty( axis, "m_Name" ).stringValue != "mouse y")
				{
					return false;
				}
			}

			{
				var axis = axisArray.GetArrayElementAtIndex( axisIndex++ );
				if (GetChildProperty( axis, "m_Name" ).stringValue != "mouse z")
				{
					return false;
				}
			}

			return true;
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

