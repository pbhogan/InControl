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
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class OuyaPanel : EditorWindow
{
    private const string KEY_POST_PROCESSOR = "OuyaPanelUsePostProcessor";

    public static bool UsePostProcessor
    {
        get
        {
            if (EditorPrefs.HasKey(OuyaPanel.KEY_POST_PROCESSOR))
            {
                return EditorPrefs.GetBool(OuyaPanel.KEY_POST_PROCESSOR);
            }
            else
            {
                return false;
            }
        }
        set
        {
            EditorPrefs.SetBool(OuyaPanel.KEY_POST_PROCESSOR, value);
        }
    }

    private const string KEY_ERROR_STOP_BUILD = "OuyaPanelErrorStopBuild";

    public static bool StopOnErrors
    {
        get
        {
            if (EditorPrefs.HasKey(OuyaPanel.KEY_ERROR_STOP_BUILD))
            {
                return EditorPrefs.GetBool(OuyaPanel.KEY_ERROR_STOP_BUILD);
            }
            else
            {
                return true;
            }
        }
        set
        {
            EditorPrefs.SetBool(OuyaPanel.KEY_ERROR_STOP_BUILD, value);
        }
    }

    private const string KEY_ADB_IP = "OuyaPanelAdbIpAddress";

    public static string AdbIpAddress
    {
        get
        {
            if (EditorPrefs.HasKey(OuyaPanel.KEY_ADB_IP))
            {
                return EditorPrefs.GetString(OuyaPanel.KEY_ADB_IP);
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            EditorPrefs.SetString(OuyaPanel.KEY_ADB_IP, value);
        }
    }

    private static string[] m_toolSets =
        {
            "OUYA",
            "Unity",
            "Java JDK",
            "Android SDK",
            "Android NDK"
        };

    private int m_selectedToolSet = 0;

    #region Operations

    private bool m_toggleRunApplication = false;

    private bool m_toggleBuildApplication = false;

    private bool m_toggleBuildAndRunApplication = false;

    private bool m_toggleBuildRunAndCompileApplication = false;

    private bool m_toggleCompileJava = false;

    private bool m_toggleCompilePlugin = false;

    private bool m_toggleCompileNDK = false;

    private bool m_toggleSyncBundleID = false;

    private bool m_toggleOpenAndroidSDK = false;

    #endregion

    #region OUYA SDK

    public const string KEY_PATH_OUYA_SDK = @"OUYA SDK";
    public const string KEY_PATH_JAR_GUAVA = @"Guava Jar";
    public const string KEY_PATH_JAR_GSON = @"GSON Jar";
    public const string KEY_PATH_JAR_OUYA_UNITY_PLUGIN = @"OUYA/Plugin Jar";
    public const string KEY_JAVA_APP_NAME = @"OuyaJavaAppName";
    public const string KEY_APK_NAME = @"OuyaJavaApkName";

    private static string pathOuyaSDKJar = string.Empty;
    private static string pathGsonJar = string.Empty;
    private static string pathGuavaJar = string.Empty;
    private static string pathOuyaUnityPluginJar = string.Empty;

    private static string pathManifestPath = string.Empty;
    private static string pathRes = string.Empty;
    private static string pathBin = string.Empty;
    private static string pathSrc = string.Empty;

#if UNITY_4 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
    private static string javaAppName = "OuyaNativeActivity";
    private static string apkName = "OuyaNativeActivity.apk";
#else
    private static string javaAppName = "OuyaUnityApplication";
    private static string apkName = "OuyaUnityApplication.apk";
#endif

    void UpdateOuyaPaths()
    {
        pathOuyaSDKJar = string.Format("{0}/Assets/Plugins/Android/libs/ouya-sdk.jar", pathUnityProject);
        pathGsonJar = string.Format("{0}/Assets/Plugins/Android/libs/gson-2.2.2.jar", pathUnityProject);
        pathGuavaJar = string.Format("{0}/Assets/Plugins/Android/libs/guava-r09.jar", pathUnityProject);
        pathOuyaUnityPluginJar = string.Format("{0}/Assets/Plugins/Android/OuyaUnityPlugin.jar", pathUnityProject);

        pathManifestPath = string.Format("{0}/Assets/Plugins/Android/AndroidManifest.xml", pathUnityProject);
        pathRes = string.Format("{0}/Assets/Plugins/Android/res", pathUnityProject);
        pathBin = string.Format("{0}/Assets/Plugins/Android/bin", pathUnityProject);
        pathSrc = string.Format("{0}/Assets/Plugins/Android/src", pathUnityProject);

        EditorPrefs.SetString(KEY_PATH_OUYA_SDK, pathOuyaSDKJar);
    }

    static string GetRJava()
    {
        if (string.IsNullOrEmpty(PlayerSettings.bundleIdentifier))
        {
            return string.Empty;
        }

        string path = string.Format("Assets/Plugins/Android/src/{0}/R.java", PlayerSettings.bundleIdentifier.Replace(".", "/"));
        FileInfo fi = new FileInfo(path);
        return fi.FullName;
    }

    private static string GetApplicationJava()
    {
        string path = string.Format("Assets/Plugins/Android/src/{0}.java", javaAppName);
        FileInfo fi = new FileInfo(path);
        return fi.FullName;
    }

    private static DateTime timerCheckAppJavaPackageName = DateTime.MinValue;
    private static string applicationJavaPackageName = string.Empty;
    private static string GetApplicationJavaPackageName()
    {
        string path = GetApplicationJava();

        if (!File.Exists(path))
        {
            return "NOT_FOUND";
        }

        if (timerCheckAppJavaPackageName < DateTime.Now)
        {
            timerCheckAppJavaPackageName = DateTime.Now + TimeSpan.FromSeconds(1);
            try
            {
                using (StreamReader sr = new StreamReader(GetApplicationJava()))
                {
                    string line = string.Empty;
                    do
                    {
                        line = sr.ReadLine();
                        if (line.Trim().StartsWith("package"))
                        {
                            applicationJavaPackageName = line;
                            break;
                        }

                    } while (null != line);
                }
            }
            catch (System.Exception)
            {
            }
        }

        return applicationJavaPackageName;
    }

    private static DateTime timerCheckManifestPackageName = DateTime.MinValue;
    private static string androidManifestPackageName = string.Empty;
    private static string GetAndroidManifestPackageName()
    {
        if (!File.Exists(pathManifestPath))
        {
            return "NOT_FOUND";
        }

        if (timerCheckManifestPackageName < DateTime.Now)
        {
            timerCheckManifestPackageName = DateTime.Now + TimeSpan.FromSeconds(1);
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pathManifestPath);
                foreach (XmlNode level1 in xDoc.ChildNodes)
                {
                    if (level1.Name.ToUpper() == "MANIFEST")
                    {
                        XmlElement element = (XmlElement) level1;
                        foreach (XmlAttribute attribute in element.Attributes)
                        {
                            if (attribute.Name.ToUpper() == "PACKAGE")
                            {
                                androidManifestPackageName = attribute.Value;
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        return androidManifestPackageName;
    }

    string GetIOuyaActivityJava()
    {
        string path = "Assets/Plugins/Android/src/IOuyaActivity.java";
        FileInfo fi = new FileInfo(path);
        return fi.FullName;
    }

    static string GetBundlePrefix()
    {
        string identifier = PlayerSettings.bundleIdentifier;
        if (string.IsNullOrEmpty(identifier))
        {
            return string.Empty;
        }

        foreach (string data in identifier.Split(".".ToCharArray()))
        {
            return data;
        }

        return string.Empty;
    }

    #endregion

    #region Android SDK

    public const string KEY_PATH_ANDROID_JAR = @"Android Jar";
    public const string KEY_PATH_ANDROID_ADB = @"ADB Path";
    public const string KEY_PATH_ANDROID_AAPT = @"APT Path";
    public const string KEY_PATH_ANDROID_SDK = @"SDK Path";

    public const string REL_ANDROID_PLATFORM_TOOLS = "platform-tools";
    public const string FILE_AAPT_WIN = "aapt.exe";
    public const string FILE_AAPT_MAC = "aapt";
    public const string FILE_ADB_WIN = "adb.exe";
    public const string FILE_ADB_MAC = "adb";

    public static string pathADB = string.Empty;
    public static string pathAAPT = string.Empty;
    public static string pathSDK = string.Empty;

    private string m_browserUrl = "https://devs.ouya.tv/developers/docs/unity";

    static string GetPathAndroidJar()
    {
        return string.Format("{0}/platforms/android-{1}/android.jar", pathSDK, (int)PlayerSettings.Android.minSdkVersion);
    }

    public static void FindFile(DirectoryInfo searchFolder, string searchFile, ref string path)
    {
        if (null == searchFolder)
        {
            return;
        }
        foreach (FileInfo file in searchFolder.GetFiles(searchFile))
        {
            if (string.IsNullOrEmpty(file.FullName))
            {
                continue;
            }
            path = file.FullName;
            return;
        }
        foreach (DirectoryInfo subDir in searchFolder.GetDirectories())
        {
            if (null == subDir)
            {
                continue;
            }
            if (subDir.Name.ToUpper().Equals(".SVN"))
            {
                continue;
            }
            if (subDir.Name.ToUpper().Equals(".GIT"))
            {
                continue;
            }
            //Debug.Log(string.Format("Directory: {0}", subDir));
            FindFile(subDir, searchFile, ref path);
        }
    }

    void UpdateAndroidSDKPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                FindFile(new DirectoryInfo(string.Format("{0}", pathSDK)), FILE_ADB_MAC, ref pathADB);
                FindFile(new DirectoryInfo(string.Format("{0}", pathSDK)), FILE_AAPT_MAC, ref pathAAPT);
                pathADB = pathADB.Replace(@"\", "/");
                pathAAPT = pathAAPT.Replace(@"\", "/");
                break;
            case RuntimePlatform.WindowsEditor:
                FindFile(new DirectoryInfo(string.Format("{0}", pathSDK)), FILE_ADB_WIN, ref pathADB);
                FindFile(new DirectoryInfo(string.Format("{0}", pathSDK)), FILE_AAPT_WIN, ref pathAAPT);
                break;
        }

        EditorPrefs.SetString(KEY_PATH_ANDROID_SDK, pathSDK);
    }

    void ResetAndroidSDKPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathSDK = @"android-sdk-mac_x86";
                break;
            case RuntimePlatform.WindowsEditor:
                pathSDK = @"C:/Program Files (x86)/Android/android-sdk";
                break;
        }

        UpdateAndroidSDKPaths();
    }

    void SelectAndroidSDKPaths()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_ANDROID_SDK), pathSDK, "../android-sdk-mac_x86");
                break;
            case RuntimePlatform.WindowsEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_ANDROID_SDK), pathSDK, @"..\android-sdk");
                break;
        }
        if (!string.IsNullOrEmpty(path))
        {
            pathSDK = path;
        }

        UpdateAndroidSDKPaths();
    }

    #endregion

    #region Android NDK

    private const string KEY_PATH_ANDROID_NDK = @"NDK Path";
    private const string KEY_PATH_ANDROID_NDK_MAKE = @"NDK Make";
    private const string KEY_PATH_ANDROID_JNI_MK = @"JNI mk";
    private const string KEY_PATH_ANDROID_JNI_CPP = @"JNI cpp";
    private const string KEY_PATH_OUYA_NDK_LIB = @"OUYA NDK Lib";

    public static string pathNDK = string.Empty;
    public static string pathNDKMake = string.Empty;
    public static string pathJNIMk = string.Empty;
    public static string pathJNICpp = string.Empty;
    public static string pathObj = string.Empty;
    public static string pathOuyaNDKLib = string.Empty;

    #region NDK Paths

    void UpdateAndroidNDKPaths()
    {
        pathObj = string.Format("{0}/Assets/Plugins/Android/obj", pathUnityProject);
        pathJNIMk = string.Format("{0}/Assets/Plugins/Android/jni/Android.mk", pathUnityProject);

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                //change this code to search instead
                pathJNICpp = string.Format("{0}/Assets/Plugins/Android/jni/jni.cpp", pathUnityProject);
                pathOuyaNDKLib = string.Format("{0}/Assets/Plugins/Android/libs/armeabi/lib-ouya-ndk.so", pathUnityProject);
                break;
            case RuntimePlatform.WindowsEditor:
                pathJNICpp = string.Format("{0}/Assets/Plugins/Android/jni/jni.cpp", pathUnityProject);
                pathOuyaNDKLib = string.Format("{0}/Assets/Plugins/Android/libs/armeabi/lib-ouya-ndk.so", pathUnityProject);
                break;
        }

        EditorPrefs.SetString(KEY_PATH_ANDROID_NDK, pathNDK);
    }

    void ResetAndroidNDKPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathNDK = @"~/";
                break;
            case RuntimePlatform.WindowsEditor:
                pathNDK = @"C:/";
                break;
        }

        UpdateAndroidNDKPaths();
    }

    void SelectAndroidNDKPaths()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_ANDROID_NDK), pathNDK, "../android-ndk-r8e");
                break;
            case RuntimePlatform.WindowsEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_ANDROID_NDK), pathNDK, @"..\android-ndk-r8e");
                break;
        }
        if (!string.IsNullOrEmpty(path))
        {
            pathNDK = path;
            UpdateAndroidNDKPaths();
        }
    }

    #endregion

    #region NDK Make Path

    void UpdateAndroidNDKMakePath()
    {
        EditorPrefs.SetString(KEY_PATH_ANDROID_NDK_MAKE, pathNDKMake);
    }

    void ResetAndroidNDKMakePath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathNDKMake = string.Format("{0}/prebuilt/darwin-x86_64/bin/make", pathNDK);
                break;
            case RuntimePlatform.WindowsEditor:
                pathNDKMake = string.Format("{0}/prebuilt/windows-x86_64/bin/make.exe", pathNDK);
                break;
        }

        UpdateAndroidNDKMakePath();
    }

    void SelectAndroidNDKMakePath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                path = EditorUtility.OpenFilePanel(string.Format("Path to {0}", KEY_PATH_ANDROID_NDK_MAKE), pathNDKMake, string.Empty);
                break;
            case RuntimePlatform.WindowsEditor:
                path = EditorUtility.OpenFilePanel(string.Format("Path to {0}", KEY_PATH_ANDROID_NDK_MAKE), pathNDKMake, @"exe");
                break;
        }
        if (!string.IsNullOrEmpty(path))
        {
            pathNDKMake = path;
            UpdateAndroidNDKMakePath();
        }
    }

    #endregion

    public static void CompileNDK()
    {
        if (!File.Exists(pathNDKMake))
        {
            return;
        }

        if (!File.Exists(pathJNICpp))
        {
            return;
        }

        if (!File.Exists(pathJNIMk))
        {
            return;
        }

        if (File.Exists(pathOuyaNDKLib))
        {
            File.Delete(pathOuyaNDKLib);
        }

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                RunProcess(pathNDKMake, "Assets/Plugins/Android",
                    string.Format("-f \"{0}/build/core/build-local.mk\" -ouya-ndk",
                        pathNDK.Replace(@"/", "/")));
                break;
            case RuntimePlatform.WindowsEditor:
                RunProcess(pathNDKMake, "Assets/Plugins/Android",
                    string.Format("-f \"{0}/build/core/build-local.mk\" -ouya-ndk",
                        pathNDK.Replace(@"/", "/")));
                break;
        }

        if (Directory.Exists(pathObj))
        {
            Directory.Delete(pathObj, true);
        }
    }

    public static void SyncBundleID()
    {
        string bundleId = PlayerSettings.bundleIdentifier;
        if (string.IsNullOrEmpty(bundleId))
        {
            return;
        }

        try
        {
            string path = GetApplicationJava();
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(GetApplicationJava()))
                {
                    bool foundPackge = false;
                    string line;
                    do
                    {
                        line = sr.ReadLine();
                        if (null != line)
                        {
                            if (!foundPackge)
                            {
                                if (line.Trim().StartsWith("package"))
                                {
                                    sb.AppendFormat("package {0};", bundleId);
                                    sb.AppendLine();

                                    foundPackge = true;
                                }
                                else
                                {
                                    sb.Append(line);
                                    sb.AppendLine();
                                }
                            }
                            else
                            {
                                sb.Append(line);
                                sb.AppendLine();
                            }
                        }
                    } while (null != line);
                }

                using (StreamWriter sw = new StreamWriter(GetApplicationJava()))
                {
                    sw.Write(sb.ToString());
                }
            }
            else
            {
                Debug.LogError(string.Format("Unable to find application java: {0}", path));
            }
        }
        catch (System.Exception)
        {
        }

        try
        {
            string path = pathManifestPath;
            if (File.Exists(path))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);

                //Debug.Log("Processing Android.manifest");

                foreach (XmlNode nodeManifest in xDoc.ChildNodes)
                {
                    if (!(nodeManifest is XmlElement))
                    {
                        continue;
                    }

                    //Debug.Log(nodeManifest.Name);

                    XmlElement manifest = nodeManifest as XmlElement;

                    //Debug.Log(manifest.Name);

                    if (nodeManifest.Name.ToUpper() == "MANIFEST")
                    {
                        foreach (XmlAttribute attribute in manifest.Attributes)
                        {
                            if (attribute.Name.ToUpper() == "PACKAGE")
                            {
                                attribute.Value = bundleId;
                            }
                        }
                        foreach (XmlElement application in manifest.ChildNodes)
                        {
                            //Debug.Log(application.Name);

                            if (application.Name.ToUpper() == "APPLICATION")
                            {
                                foreach (XmlElement activity in application.ChildNodes)
                                {
                                    //Debug.Log(activity.Name);

                                    if (activity.Name.ToUpper() == "ACTIVITY")
                                    {
                                        foreach (XmlAttribute attribute in activity.Attributes)
                                        {
                                            if (attribute.Name.ToUpper() == "ANDROID:NAME")
                                            {
                                                attribute.Value = string.Format(".{0}", javaAppName);
                                                break; //only update the first activity
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                xDoc.Save(path);
            }
            else
            {
                Debug.LogError(string.Format("Unable to find android manifest: {0}", path));
            }
        }
        catch (System.Exception)
        {
        }
    }

    #endregion

    #region Unity Paths

    public const string KEY_PATH_UNITY_JAR = @"Unity Jar";
    public const string KEY_PATH_UNITY_EDITOR = @"Unity Editor";
    public const string KEY_PATH_UNITY_PROJECT = @"Unity Project";

    public const string PATH_UNITY_JAR_WIN = "Data/PlaybackEngines/androidplayer/release/bin/classes.jar";
    public const string PATH_UNITY_JAR_MAC = "Contents/PlaybackEngines/AndroidPlayer/release/bin/classes.jar";

    private static string pathUnityJar = string.Empty;
    private static string pathUnityEditor = string.Empty;
    private static string pathUnityProject = string.Empty;

    void UpdateUnityPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathUnityJar = string.Format("{0}/{1}", EditorApplication.applicationPath, PATH_UNITY_JAR_MAC);
                break;
            case RuntimePlatform.WindowsEditor:
                pathUnityJar = string.Format("{0}/{1}", pathUnityEditor, PATH_UNITY_JAR_WIN);
                break;
        }
    }

    #endregion

    #region Java JDK

    public const string KEY_PATH_JAVA_TOOLS_JAR = @"Tools Jar";
    public const string KEY_PATH_JAVA_JAR = @"Jar Path";
    public const string KEY_PATH_JAVA_JAVAC = @"JavaC Path";
    public const string KEY_PATH_JAVA_JAVAP = @"SDK Path";
    public const string KEY_PATH_JAVA_JDK = @"JDK Path";

    public const string REL_JAVA_PLATFORM_TOOLS = "bin";
    public const string FILE_JAR_WIN = "jar.exe";
    public const string FILE_JAR_MAC = "jar";
    public const string FILE_JAVAC_WIN = "javac.exe";
    public const string FILE_JAVAC_MAC = "javac";
    public const string FILE_JAVAP_WIN = "javap.exe";
    public const string FILE_JAVAP_MAC = "javap";

    public static string pathToolsJar = string.Empty;
    public static string pathJar = string.Empty;
    public static string pathJavaC = string.Empty;
    public static string pathJavaP = string.Empty;
    public static string pathJDK = string.Empty;

    void UpdateJavaJDKPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathToolsJar = string.Format("{0}/Contents/Classes/classes.jar", pathJDK);
                pathJar = string.Format("{0}/Contents/Commands/{1}", pathJDK, FILE_JAR_MAC);
                pathJavaC = string.Format("{0}/Contents/Commands/{1}", pathJDK, FILE_JAVAC_MAC);
                pathJavaP = string.Format("{0}/Contents/Commands/{1}", pathJDK, FILE_JAVAP_MAC);
                break;
            case RuntimePlatform.WindowsEditor:
                pathToolsJar = string.Format("{0}/lib/tools.jar", pathJDK);
                pathJar = string.Format("{0}/{1}/{2}", pathJDK, REL_JAVA_PLATFORM_TOOLS, FILE_JAR_WIN);
                pathJavaC = string.Format("{0}/{1}/{2}", pathJDK, REL_JAVA_PLATFORM_TOOLS, FILE_JAVAC_WIN);
                pathJavaP = string.Format("{0}/{1}/{2}", pathJDK, REL_JAVA_PLATFORM_TOOLS, FILE_JAVAP_WIN);
                break;
        }

        EditorPrefs.SetString(KEY_PATH_JAVA_JDK, pathJDK);
    }

    void ResetJavaJDKPaths()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                pathJDK = @"/System/Library/Java/JavaVirtualMachines/1.6.0.jdk";
                break;
            case RuntimePlatform.WindowsEditor:
                pathJDK = @"C:\Program Files (x86)/Java/jdk1.6.0_37";
                break;
        }

        UpdateJavaJDKPaths();
    }

    void SelectJavaJDKPaths()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_JAVA_JDK), pathJDK, "../jdk1.6.0_37");
                break;
            case RuntimePlatform.WindowsEditor:
                path = EditorUtility.OpenFolderPanel(string.Format("Path to {0}", KEY_PATH_JAVA_JDK), pathJDK, @"..\jdk1.6.0_37");
                break;
        }
        if (!string.IsNullOrEmpty(path))
        {
            pathJDK = path;
        }

        UpdateJavaJDKPaths();
    }

    #endregion

    [MenuItem("Window/Open Ouya Panel")]
    private static void MenuOpenPanel()
    {
        GetWindow<OuyaPanel>("Ouya Panel");
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey(KEY_JAVA_APP_NAME))
        {
            javaAppName = EditorPrefs.GetString(KEY_JAVA_APP_NAME);
        }

        if (EditorPrefs.HasKey(KEY_APK_NAME))
        {
            apkName = EditorPrefs.GetString(KEY_APK_NAME);
        }

        pathUnityEditor = new FileInfo(EditorApplication.applicationPath).Directory.FullName;
        pathUnityProject = new DirectoryInfo(Directory.GetCurrentDirectory()).FullName;
        UpdateUnityPaths();

        if (EditorPrefs.HasKey(KEY_PATH_ANDROID_SDK))
        {
            pathSDK = EditorPrefs.GetString(KEY_PATH_ANDROID_SDK);
        }

        if (string.IsNullOrEmpty(pathSDK))
        {
            ResetAndroidSDKPaths();
        }
        else
        {
            UpdateAndroidSDKPaths();
        }


        if (EditorPrefs.HasKey(KEY_PATH_ANDROID_NDK))
        {
            pathNDK = EditorPrefs.GetString(KEY_PATH_ANDROID_NDK);
        }
        if (string.IsNullOrEmpty(pathNDK))
        {
            ResetAndroidNDKPaths();
        }
        else
        {
            UpdateAndroidNDKPaths();
        }

        if (EditorPrefs.HasKey(KEY_PATH_ANDROID_NDK_MAKE))
        {
            pathNDKMake = EditorPrefs.GetString(KEY_PATH_ANDROID_NDK_MAKE);
        }
        if (string.IsNullOrEmpty(pathNDKMake))
        {
            ResetAndroidNDKMakePath();
        }
        else
        {
            UpdateAndroidNDKMakePath();
        }


        if (EditorPrefs.HasKey(KEY_PATH_JAVA_JDK))
        {
            pathJDK = EditorPrefs.GetString(KEY_PATH_JAVA_JDK);
        }

        if (string.IsNullOrEmpty(pathJDK))
        {
            ResetJavaJDKPaths();
        }
        else
        {
            UpdateJavaJDKPaths();
        }

        UpdateOuyaPaths();
    }

    void Update()
    {
        Repaint();

        if (!string.IsNullOrEmpty(m_nextScene))
        {
            EditorApplication.OpenScene(m_nextScene);
            m_nextScene = string.Empty;
            return;
        }

        var scenes = EditorBuildSettings.scenes;
        var sceneList = new List<string>();

        foreach (var scene in scenes)
        {
            if (scene.enabled)
                sceneList.Add(scene.path);
        }

        var sceneArray = sceneList.ToArray();

        if (m_toggleSyncBundleID)
        {
            m_toggleSyncBundleID = false;

            SyncBundleID();

            AssetDatabase.Refresh();
        }

        if (m_toggleCompileNDK)
        {
            m_toggleCompileNDK = false;

            CompileNDK();

            AssetDatabase.Refresh();
        }

        if (m_toggleCompilePlugin)
        {
            m_toggleCompilePlugin = false;

            OuyaMenuAdmin.MenuGeneratePluginJar();

            AssetDatabase.Refresh();
        }

        if (m_toggleCompileJava)
        {
            m_toggleCompileJava = false;

            if (CompileApplicationJava())
            {
                AssetDatabase.Refresh();
            }
        }

        if (m_toggleRunApplication)
        {
            m_toggleRunApplication = false;

            string appPath = string.Format("{0}/{1}", pathUnityProject, apkName);
            if (File.Exists(appPath))
            {
                //Debug.Log(appPath);
                //Debug.Log(pathADB);
                string args = string.Format("shell am start -n {0}/.{1}", PlayerSettings.bundleIdentifier, javaAppName);
                //Debug.Log(args);
                ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                Process p = new Process();
                p.StartInfo = ps;
                p.Exited += (object sender, EventArgs e) =>
                                {
                                    p.Dispose();
                                };
                p.Start();
            }
        }

        if (m_toggleBuildApplication)
        {
            m_toggleBuildApplication = false;

            if (GenerateRJava())
            {
                if (CompileApplicationClasses())
                {
                    if (BuildApplicationJar())
                    {
                        AssetDatabase.Refresh();

                        BuildPipeline.BuildPlayer(sceneArray, string.Format("{0}/{1}", pathUnityProject, apkName),
                                                  BuildTarget.Android, BuildOptions.None);
                    }
                }
            }
        }

        if (m_toggleBuildAndRunApplication)
        {
            m_toggleBuildAndRunApplication = false;

            AssetDatabase.Refresh();

            BuildOptions options = BuildOptions.AutoRunPlayer;
            if (EditorUserBuildSettings.allowDebugging)
            {
                options |= BuildOptions.Development | BuildOptions.AllowDebugging;
            }

            BuildPipeline.BuildPlayer(sceneArray, string.Format("{0}/{1}", pathUnityProject, apkName),
                                      BuildTarget.Android, options);
        }

        if (m_toggleBuildRunAndCompileApplication)
        {
            m_toggleBuildRunAndCompileApplication = false;

            if (GenerateRJava())
            {
                if (CompileApplicationClasses())
                {
                    if (BuildApplicationJar())
                    {
                        AssetDatabase.Refresh();

                        BuildOptions options = BuildOptions.AutoRunPlayer;
                        if (EditorUserBuildSettings.allowDebugging)
                        {
                            options |= BuildOptions.Development | BuildOptions.AllowDebugging;
                        }

                        BuildPipeline.BuildPlayer(sceneArray, string.Format("{0}/{1}", pathUnityProject, apkName),
                                                  BuildTarget.Android, options);
                    }
                }
            }
        }

        if (m_toggleOpenAndroidSDK)
        {
            m_toggleOpenAndroidSDK = false;

            string androidPath = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    androidPath = string.Format("{0}/tools/android", pathSDK);
                    break;

                case RuntimePlatform.WindowsEditor:
                    androidPath = string.Format("{0}/tools/android.bat", pathSDK);
                    break;
            }

            if (!string.IsNullOrEmpty(androidPath) &&
                File.Exists(androidPath))
            {
                //Debug.Log(androidPath);
                string args = "sdk";
                ProcessStartInfo ps = new ProcessStartInfo(androidPath, args);
                Process p = new Process();
                p.StartInfo = ps;
                p.Exited += (object sender, EventArgs e) =>
                                {
                                    p.Dispose();
                                };
                p.Start();
            }
        }
    }

    static bool GenerateRJava()
    {
        //clean meta files
        List<string> files = new List<string>();
        GetAssets("*.meta", files, new DirectoryInfo(pathRes.Replace(@"\", "/")));

        foreach (string meta in files)
        {
            //Debug.Log(meta);
            if (File.Exists(meta))
            {
                File.Delete(meta);
            }
        }

        if (!Directory.Exists(pathBin))
        {
            Directory.CreateDirectory(pathBin);
        }

        if (!Directory.Exists(pathBin))
        {
            Directory.CreateDirectory(pathBin);
        }

        if (Directory.Exists(pathBin))
        {
            Debug.Log(string.Format("Path exists: {0}", pathBin));
        }
        else
        {
            Debug.Log(string.Format("Path not exists: {0}", pathBin));
            return false;
        }

        Thread.Sleep(100);

        RunProcess(pathAAPT, string.Format("package -v -f -m -J gen -M \"{0}\" -S \"{1}\" -I \"{2}\" -F \"{3}/resources.ap_\" -J \"{4}\"",
            pathManifestPath, pathRes, GetPathAndroidJar(), pathBin, pathSrc));

        string pathRJava = GetRJava();
        if (string.IsNullOrEmpty(pathRJava))
        {
            Debug.LogError("Path to R.java is empty");
            return false;
        }
        if (File.Exists(pathRJava))
        {
            using (FileStream fs = File.Open(pathRJava, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Debug.Log(sr.ReadToEnd());
                }
            }
        }

        if (Directory.Exists(pathBin))
        {
            Directory.Delete(pathBin, true);
        }

        return true;
    }

    static bool CompileApplicationClasses()
    {
        string pathAndroid = string.Format("{0}/Assets/Plugins/Android", pathUnityProject);
        if (!Directory.Exists(pathAndroid))
        {
            Debug.LogError(string.Format("Failed to find Android Path: {0}", pathAndroid));
            return false;
        }

        string pathClasses = string.Format("{0}/Assets/Plugins/Android/Classes", pathUnityProject);
        if (!Directory.Exists(pathClasses))
        {
            Directory.CreateDirectory(pathClasses);
        }
        string pathRJava = GetRJava();
        if (string.IsNullOrEmpty(pathRJava))
        {
            Debug.LogError("Path to R.java is empty");
            return false;
        }
        if (!File.Exists(pathRJava))
        {
            Debug.LogError("R.java cannot be found");
            return false;
        }
        string includeFiles = string.Format("\"{0}/{1}.java\" \"{0}/IOuyaActivity.java\" \"{0}/UnityOuyaFacade.java\" \"{2}\"",
            pathSrc, javaAppName, pathRJava);
        string jars = string.Empty;

        if (File.Exists(pathToolsJar))
        {
            Debug.Log(string.Format("Found Java tools jar: {0}", pathToolsJar));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find Java tools jar: {0}", pathToolsJar));
            return false;
        }

        if (File.Exists(GetPathAndroidJar()))
        {
            Debug.Log(string.Format("Found Android jar: {0}", GetPathAndroidJar()));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find Android jar: {0}", GetPathAndroidJar()));
            return false;
        }

        if (File.Exists(pathGsonJar))
        {
            Debug.Log(string.Format("Found GJON jar: {0}", pathGsonJar));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find GSON jar: {0}", pathGsonJar));
            return false;
        }

        if (File.Exists(pathUnityJar))
        {
            Debug.Log(string.Format("Found Unity jar: {0}", pathUnityJar));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find Unity jar: {0}", pathUnityJar));
            return false;
        }

        if (File.Exists(pathOuyaUnityPluginJar))
        {
            Debug.Log(string.Format("Found Ouya Unity Plugin jar: {0}", pathOuyaUnityPluginJar));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find Ouya Unity Plugin jar: {0}", pathOuyaUnityPluginJar));
            return false;
        }

        if (File.Exists(pathOuyaSDKJar))
        {
            Debug.Log(string.Format("Found Ouya SDK jar: {0}", pathOuyaSDKJar));
        }
        else
        {
            Debug.LogError(string.Format("Failed to find Ouya SDK jar: {0}", pathOuyaSDKJar));
            return false;
        }

        string output = string.Empty;
        string error = string.Empty;

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                jars = string.Format("\"{0}:{1}:{2}:{3}:{4}:{5}\"",
                    pathToolsJar, GetPathAndroidJar(), pathGsonJar, pathUnityJar, pathOuyaUnityPluginJar, pathOuyaSDKJar);

                RunProcess(pathJavaC, string.Empty, string.Format("-g -source 1.6 -target 1.6 {0} -classpath {1} -bootclasspath {1} -d \"{2}\"",
                    includeFiles,
                    jars,
                    pathClasses),
                    ref output,
                    ref error);
                break;
            case RuntimePlatform.WindowsEditor:
                jars = string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\"",
                    pathToolsJar, GetPathAndroidJar(), pathGsonJar, pathUnityJar, pathOuyaUnityPluginJar, pathOuyaSDKJar);

                RunProcess(pathJavaC, string.Empty, string.Format("-Xlint:deprecation -g -source 1.6 -target 1.6 {0} -classpath {1} -bootclasspath {1} -d \"{2}\"",
                    includeFiles,
                    jars,
                    pathClasses),
                    ref output,
                    ref error);
                break;
        }

        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError(error);
            if (StopOnErrors)
            {
                return false;
            }
        }

        return true;
    }

    static bool BuildApplicationJar()
    {
        string pathClasses = string.Format("{0}/Assets/Plugins/Android/Classes", pathUnityProject);
        string bundlePrefix = GetBundlePrefix();
        if (string.IsNullOrEmpty(bundlePrefix))
        {
            Debug.LogError("Bundle prefix cannot be empty");
            return false;
        }

        //@hack: remove extra class file
        // tv/ouya/sdk/IOuyaActivity.class
        string extraClass = string.Format("{0}/tv/ouya/sdk/IOuyaActivity.class", pathClasses);
        if (File.Exists(extraClass))
        {
            File.Delete(extraClass);
            Debug.Log(string.Format("Removed: {0}", extraClass));
        }
        extraClass = string.Format("{0}/tv/ouya/sdk/IOuyaActivity.class.meta", pathClasses);
        if (File.Exists(extraClass))
        {
            File.Delete(extraClass);
            Debug.Log(string.Format("Removed: {0}", extraClass));
        }

        //@hack: remove extra class file
        // tv/ouya/sdk/UnityOuyaFacade.class
        extraClass = string.Format("{0}/tv/ouya/sdk/UnityOuyaFacade.class", pathClasses);
        if (File.Exists(extraClass))
        {
            File.Delete(extraClass);
            Debug.Log(string.Format("Removed: {0}", extraClass));
        }
        extraClass = string.Format("{0}/tv/ouya/sdk/UnityOuyaFacade.class.meta", pathClasses);
        if (File.Exists(extraClass))
        {
            File.Delete(extraClass);
            Debug.Log(string.Format("Removed: {0}", extraClass));
        }

        //@hack: remove extra folder
        // tv/ouya/sdk/*
        string extraFolder = string.Format("{0}/tv/ouya/sdk", pathClasses);
        if (Directory.Exists(extraFolder))
        {
            Directory.Delete(extraFolder, true);
            Debug.Log(string.Format("Removed: {0}", extraFolder));
        }
        string extraMeta = string.Format("{0}/tv/ouya/sdk.meta", pathClasses);
        if (File.Exists(extraMeta))
        {
            File.Delete(extraMeta);
            Debug.Log(string.Format("Removed: {0}", extraMeta));
        }

        RunProcess(pathJar, pathClasses, string.Format("cvfM OuyaUnityApplication.jar {0}/", bundlePrefix));
        OuyaPanel.RunProcess(pathJavaP, pathClasses, string.Format("-s {0}.{1}", PlayerSettings.bundleIdentifier, javaAppName));
        string pathAppJar = string.Format("{0}/OuyaUnityApplication.jar", pathClasses);
        string pathDest = string.Format("{0}/Assets/Plugins/Android/OuyaUnityApplication.jar", pathUnityProject);
        try
        {
            if (File.Exists(pathDest))
            {
                File.Delete(pathDest);
            }
            if (File.Exists(pathAppJar))
            {
                File.Move(pathAppJar, pathDest);
            }
            if (Directory.Exists(pathClasses))
            {
                Directory.Delete(pathClasses, true);
            }

            extraMeta = string.Format("{0}.meta", pathClasses);
            if (File.Exists(extraMeta))
            {
                File.Delete(extraMeta);
                Debug.Log(string.Format("Removed: {0}", extraMeta));
            }
        }
        catch (System.Exception)
        {

        }

        return true;
    }

    public static bool CompileApplicationJava()
    {
        if (GenerateRJava() &&
            CompileApplicationClasses() &&
            BuildApplicationJar())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GUIDisplayFolder(string label, string path)
    {
        bool dirExists = Directory.Exists(path);

        if (!dirExists)
        {
            GUI.enabled = false;
        }
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
        GUILayout.Space(25);
        GUILayout.Label(string.Format("{0}:", label), GUILayout.Width(100));
        GUILayout.Space(5);
        GUILayout.Label(path.Replace("/", @"\"), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
        GUILayout.EndHorizontal();
        if (!dirExists)
        {
            GUI.enabled = true;
        }
    }

    void GUIDisplayFile(string label, string path)
    {
        bool fileExists = File.Exists(path);

        if (!fileExists)
        {
            GUI.enabled = false;
        }
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
        GUILayout.Space(25);
        GUILayout.Label(string.Format("{0}:", label), GUILayout.Width(100));
        GUILayout.Space(5);
        GUILayout.Label(path.Replace("/", @"\"), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
        GUILayout.EndHorizontal();
        if (!fileExists)
        {
            GUI.enabled = true;
        }
    }

    void GUIDisplayUnityFile(string label, string path)
    {
        bool fileExists = File.Exists(path);

        if (!fileExists)
        {
            GUI.enabled = false;
        }
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
        GUILayout.Space(25);
        GUILayout.Label(string.Format("{0}:", label), GUILayout.Width(100));
        GUILayout.Space(5);
        if (string.IsNullOrEmpty(path))
        {
            EditorGUILayout.ObjectField(string.Empty, null, typeof(UnityEngine.Object), false);
        }
        else
        {
            try
            {
                DirectoryInfo assets = new DirectoryInfo("Assets");
                Uri assetsUri = new Uri(assets.FullName);
                FileInfo fi = new FileInfo(path);
                string relativePath = assetsUri.MakeRelativeUri(new Uri(fi.FullName)).ToString();
                UnityEngine.Object fileRef = AssetDatabase.LoadAssetAtPath(relativePath, typeof(UnityEngine.Object));
                EditorGUILayout.ObjectField(string.Empty, fileRef, typeof(UnityEngine.Object), false);
            }
            catch (System.Exception)
            {
                Debug.LogError(string.Format("Path is invalid: label={0} path={1}", label, path));
            }
        }
        GUILayout.EndHorizontal();
        if (!fileExists)
        {
            GUI.enabled = true;
        }
    }

    private string GetLicenseInfo()
    {
        string license = UnityEditorInternal.InternalEditorUtility.GetLicenseInfo();
        if (license.Contains("Serial number"))
        {
            int index = license.IndexOf("Serial number");
            if (index > 0)
            {
                return license.Substring(0, index);
            }
        }
        return license;
    }

    private void ShowIcons()
    {
        if (PlayerSettings.resolutionDialogBanner != null)
        {
            ShowImage("resolutionDialogBanner", PlayerSettings.resolutionDialogBanner);
        }
        if (PlayerSettings.xboxSplashScreen != null)
        {
            ShowImage("xboxSplashScreen", PlayerSettings.xboxSplashScreen);
        }
        // DEFAULT ICON IS UNKNOWN IMAGES
        DisplayImagesFor("Unknown Images", BuildTargetGroup.Unknown);

        DisplayImagesFor("Android Images", BuildTargetGroup.Android);
        DisplayImagesFor("WebPlayer Images", BuildTargetGroup.WebPlayer);
        DisplayImagesFor("iPhone Images", BuildTargetGroup.iPhone);
        DisplayImagesFor("Standalone Images", BuildTargetGroup.Standalone);
    }

    private void DisplayImagesFor(string title, BuildTargetGroup target)
    {
        var found = false;
        Texture2D[] textures = PlayerSettings.GetIconsForTargetGroup(target);
        int[] textureSizes = PlayerSettings.GetIconSizesForTargetGroup(target);
        for (var i = 0; i < textureSizes.Length; i++)
        {
            var texture2D = textures[i];
            if (texture2D == null) continue;
            if (!found)
            {
                EditorGUILayout.LabelField(title);
                found = true;
            }

            EditorGUILayout.LabelField(string.Format("[{1}] - {0}",texture2D.ToString(), textureSizes[i]));
            new GUIContent(texture2D);
            Rect pos = GUILayoutUtility.GetRect(textureSizes[i], textureSizes[i], EditorStyles.miniButton, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(pos, texture2D);
        }
    }

    private void ShowImage(string title, Texture2D image)
    {
        EditorGUILayout.LabelField(title);
        if (image != null)
        {
            GUIContent content = new GUIContent(image);
            Rect pos = GUILayoutUtility.GetRect(content, EditorStyles.miniButton, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(pos, image);
        }
    }

    // load the scene in the update method
    private string m_nextScene = string.Empty;

    private void SwitchToExampleScene(string sceneName)
    {
        EditorBuildSettingsScene[] scenes =
            {
                new EditorBuildSettingsScene(string.Format("Assets/Ouya/Examples/Scenes/{0}.unity", sceneName), true),
            };

        File.Copy(string.Format("Assets/Ouya/Examples/Icons/{0}/app_icon.png", sceneName),
            "Assets/Plugins/Android/res/drawable/app_icon.png",
            true);

        File.Copy(string.Format("Assets/Ouya/Examples/Icons/{0}/ouya_icon.png", sceneName),
            @"Assets/Plugins/Android/res/drawable-xhdpi/ouya_icon.png",
            true);

        SetupProductBundleAndCompile(scenes, sceneName);
    }

    private void SwitchToStarterKitScene(string[] sceneNames, string productName)
    {
        List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>();

        foreach (string sceneName in sceneNames)
        {
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(sceneName, true);
            sceneList.Add(scene);
        }

        File.Copy("Assets/Ouya/StarterKit/Icons/app_icon.png",
            "Assets/Plugins/Android/res/drawable/app_icon.png",
            true);

        File.Copy("Assets/Ouya/StarterKit/Icons/ouya_icon.png",
            @"Assets/Plugins/Android/res/drawable-xhdpi/ouya_icon.png",
            true);

        EditorBuildSettingsScene[] scenes = sceneList.ToArray();
        SetupProductBundleAndCompile(scenes, productName);
    }

    private void SetupProductBundleAndCompile(EditorBuildSettingsScene[] scenes, string productName)
    {
        EditorBuildSettings.scenes = scenes;
        m_nextScene = scenes[0].path;

        apkName = string.Format ("{0}.apk", productName);
        EditorPrefs.SetString(KEY_APK_NAME, apkName);

        PlayerSettings.bundleIdentifier = string.Format("tv.ouya.demo.{0}", productName);
        PlayerSettings.productName = productName;

        m_toggleSyncBundleID = true;
        m_toggleCompileNDK = true;
        m_toggleCompilePlugin = true;
        m_toggleCompileJava = true;
    }

    private Vector2 m_scroll = Vector2.zero;

    private static int m_selectedExample = 0;

    private static string[] m_exampleScenes =
        {
            "Starter Kit Scenes",
            "SceneShowCamera",
            "SceneShowDrumkit",
            "SceneShowGuitar",
            "SceneShowJavaScript",
            "SceneSetResolution",
            "SceneShowMeshPerformance",
            "SceneShowNDK",
            "SceneShowProducts",
            "SceneShowSounds",
            "SceneShowSticks",
            "SceneShowUnityInput",
        };

    private static int m_selectedAdbMode = 0;

    private static string[] m_abdModes =
        {
            "wired",
            "wireless",
        };

    void OnGUI()
    {
        m_scroll = GUILayout.BeginScrollView(m_scroll, GUILayout.MaxWidth(position.width));

        GUILayout.Label(string.Format("{0} UID: {1}", OuyaSDK.VERSION, UID));

        m_selectedToolSet = GUILayout.Toolbar(m_selectedToolSet, m_toolSets, GUILayout.MaxWidth(position.width));

        GUILayout.Space(20);

        switch (m_selectedToolSet)
        {
            case 0:

                if (GUILayout.Button("Run Application", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleRunApplication = true;
                }

                if (GUILayout.Button("Build Application", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleCompileJava = true;
                    m_toggleCompilePlugin = true;
                    m_toggleCompileNDK = true;
                    m_toggleBuildApplication = true;
                }

                if (GUILayout.Button("Build and Run Application", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleBuildAndRunApplication = true;
                }

                if (GUILayout.Button("Build, Run, and Compile Application", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleBuildRunAndCompileApplication = true;
                }

                if (GUILayout.Button("Compile", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleCompileJava = true;
                    m_toggleCompileNDK = true;
                }

                if (GUILayout.Button("Compile Java", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleCompileJava = true;
                }

                if (GUILayout.Button("Compile Plugin", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleCompilePlugin = true;
                }

                if (GUILayout.Button("Compile NDK", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleCompileNDK = true;
                }

                /*
                if (GUILayout.Button("Build Application Jar", GUILayout.MaxWidth(position.width)))
                {
                    BuildApplicationJar();
                }

                if (GUILayout.Button("Compile Application Classes", GUILayout.MaxWidth(position.width)))
                {
                    CompileApplicationClasses();
                }

                if (GUILayout.Button("Generate R.java from main layout", GUILayout.MaxWidth(position.width)))
                {
                    GenerateRJava();
                }
                */

                UsePostProcessor = GUILayout.Toggle(UsePostProcessor, "Enable Java/C++ post processor");

                GUILayout.Space(5);

                GUILayout.Label("By default any Java compiler error will stop building");
                StopOnErrors = GUILayout.Toggle(StopOnErrors, "Stop Build on Errors");

                GUILayout.Space(5);
                if (GUILayout.Button("Sync Bundle ID", GUILayout.MaxWidth(position.width)))
                {
                    m_toggleSyncBundleID = true;
                }
                GUILayout.Space(5);

                #region Example scenes

                GUILayout.Label("Build Settings:");

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width - 35));
                m_selectedExample = EditorGUILayout.Popup(m_selectedExample, m_exampleScenes, GUILayout.MaxWidth(position.width));
                if (GUILayout.Button("Switch to Example"))
                {
                    if (m_selectedExample > 0)
                    {
                        SwitchToExampleScene(m_exampleScenes[m_selectedExample]);
                    }
                    else if (m_selectedExample == 0)
                    {
                        string[] newScenes =
                        {
                            "Assets/Ouya/StarterKit/Scenes/SceneInit.unity",
                            "Assets/Ouya/StarterKit/Scenes/SceneSplash.unity",
                            "Assets/Ouya/StarterKit/Scenes/SceneMain.unity",
                            "Assets/Ouya/StarterKit/Scenes/SceneGame.unity",
                        };

                        SwitchToStarterKitScene(newScenes, "StarterKit");
                    }
                }
                GUILayout.EndHorizontal();

                #endregion

                GUILayout.Label("OUYA", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("License:", GUILayout.Width(100));
                GUILayout.Label(GetLicenseInfo(), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                //ShowIcons();

                // show splash screen settings

                string error = string.Empty;

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("Product Name", GUILayout.Width(100));
                GUILayout.Space(5);
                PlayerSettings.productName = GUILayout.TextField(PlayerSettings.productName, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                if ((PlayerSettings.bundleIdentifier.Contains(" ") ||
                    PlayerSettings.bundleIdentifier.Contains("\t") ||
                    PlayerSettings.bundleIdentifier.Contains("\r") ||
                    PlayerSettings.bundleIdentifier.Contains("\n") ||
                    PlayerSettings.bundleIdentifier.Contains("(") ||
                    PlayerSettings.bundleIdentifier.Contains(")")))
                {
                    String fieldError = "[error] (bundle id has an invalid character)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                }
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("Bundle Identifier", GUILayout.Width(100));
                GUILayout.Space(5);
                PlayerSettings.bundleIdentifier = GUILayout.TextField(PlayerSettings.bundleIdentifier, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("APK Name:", GUILayout.Width(100));
                GUILayout.Space(5);
                string newApkName = GUILayout.TextField(apkName, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();
                if (apkName != newApkName)
                {
                    apkName = newApkName;
                    EditorPrefs.SetString(KEY_APK_NAME, apkName);
                }

#if UNITY_4 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
                if (javaAppName.ToUpper().Equals("OUYAUNITYAPPLICATION"))
                {
                    String fieldError = "[error] (OuyaNativeActivity should be used as the 'Main Activity' in Unity 4.X to let Unity handle native input)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width));
                }
#else
                if (javaAppName.ToUpper().Equals("OUYANATIVEACTIVITY"))
                {
                    String fieldError = "[error] (OuyaUnityApplication should be used as the 'Main Activity' in Unity 3.X to avoid a crash within the Unity player input handling)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                }
#endif
                else if (javaAppName.ToUpper().Equals("IOUYAACTIVITY") ||
                    javaAppName.ToUpper().Equals("OUYAUNITYPLUGIN") ||
                    javaAppName.ToUpper().Equals("UNITYOUYAFACADE"))
                {
                    String fieldError = "[error] (Used reserved Java Class Name)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                }
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("Main Activity:", GUILayout.Width(100));
                GUILayout.Space(5);
                string newJavaAppName = GUILayout.TextField(javaAppName, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();
                if (javaAppName != newJavaAppName)
                {
                    javaAppName = newJavaAppName;
                    EditorPrefs.SetString(KEY_JAVA_APP_NAME, javaAppName);
                }

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label(string.Format("{0}:", "Bundle Prefix"), GUILayout.Width(100));
                GUILayout.Space(5);
                GUILayout.Label(GetBundlePrefix(), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                string javaPackageName = GetApplicationJavaPackageName();
                if (!javaPackageName.Equals(string.Format("package {0};", PlayerSettings.bundleIdentifier)))
                {
                    String fieldError = "[error] (bundle mismatched)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                }

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("App Java Pack:", GUILayout.Width(100));
                GUILayout.Space(5);
                GUILayout.Label(string.Format("{0}{1}", error, GetApplicationJavaPackageName()), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                string manifestPackageName = GetAndroidManifestPackageName();
                if (!manifestPackageName.Equals(PlayerSettings.bundleIdentifier))
                {
                    String fieldError = "[error] (bundle mismatched)\n";
                    if (string.IsNullOrEmpty(error))
                    {
                        ShowNotification(new GUIContent(fieldError));
                        error = fieldError;
                    }
                    EditorGUILayout.Separator();
                    GUILayout.Label(fieldError, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                }
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label("Manifest Pack:", GUILayout.Width(100));
                GUILayout.Space(5);
                GUILayout.Label(string.Format("{0}{1}", error, GetAndroidManifestPackageName()), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                GameObject go = GameObject.Find("OuyaGameObject");
                OuyaGameObject ouyaGO = null;
                if (go)
                {
                    ouyaGO = go.GetComponent<OuyaGameObject>();
                }
                if (null == ouyaGO)
                {
                    GUI.enabled = false;
                }
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label(string.Format("{0}:", "GameObject"), GUILayout.Width(100));
                GUILayout.Space(5);
                EditorGUILayout.ObjectField(string.Empty, ouyaGO, typeof(OuyaGameObject), true);
                GUILayout.EndHorizontal();
                if (null == ouyaGO)
                {
                    GUI.enabled = true;
                }

                GUIDisplayUnityFile(KEY_PATH_OUYA_SDK, pathOuyaSDKJar);
                GUIDisplayUnityFile(KEY_PATH_JAR_GUAVA, pathGsonJar);
                GUIDisplayUnityFile(KEY_PATH_JAR_GSON, pathGuavaJar);
                GUIDisplayUnityFile(KEY_PATH_JAR_OUYA_UNITY_PLUGIN, pathOuyaUnityPluginJar);
                GUIDisplayUnityFile("Manifest", pathManifestPath);
                GUIDisplayUnityFile("key.der", "Assets/Plugins/Android/res/raw/key.der");
                GUIDisplayUnityFile("R.Java", GetRJava());
                GUIDisplayUnityFile("Activity.Java", GetApplicationJava());
                GUIDisplayUnityFile("IOuyaActivity.Java", GetIOuyaActivityJava());
                //GUIDisplayFolder("Bin", pathBin);
                GUIDisplayFolder("Res", pathRes);
                GUIDisplayFolder("Src", pathSrc);

                if (GUILayout.Button("Check for plugin updates"))
                {
                    Application.OpenURL("http://github.com/ouya/ouya-unity-plugin");
                }

                if (GUILayout.Button("Visit Unity3d on OUYA Forum"))
                {
                    Application.OpenURL("http://forums.ouya.tv/categories/unity-on-ouya");
                }

                if (GUILayout.Button("Read OUYA Unity Docs"))
                {
                    Application.OpenURL("https://devs.ouya.tv/developers/docs/unity");
                }

                if (GUILayout.Button("OUYA Developer Portal"))
                {
                    Application.OpenURL("https://devs.ouya.tv/developers");
                }

                break;
            case 1:
                GUILayout.Label("Unity Paths", EditorStyles.boldLabel);

                GUIDisplayFile(KEY_PATH_UNITY_JAR, pathUnityJar);
                GUIDisplayFolder(KEY_PATH_UNITY_EDITOR, pathUnityEditor);
                GUIDisplayFolder(KEY_PATH_UNITY_PROJECT, pathUnityProject);

                if (GUILayout.Button("Download Unity3d"))
                {
                    Application.OpenURL("http://unity3d.com/unity/download/");
                }

                if (GUILayout.Button("Unity3d Training"))
                {
                    Application.OpenURL("http://unity3d.com/learn");
                }

                if (GUILayout.Button("Unity3d Scripting Reference"))
                {
                    Application.OpenURL("http://docs.unity3d.com/Documentation/ScriptReference/index.html");
                }

                break;
            case 2:
                GUILayout.Label("Java JDK Paths", EditorStyles.boldLabel);

                GUIDisplayFile(KEY_PATH_JAVA_TOOLS_JAR, pathToolsJar);
                GUIDisplayFile(KEY_PATH_JAVA_JAR, pathJar);
                GUIDisplayFile(KEY_PATH_JAVA_JAVAC, pathJavaC);
                GUIDisplayFile(KEY_PATH_JAVA_JAVAP, pathJavaP);
                GUIDisplayFolder(KEY_PATH_JAVA_JDK, pathJDK);

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                if (GUILayout.Button("Select SDK Path..."))
                {
                    SelectJavaJDKPaths();
                }
                if (GUILayout.Button("Reset Paths"))
                {
                    ResetJavaJDKPaths();
                }

                GUILayout.EndHorizontal();

                GUILayout.Label("Java JDK Download Links (32bit)", EditorStyles.boldLabel);

                if (GUILayout.Button("Windows XP, Vista, 7"))
                {
                    Application.OpenURL("http://www.oracle.com/technetwork/java/javasebusiness/downloads/java-archive-downloads-javase6-419409.html#jdk-6u45-oth-JPR");
                }

                if (GUILayout.Button("Windows 8"))
                {
                    Application.OpenURL("http://www.oracle.com/technetwork/java/javase/downloads/jdk7-downloads-1880260.html");
                }

                if (GUILayout.Button("OSX"))
                {
                    Application.OpenURL("http://support.apple.com/kb/DL1572?viewlocale=en_US");
                }

                break;
            case 3:
                GUILayout.Label("Android SDK", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                GUILayout.Space(25);
                GUILayout.Label(string.Format("{0}:", "minSDKVersion"), GUILayout.Width(100));
                GUILayout.Space(5);
                GUILayout.Label(((int)(PlayerSettings.Android.minSdkVersion)).ToString(), EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(position.width - 130));
                GUILayout.EndHorizontal();

                GUIDisplayFile(KEY_PATH_ANDROID_JAR, GetPathAndroidJar());
                GUIDisplayFile(KEY_PATH_ANDROID_ADB, pathADB);
                GUIDisplayFile(KEY_PATH_ANDROID_AAPT, pathAAPT);
                GUIDisplayFolder(KEY_PATH_ANDROID_SDK, pathSDK);

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                if (GUILayout.Button("Select SDK Path..."))
                {
                    SelectAndroidSDKPaths();
                }
                if (GUILayout.Button("Reset Paths"))
                {
                    ResetAndroidSDKPaths();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Download Android SDK"))
                {
                    Application.OpenURL("http://developer.android.com/sdk/index.html");
                }

                if (GUILayout.Button("Open Android SDK"))
                {
                    m_toggleOpenAndroidSDK = true;
                }

                if (GUILayout.Button("Open Shell"))
                {
                    string shellPath = @"c:\windows\system32\cmd.exe";
                    if (File.Exists(shellPath))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        string args = string.Format(@"/k");
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(shellPath, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                                        {
                                            p.Dispose();
                                        };
                        p.Start();
                    }
                    EditorGUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Advanced Settings"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        if (File.Exists(pathADB))
                        {
                            //Debug.Log(appPath);
                            //Debug.Log(pathADB);
                            string args =
                                string.Format(
                                    @"shell su -c am start com.android.settings");
                            //Debug.Log(args);
                            ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                       args);
                            Process p = new Process();
                            ps.RedirectStandardOutput = false;
                            ps.UseShellExecute = true;
                            ps.CreateNoWindow = false;
                            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                            p.StartInfo = ps;
                            p.Exited += (object sender, EventArgs e) =>
                            {
                                p.Dispose();
                            };
                            p.Start();

                            p.WaitForExit();
                        }
                    });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Ethernet Mode"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        if (File.Exists(pathADB))
                        {
                            //Debug.Log(appPath);
                            //Debug.Log(pathADB);
                            string args =
                                string.Format(
                                    @"shell su -c ifconfig eth0 up");
                            //Debug.Log(args);
                            ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                       args);
                            Process p = new Process();
                            ps.RedirectStandardOutput = false;
                            ps.UseShellExecute = true;
                            ps.CreateNoWindow = false;
                            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                            p.StartInfo = ps;
                            p.Exited += (object sender, EventArgs e) =>
                            {
                                p.Dispose();
                            };
                            p.Start();

                            p.WaitForExit();
                        }
                    });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Wifi Mode"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        if (File.Exists(pathADB))
                        {
                            //Debug.Log(appPath);
                            //Debug.Log(pathADB);
                            string args =
                                string.Format(
                                    @"shell su -c ifconfig eth0 down");
                            //Debug.Log(args);
                            ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                       args);
                            Process p = new Process();
                            ps.RedirectStandardOutput = false;
                            ps.UseShellExecute = true;
                            ps.CreateNoWindow = false;
                            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                            p.StartInfo = ps;
                            p.Exited += (object sender, EventArgs e) =>
                            {
                                p.Dispose();
                            };
                            p.Start();

                            p.WaitForExit();
                        }
                    });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Wifi Settings"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        if (File.Exists(pathADB))
                        {
                            //Debug.Log(appPath);
                            //Debug.Log(pathADB);
                            string args =
                                string.Format(
                                    @"shell su -c am start -n com.android.settings/com.android.settings.wifi.WifiSettings");
                            //Debug.Log(args);
                            ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                       args);
                            Process p = new Process();
                            ps.RedirectStandardOutput = false;
                            ps.UseShellExecute = true;
                            ps.CreateNoWindow = false;
                            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                            p.StartInfo = ps;
                            p.Exited += (object sender, EventArgs e) =>
                            {
                                p.Dispose();
                            };
                            p.Start();

                            p.WaitForExit();
                        }
                    });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Toggle Safe Overlay"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        if (File.Exists(pathADB))
                        {
                            //Debug.Log(appPath);
                            //Debug.Log(pathADB);
                            string args =
                                string.Format(
                                    @"shell su -c am start -n tv.ouya.console/tv.ouya.console.launcher.ToggleSafeZoneActivity");
                            //Debug.Log(args);
                            ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                       args);
                            Process p = new Process();
                            ps.RedirectStandardOutput = false;
                            ps.UseShellExecute = true;
                            ps.CreateNoWindow = false;
                            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                            p.StartInfo = ps;
                            p.Exited += (object sender, EventArgs e) =>
                            {
                                p.Dispose();
                            };
                            p.Start();

                            p.WaitForExit();
                        }
                    });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Take Screenshot"))
                {
                    ThreadStart ts = new ThreadStart(() =>
                                                         {
                                                             if (File.Exists(pathADB))
                                                             {
                                                                 //Debug.Log(appPath);
                                                                 //Debug.Log(pathADB);
                                                                 string args =
                                                                     string.Format(
                                                                         @"shell /system/bin/screencap -p /sdcard/screenshot.png");
                                                                 //Debug.Log(args);
                                                                 ProcessStartInfo ps = new ProcessStartInfo(pathADB,
                                                                                                            args);
                                                                 Process p = new Process();
                                                                 ps.RedirectStandardOutput = false;
                                                                 ps.UseShellExecute = true;
                                                                 ps.CreateNoWindow = false;
                                                                 ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                                                                 p.StartInfo = ps;
                                                                 p.Exited += (object sender, EventArgs e) =>
                                                                                 {
                                                                                     p.Dispose();
                                                                                 };
                                                                 p.Start();

                                                                 p.WaitForExit();


                                                                 string args2 =
                                                                     string.Format(
                                                                         @"pull /sdcard/screenshot.png screenshot.png");
                                                                 //Debug.Log(args2);
                                                                 ProcessStartInfo ps2 = new ProcessStartInfo(pathADB,
                                                                                                             args2);
                                                                 Process p2 = new Process();
                                                                 ps2.RedirectStandardOutput = false;
                                                                 ps2.UseShellExecute = true;
                                                                 ps2.CreateNoWindow = false;
                                                                 ps2.WorkingDirectory = Path.GetDirectoryName(pathADB);
                                                                 p2.StartInfo = ps2;
                                                                 p2.Exited += (object sender, EventArgs e) =>
                                                                                  {
                                                                                      p2.Dispose();
                                                                                  };
                                                                 p2.Start();

                                                                 p2.WaitForExit();

                                                                 string shellPath = @"c:\windows\system32\cmd.exe";
                                                                 if (File.Exists(shellPath))
                                                                 {
                                                                     //Debug.Log(appPath);
                                                                     //Debug.Log(pathADB);
                                                                     string args3 =
                                                                         string.Format(@"/c start screenshot.png");
                                                                     //Debug.Log(args3);
                                                                     ProcessStartInfo ps3 =
                                                                         new ProcessStartInfo(shellPath, args3);
                                                                     Process p3 = new Process();
                                                                     ps3.RedirectStandardOutput = false;
                                                                     ps3.UseShellExecute = true;
                                                                     ps3.CreateNoWindow = false;
                                                                     ps3.WorkingDirectory =
                                                                         Path.GetDirectoryName(pathADB);
                                                                     p3.StartInfo = ps3;
                                                                     p3.Exited += (object sender, EventArgs e) =>
                                                                                      {
                                                                                          p3.Dispose();
                                                                                      };
                                                                     p3.Start();
                                                                     //p.WaitForExit();
                                                                 }
                                                             }
                                                         });
                    Thread thread = new Thread(ts);
                    thread.Start();
                    EditorGUIUtility.ExitGUI();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Logcat");
                if (GUILayout.Button("Open"))
                {
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        string args = string.Format(@"shell logcat");
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Clear"))
                {
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        string args = string.Format(@"shell logcat -c");
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Language:");
                Languages newLang = (Languages)EditorGUILayout.EnumPopup(m_language);
                if (newLang != m_language)
                {
                    m_language = newLang;
                    //set language on console
                    SetLanguage();
                    Reboot();
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Reboot Console"))
                {
                    Reboot();
                    EditorGUIUtility.ExitGUI();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("ADB Mode:");
                int adbMode = EditorGUILayout.Popup(m_selectedAdbMode, m_abdModes, GUILayout.MaxWidth(position.width));
                if (adbMode != m_selectedAdbMode)
                {
                    m_selectedAdbMode = adbMode;
                    string args;
                    //wired
                    if (m_selectedAdbMode == 0)
                    {
                        args = "usb";
                    }
                    //wireless
                    else
                    {
                        args = "tcpip 5555";
                    }

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                //wireless
                if (m_selectedAdbMode == 1)
                {
                    GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width - 25));
                    GUILayout.Label("Wireless IP:", GUILayout.MaxWidth(100));
                    string adbIpAddress = GUILayout.TextField(AdbIpAddress);
                    if (!adbIpAddress.Equals(AdbIpAddress))
                    {
                        AdbIpAddress = adbIpAddress;
                        EditorGUIUtility.ExitGUI();
                    }
                    if (!string.IsNullOrEmpty(adbIpAddress))
                    {
                        if (GUILayout.Button("Connect", GUILayout.MaxWidth(100)))
                        {
                            string args = string.Format("connect {0}", AdbIpAddress);

                            if (File.Exists(pathADB))
                            {
                                //Debug.Log(appPath);
                                //Debug.Log(pathADB);
                                //Debug.Log(args);
                                ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                                Process p = new Process();
                                ps.RedirectStandardOutput = false;
                                ps.UseShellExecute = true;
                                ps.CreateNoWindow = false;
                                ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                                p.StartInfo = ps;
                                p.Exited += (object sender, EventArgs e) =>
                                                {
                                                    p.Dispose();
                                                };
                                p.Start();
                                //p.WaitForExit();
                            }
                            EditorGUIUtility.ExitGUI();
                        }

                        if (GUILayout.Button("Disconnect", GUILayout.MaxWidth(100)))
                        {
                            string args = string.Format("disconnect {0}", AdbIpAddress);

                            if (File.Exists(pathADB))
                            {
                                //Debug.Log(appPath);
                                //Debug.Log(pathADB);
                                //Debug.Log(args);
                                ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                                Process p = new Process();
                                ps.RedirectStandardOutput = false;
                                ps.UseShellExecute = true;
                                ps.CreateNoWindow = false;
                                ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                                p.StartInfo = ps;
                                p.Exited += (object sender, EventArgs e) =>
                                {
                                    p.Dispose();
                                };
                                p.Start();
                                //p.WaitForExit();
                            }
                            EditorGUIUtility.ExitGUI();
                        }
                    }
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Volume:");
                if (GUILayout.Button("Down"))
                {
                    string args = string.Format("shell input keyevent VOLUME_DOWN");

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Up"))
                {
                    string args = string.Format("shell input keyevent VOLUME_UP");

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("Browser:");
                GUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if (GUILayout.Button("Explorer"))
                {
                    string args = string.Format("shell input keyevent EXPLORER");
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Search"))
                {
                    string args = string.Format("shell input keyevent SEARCH");

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Back"))
                {
                    string args = string.Format("shell input keyevent BACK");

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Home"))
                {
                    string args = string.Format("shell input keyevent HOME");

                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Space(25);
                m_browserUrl = GUILayout.TextField(m_browserUrl, GUILayout.MinWidth(100));
                if (GUILayout.Button("Go"))
                {
                    string args = string.Format("shell input keyevent EXPLORER");
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    Thread.Sleep(1000);
                    args = string.Format("shell input keyevent SEARCH");
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    Thread.Sleep(500);
                    args = string.Format("shell input text {0}", m_browserUrl);
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    Thread.Sleep(4000);
                    args = "shell input keyevent ENTER";
                    if (File.Exists(pathADB))
                    {
                        //Debug.Log(appPath);
                        //Debug.Log(pathADB);
                        //Debug.Log(args);
                        ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
                        Process p = new Process();
                        ps.RedirectStandardOutput = false;
                        ps.UseShellExecute = true;
                        ps.CreateNoWindow = false;
                        ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
                        p.StartInfo = ps;
                        p.Exited += (object sender, EventArgs e) =>
                        {
                            p.Dispose();
                        };
                        p.Start();
                        //p.WaitForExit();
                    }
                    EditorGUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                break;

            case 4:
                GUILayout.Label("Android NDK", EditorStyles.boldLabel);

                GUIDisplayFolder(KEY_PATH_ANDROID_NDK, pathNDK);
                GUI.enabled = !string.IsNullOrEmpty(pathNDK);
                GUIDisplayFile(KEY_PATH_ANDROID_NDK_MAKE, pathNDKMake);
                GUI.enabled = true;
                GUIDisplayUnityFile(KEY_PATH_ANDROID_JNI_CPP, pathJNICpp);
                GUIDisplayUnityFile(KEY_PATH_ANDROID_JNI_MK, pathJNIMk);
                GUIDisplayUnityFile(KEY_PATH_OUYA_NDK_LIB, pathOuyaNDKLib);

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                if (GUILayout.Button("Select NDK Path..."))
                {
                    SelectAndroidNDKPaths();
                }
                if (GUILayout.Button("Reset Paths"))
                {
                    ResetAndroidNDKPaths();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
                if (GUILayout.Button("Select NDK Make Path..."))
                {
                    SelectAndroidNDKMakePath();
                }
                if (GUILayout.Button("Reset Make Path"))
                {
                    ResetAndroidNDKMakePath();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Download Android NDK"))
                {
                    Application.OpenURL("http://developer.android.com/tools/sdk/ndk/index.html");
                }

                break;
        }

        GUILayout.EndScrollView();
    }

    #region Language

    enum Languages
    {
        EnglishUnitedStates,
        EnglishUnitedAustralia,
        EnglishUnitedCanada,
        EnglishUnitedKingdom,
        German,
        Spanish,
        Korean,
        China,
        Taiwan,
        Japan,
    }

    struct LanguageDetails
    {
        public Languages Language;
        public string PropertySystemLanguage;
        public string PropertySystemCountry;
    }

    private static LanguageDetails[] LanguageMap =
        {
            new LanguageDetails() { Language = Languages.EnglishUnitedStates, PropertySystemLanguage="en", PropertySystemCountry="US"},
            new LanguageDetails() { Language = Languages.EnglishUnitedAustralia, PropertySystemLanguage="en", PropertySystemCountry="AU"},
            new LanguageDetails() { Language = Languages.EnglishUnitedCanada, PropertySystemLanguage="en", PropertySystemCountry="CA"},
            new LanguageDetails() { Language = Languages.EnglishUnitedKingdom, PropertySystemLanguage="en", PropertySystemCountry="GB"},
            new LanguageDetails() { Language = Languages.German, PropertySystemLanguage="de", PropertySystemCountry="de"},
            new LanguageDetails() { Language = Languages.Spanish, PropertySystemLanguage="es", PropertySystemCountry="ES"},
            new LanguageDetails() { Language = Languages.Korean, PropertySystemLanguage="ko", PropertySystemCountry="KR"},
            new LanguageDetails() { Language = Languages.China, PropertySystemLanguage="zh", PropertySystemCountry="CN"},
            new LanguageDetails() { Language = Languages.Taiwan, PropertySystemLanguage="zh", PropertySystemCountry="TW"},
            new LanguageDetails() { Language = Languages.Japan, PropertySystemLanguage="ja", PropertySystemCountry="JP"},
        };

    private Languages m_language = Languages.EnglishUnitedStates;

    private string GetPropertySystemLanguage()
    {
        foreach (LanguageDetails details in LanguageMap)
        {
            if (details.Language == m_language)
            {
                return details.PropertySystemLanguage;
            }
        }
        return string.Empty;
    }

    private string GetPropertySystemCountry()
    {
        foreach (LanguageDetails details in LanguageMap)
        {
            if (details.Language == m_language)
            {
                return details.PropertySystemCountry;
            }
        }
        return string.Empty;
    }

    private void SetLanguage()
    {
        if (File.Exists(pathADB))
        {
            //Debug.Log(appPath);
            //Debug.Log(pathADB);
            string args = "shell";
            //Debug.Log(args);
            ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
            Process p = new Process();
            ps.RedirectStandardOutput = false;
            ps.RedirectStandardInput = true;
            ps.UseShellExecute = false;
            ps.CreateNoWindow = false;
            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
            p.StartInfo = ps;
            p.Exited += (object sender, EventArgs e) =>
            {
                p.Dispose();
            };
            p.Start();

            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("su");
            p.StandardInput.WriteLine("setprop persist.sys.language {0}; setprop persist.sys.country {1}; stop; sleep 1; start;",
                GetPropertySystemLanguage(),
                GetPropertySystemCountry());
            p.StandardInput.WriteLine("exit");
            p.StandardInput.WriteLine("exit");
            p.WaitForExit(1);
            p.Close();

            Thread.Sleep(1000);
        }
    }

    #endregion

    #region Reboot

    private void Reboot()
    {
        if (File.Exists(pathADB))
        {
            //Debug.Log(appPath);
            //Debug.Log(pathADB);
            string args = string.Format(@"reboot");
            //Debug.Log(args);
            ProcessStartInfo ps = new ProcessStartInfo(pathADB, args);
            Process p = new Process();
            ps.RedirectStandardOutput = false;
            ps.UseShellExecute = true;
            ps.CreateNoWindow = false;
            ps.WorkingDirectory = Path.GetDirectoryName(pathADB);
            p.StartInfo = ps;
            p.Exited += (object sender, EventArgs e) =>
            {
                p.Dispose();
            };
            p.Start();
            //p.WaitForExit();
        }
    }

    #endregion

    #region RUN PROCESS
    public static void RunProcess(string path, string arguments)
    {
        string error = string.Empty;
        string output = string.Empty;
        RunProcess(path, string.Empty, arguments, ref output, ref error);
    }

    public static void RunProcess(string path, string workingDirectory, string arguments)
    {
        string error = string.Empty;
        string output = string.Empty;
        RunProcess(path, workingDirectory, arguments, ref output, ref error);
    }

    public static void RunProcess(string path, string workingDirectory, string arguments, ref string output, ref string error)
    {
        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.FileName = path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.ErrorDialog = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            DateTime startTime = DateTime.Now;
            Debug.Log(string.Format("[Running Process] filename={0} arguments={1}", process.StartInfo.FileName,
                                    process.StartInfo.Arguments));

            process.Start();

            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();

            float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
            Debug.Log(string.Format("[Results] elapsedTime: {3} errors: {2}\noutput: {1}", process.StartInfo.FileName,
                                    output, error, elapsed));

            //if (output.Length > 0 ) Debug.Log("Output: " + output);
            //if (error.Length > 0 ) Debug.Log("Error: " + error);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(string.Format("Unable to run process: path={0} arguments={1} exception={2}", path, arguments, ex));
        }

    }
    #endregion

    #region File IO

    private static void GetAssets(string extension, List<string> files, DirectoryInfo directory)
    {
        if (null == directory)
        {
            return;
        }
        foreach (FileInfo file in directory.GetFiles(extension))
        {
            if (string.IsNullOrEmpty(file.FullName) ||
                files.Contains(file.FullName))
            {
                continue;
            }
            files.Add(file.FullName);
            //Debug.Log(string.Format("File: {0}", file.FullName));
        }
        foreach (DirectoryInfo subDir in directory.GetDirectories())
        {
            if (null == subDir)
            {
                continue;
            }
            //Debug.Log(string.Format("Directory: {0}", subDir));
            GetAssets(extension, files, subDir);
        }
    }

    #endregion

    #region Unique identification

    public static string UID = GetUID();

    /// <summary>
    /// Get the machine name
    /// </summary>
    /// <returns></returns>
    private static string GetMachineName()
    {
        try
        {
            string machineName = System.Environment.MachineName;
            if (!string.IsNullOrEmpty(machineName))
            {
                return machineName;
            }
        }
        catch (System.Exception)
        {
            Debug.LogError("GetMachineName: Failed to get machine name");
        }

        return "Unknown";
    }

    /// <summary>
    /// Get the IDE process IDE
    /// </summary>
    /// <returns></returns>
    private static int GetProcessID()
    {
        try
        {
            Process process = Process.GetCurrentProcess();
            if (null != process)
            {
                return process.Id;
            }
        }
        catch
        {
            Debug.LogError("GetProcessID: Failed to get process id");
        }

        return 0;
    }

    /// <summary>
    /// Get a unique identifier for the Unity instance
    /// </summary>
    /// <returns></returns>
    private static string GetUID()
    {
        try
        {
            return string.Format("{0}_{1}", GetMachineName(), GetProcessID());
        }
        catch (System.Exception)
        {
            Debug.LogError("GetUID: Failed to create uid");
        }

        return string.Empty;
    }

    #endregion
}