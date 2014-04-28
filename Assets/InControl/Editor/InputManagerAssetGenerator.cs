#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace InControl
{
	[InitializeOnLoad]
	internal class InputManagerAssetGenerator
	{
		static List<AxisPreset> axisPresets = new List<AxisPreset>();


		static InputManagerAssetGenerator()
		{
			SetupAxisPresets();
			CheckInputManagerAsset();
		}


		[MenuItem("Edit/Project Settings/InControl/Setup Input Manager")]
		static void GenerateInputManagerAsset()
		{
			ApplyAxisPresets();
			EditorUtility.DisplayDialog( "Success", "Input Manager settings have been configured.", "OK" );
		}


		[MenuItem("Edit/Project Settings/InControl/Check Input Manager")]
		static void CheckInputManagerAsset()
		{
			if (!CheckAxisPresets())
			{
				Debug.LogError( "InControl has detected an invalid Input Manager setup. To fix, execute 'Edit > Project Settings > InControl > Setup Input Manager'." );
			}
		}


		static void SetupAxisPresets()
		{
			axisPresets.Clear();

			for (int device = 1; device <= UnityInputDevice.MaxDevices; device++)
			{
				for (int analog = 0; analog < UnityInputDevice.MaxAnalogs; analog++)
				{
					axisPresets.Add( new AxisPreset( device, analog ) );
				}
			}

			axisPresets.Add( new AxisPreset( "mouse x", 1, 0, 1.0f ) );
			axisPresets.Add( new AxisPreset( "mouse y", 1, 1, 1.0f ) );
			axisPresets.Add( new AxisPreset( "mouse z", 1, 2, 1.0f ) );

			// To avoid conflicts with NGUI
			axisPresets.Add( new AxisPreset( "Mouse ScrollWheel", 1, 2, 0.1f ) );
			axisPresets.Add( new AxisPreset( "Vertical", 2, 1, 1.0f, 0.2f, true ) );
		}


		static void ApplyAxisPresets()
		{
			var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0];
			var serializedObject = new SerializedObject( inputManagerAsset );
			var axisArray = serializedObject.FindProperty( "m_Axes" );
			
			axisArray.arraySize = Mathf.Max( axisPresets.Count, axisArray.arraySize );
			serializedObject.ApplyModifiedProperties();

			for (int i = 0; i < axisPresets.Count; i++)
			{
				var axisEntry = axisArray.GetArrayElementAtIndex( i );
				axisPresets[i].ApplyTo( ref axisEntry );
			}

			serializedObject.ApplyModifiedProperties();
			
			AssetDatabase.Refresh();
		}


		static bool CheckAxisPresets()
		{
			var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0];
			var serializedObject = new SerializedObject( inputManagerAsset );
			var axisArray = serializedObject.FindProperty( "m_Axes" );
						
			for (int i = 0; i < axisPresets.Count; i++)
			{
				var axisEntry = axisArray.GetArrayElementAtIndex( i );
				if (!axisPresets[i].Check( ref axisEntry ))
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


		internal class AxisPreset
		{
			public string name;
			public int type;
			public int axis;
			public int joyNum;
			public float sensitivity;
			public float deadZone;
			public bool invert;
			
			
			public AxisPreset( string name, int type, int axis, float sensitivity, float deadZone = 0.001f, bool invert = false )
			{
				this.name = name;
				this.type = type;
				this.axis = axis;
				this.joyNum = 0;
				this.sensitivity = sensitivity;
				this.deadZone = deadZone;
				this.invert = invert;
			}
			
			
			public AxisPreset( int device, int analog )
			{
				this.name = string.Format( "joystick {0} analog {1}", device, analog );
				this.type = 2;
				this.axis = analog;
				this.joyNum = device;
				this.sensitivity = 1.0f;
				this.deadZone = 0.001f;
			}
			
			
			public void ApplyTo( ref SerializedProperty axisPreset )
			{
				GetChildProperty( axisPreset, "m_Name" ).stringValue = this.name;
				GetChildProperty( axisPreset, "type" ).intValue = this.type;
				GetChildProperty( axisPreset, "axis" ).intValue = this.axis;
				GetChildProperty( axisPreset, "joyNum" ).intValue = this.joyNum;
				GetChildProperty( axisPreset, "sensitivity" ).floatValue = sensitivity;
				
				GetChildProperty( axisPreset, "descriptiveName" ).stringValue = "";
				GetChildProperty( axisPreset, "descriptiveNegativeName" ).stringValue = "";
				GetChildProperty( axisPreset, "negativeButton" ).stringValue = "";
				GetChildProperty( axisPreset, "positiveButton" ).stringValue = "";
				GetChildProperty( axisPreset, "altNegativeButton" ).stringValue = "";
				GetChildProperty( axisPreset, "altPositiveButton" ).stringValue = "";
				GetChildProperty( axisPreset, "gravity" ).floatValue = 0.0f;
				GetChildProperty( axisPreset, "dead" ).floatValue = this.deadZone;
				GetChildProperty( axisPreset, "snap" ).boolValue = false;
				GetChildProperty( axisPreset, "invert" ).boolValue = this.invert;
			}
			
			
			public bool Check( ref SerializedProperty axisPreset )
			{
				if (GetChildProperty( axisPreset, "m_Name" ).stringValue != this.name) return false;
				if (GetChildProperty( axisPreset, "type" ).intValue != this.type) return false;
				if (GetChildProperty( axisPreset, "axis" ).intValue != this.axis) return false;
				if (GetChildProperty( axisPreset, "joyNum" ).intValue != this.joyNum) return false;
				if (!Mathf.Approximately( GetChildProperty( axisPreset, "sensitivity" ).floatValue, this.sensitivity)) return false;
				
				return true;
			}
		}
	}
}
#endif

