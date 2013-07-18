/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OuyaPostProcessor : AssetPostprocessor
{
    // detected files
    private static Dictionary<string, DateTime> m_detectedFiles = new Dictionary<string, DateTime>();

    // post processor event
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                                               string[] movedFromPath)
    {
        if (!OuyaPanel.UsePostProcessor)
        {
            return;
        }

        if (EditorApplication.isCompiling)
        {
            return;
        }

        bool detectedCPlusPlus = false;
        bool detectedJava = false;
        foreach (string path in importedAssets)
        {
            if (Path.GetExtension(path).ToUpper().Equals(".CPP"))
            {
                if (!m_detectedFiles.ContainsKey(path) ||
                    (m_detectedFiles[path] + TimeSpan.FromSeconds(5)) < File.GetLastWriteTime(path))
                {
                    m_detectedFiles[path] = File.GetLastWriteTime(path);
                    Debug.Log(string.Format("{0} C++ change: {1}", File.GetLastWriteTime(path), path));
                    detectedCPlusPlus = true;
                }
            }
            else if (Path.GetExtension(path).ToUpper().Equals(".JAVA") &&
                !Path.GetFileName(path).ToUpper().Equals("R.JAVA"))
            {
                if (!m_detectedFiles.ContainsKey(path) ||
                    (m_detectedFiles[path] + TimeSpan.FromSeconds(5)) < File.GetLastWriteTime(path))
                {
                    m_detectedFiles[path] = File.GetLastWriteTime(path);
                    Debug.Log(string.Format("{0} Java change: {1}", File.GetLastWriteTime(path), path));
                    detectedJava = true;
                }
            }
        }

        if (detectedCPlusPlus)
        {
            //compile NDK
            OuyaPanel.CompileNDK();
        }

        if (detectedJava)
        {
            //compile Plugin
            OuyaMenuAdmin.MenuGeneratePluginJar();

            //compile Application Java
            OuyaPanel.CompileApplicationJava();
        }
    }
}