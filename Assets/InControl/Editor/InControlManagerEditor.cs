#if UNITY_EDITOR
using UnityEditor;
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
		

		private void OnEnable()
		{
			invertYAxis = serializedObject.FindProperty( "invertYAxis" );
			enableXInput = serializedObject.FindProperty( "enableXInput" );
			useFixedUpdate = serializedObject.FindProperty( "useFixedUpdate" );
			customProfiles = serializedObject.FindProperty( "customProfiles" );
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();

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