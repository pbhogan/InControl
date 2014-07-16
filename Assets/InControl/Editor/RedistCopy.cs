using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace InControl
{
    public class RedistCopy
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            var dllName = "XInputInterface.dll";
            var sourcePath = dllName;
            var destPath = Path.GetDirectoryName(pathToBuiltProject) + "/" + dllName;

            if (!File.Exists(sourcePath))
            {
                Debug.LogWarning("Can't find " + dllName);
                return;
            }

            if (target == BuildTarget.StandaloneWindows)
            {
                File.Copy(sourcePath, destPath, true);
            }
        }
    }

}
