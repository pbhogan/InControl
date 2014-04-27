#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using InControl.ReorderableList;


namespace InControl
{
	[CustomEditor(typeof(InControlManager))]
	public class InControlManagerEditor : Editor
	{	
		private SerializedProperty invertYAxis;
		private SerializedProperty enableXInput;
		private SerializedProperty useFixedUpdate;
		private SerializedProperty customProfiles;
		private Texture headerTexture;
		

		private void OnEnable()
		{
			invertYAxis = serializedObject.FindProperty( "invertYAxis" );
			enableXInput = serializedObject.FindProperty( "enableXInput" );
			useFixedUpdate = serializedObject.FindProperty( "useFixedUpdate" );
			customProfiles = serializedObject.FindProperty( "customProfiles" );

			var path = AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( this ) );
			headerTexture = Resources.LoadAssetAtPath<Texture>( Path.GetDirectoryName( path ) + "/Images/InControlHeader.png" );
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var headerRect = GUILayoutUtility.GetRect( 0.0f, 7.0f );
			headerRect.y += 5.0f;
			headerRect.width = headerTexture.width;
			headerRect.height = headerTexture.height;
			GUILayout.Space( headerRect.height );
			GUI.DrawTexture( headerRect, headerTexture );

			invertYAxis.boolValue = EditorGUILayout.ToggleLeft( "Invert Y Axis", invertYAxis.boolValue );
			enableXInput.boolValue = EditorGUILayout.ToggleLeft( "Enable XInput (Windows)", enableXInput.boolValue );
			useFixedUpdate.boolValue = EditorGUILayout.ToggleLeft( "Use Fixed Update", useFixedUpdate.boolValue );

			ReorderableListGUI.Title( "Custom Profiles" );
			ReorderableListGUI.ListField( customProfiles );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif