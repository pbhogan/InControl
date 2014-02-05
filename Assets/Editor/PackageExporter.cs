#if UNITY_EDITOR
using System;
using UnityEditor;
using InControl;


namespace InControl
{
	public class PackageExporter
	{
		[MenuItem("Assets/InControl/Generate Unity Package")]
		static void CreateUnityPackage()
		{
			var assetPathNames = new string[] { 
				"Assets/InControl"
//				, "ProjectSettings/InputManager.asset"
			};
			AssetDatabase.ExportPackage( assetPathNames, "InControl.unitypackage", ExportPackageOptions.Recurse );
		}
	}
}
#endif

