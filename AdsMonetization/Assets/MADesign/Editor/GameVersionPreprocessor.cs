using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;


#if UNITY_2018_1_OR_NEWER
public class GameVersionPreProcessor : IPreprocessBuildWithReport
#else
public class GameVersionPreProcessor : IPreprocessBuild 
#endif
{
    public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
    {
        TextAsset versionFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/version.txt");
        string strVersion = versionFile.text.Trim();
        strVersion = strVersion.Trim();
        string[] listStr = strVersion.Split('.');
        if (listStr != null && listStr.Length >= 3)
        {
            PlayerSettings.bundleVersion = string.Format("{0}.{1}", listStr[0], listStr[1]);
            //PlayerSettings.Android.bundleVersionCode = int.Parse(listStr[2]);
            //PlayerSettings.iOS.buildNumber = listStr[2];
        }
    }
}

