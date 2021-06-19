using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Purchasing;
using UnityEditor.Build.Reporting;
//using UnityEngine.Purchasing;

// 201023001
public class AutoIncrementVersionCodeInCloudBuild : MonoBehaviour, UnityEditor.Build.IPreprocessBuildWithReport
{
#if UNITY_CLOUD_BUILD
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
#if AMAZON_STORE
        UnityPurchasingEditor.TargetAndroidStore(AndroidStore.AmazonAppStore);
#endif
        string buildNumber = manifest.GetValue("buildNumber", "0");
        increaseBuildVersion(buildNumber);
    }
#else

#endif

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
#if !UNITY_CLOUD_BUILD
        string buildNumber = PlayerPrefs.GetString("buildNumber", "0");
        increaseBuildVersion(buildNumber);

        int intBuildNumber = int.Parse(buildNumber);
        PlayerPrefs.SetString("buildNumber", (intBuildNumber + 1).ToString());
        PlayerPrefs.Save();
#endif
    }

    private static void increaseBuildVersion(string buildNumber)
    {
        System.DateTime dateTime = System.DateTime.Now;
        int yearIn2Number = dateTime.Year % 1000;

        string dayIn2Numbers = dateTime.Day.ToString();
        if (dateTime.Day < 10)
        {
            dayIn2Numbers = "0" + dateTime.Day.ToString();
        }

        string monthIn2Numbers = dateTime.Month.ToString();
        if (dateTime.Month < 10)
        {
            monthIn2Numbers = "0" + monthIn2Numbers;
        }


        string prefix = "" + yearIn2Number + monthIn2Numbers + dayIn2Numbers;
        int intBuildNumber = int.Parse(buildNumber);
        intBuildNumber = intBuildNumber % 1000;
        buildNumber = intBuildNumber.ToString();

        if (buildNumber.Length == 1)
        {
            buildNumber = "00" + buildNumber;
        }
        else if (buildNumber.Length == 2)
        {
            buildNumber = "0" + buildNumber;
        }
        buildNumber = prefix + buildNumber;

        Debug.Log("Build number Setting build number to " + buildNumber);

        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);
        PlayerSettings.iOS.buildNumber = buildNumber;
    }
}
