#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace InControl
{
	[InitializeOnLoad]
	internal class ProfileListGenerator
	{
		static ProfileListGenerator()
		{
			DiscoverProfiles();
		}
		
		
		static void DiscoverProfiles()
		{
			var unityInputDeviceProfileType = typeof(InControl.UnityInputDeviceProfile);
			var autoDiscoverAttributeType = typeof(InControl.AutoDiscover);

			var code2 = "";
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (type.IsSubclassOf( unityInputDeviceProfileType ))
					{
						var typeAttrs = type.GetCustomAttributes( autoDiscoverAttributeType, false );
						if (typeAttrs != null && typeAttrs.Length > 0)
						{
							//code2 += "\t\t\tnew " + type.Name + "(),\n";
							code2 += "\t\t\t\"" + type.FullName + "\",\n";
						}
					}
				}
			}

			var instance = ScriptableObject.CreateInstance<UnityInputDeviceProfileList>(); 
			var filePath = AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( instance ) );
			ScriptableObject.DestroyImmediate( instance );

			string code1 = @"using System;
using UnityEngine;


namespace InControl
{
	public class UnityInputDeviceProfileList : ScriptableObject
	{
		public static string[] Profiles = new string[] 
		{
";

			string code3 = 
@"		};
	}
}";
			
			if (PutFileContents( filePath, code1 + code2 + code3 ))
			{
				Debug.Log( "InControl has updated the autodiscover profiles list." );
			}
		}


		static string GetFileContents( string fileName )
		{			
			StreamReader streamReader = new StreamReader( fileName );;			
			var fileContents = streamReader.ReadToEnd();
			streamReader.Close();
			
			return fileContents;
		}


		static bool PutFileContents( string filePath, string content )
		{
			var oldContent = GetFileContents( filePath );
			if (content == oldContent)
			{
				return false;
			}

			StreamWriter streamWriter = new StreamWriter( filePath );
			streamWriter.Write( content );
			streamWriter.Flush();
			streamWriter.Close();

			return true;
		}
	}
}
#endif

