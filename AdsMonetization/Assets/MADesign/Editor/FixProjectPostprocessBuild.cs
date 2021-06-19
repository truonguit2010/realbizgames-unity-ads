using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

// https://developers.ironsrc.com/ironsource-mobile/unity/admob-mediation-guide/#step-4
class FixProjectPostprocessBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 2; } }

    public string ADMOB_IOS_APP_ID = "";


    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("MyCustomBuildProcessor.OnPostprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        Environment.SetEnvironmentVariable("OUTPUT_PATH", report.summary.outputPath);
#if UNITY_EDITOR_OSX
        if (report.summary.platform == BuildTarget.iOS)
        {

            string plistPath = report.summary.outputPath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            string encryptKey = "ITSAppUsesNonExemptEncryption";
            rootDict.SetBoolean(encryptKey, false);

            plist.root.SetString("GADApplicationIdentifier", ADMOB_IOS_APP_ID);

            if (ADMOB_IOS_APP_ID == string.Empty) {
                Debug.LogErrorFormat("FixProjectPostprocessBuild - please add ADMOB_IOS_APP_ID for this project");
            }

        // --------------------------------
        // https://developers.ironsrc.com/ironsource-mobile/unity/unity-plugin/#step-3
        //https://developers.ironsrc.com/ironsource-mobile/general/ios-14-network-support/
            // You can also add SKAdNetworkIdentifier  to your Info.plist, by using this code:
            // <key>SKAdNetworkItems</key>
            //  < array >
            //      < dict >
            //          < key > SKAdNetworkIdentifier </ key >
            //          < string > su67r6k2v3.skadnetwork </ string >
            //      </ dict >
            // </ array >
            //---------------------------------
            PlistElementArray ios14Array = plist.root.CreateArray("SKAdNetworkItems");
            // ironsource
            ios14Array.AddDict().SetString("SKAdNetworkIdentifier", "su67r6k2v3.skadnetwork");
            // unity
            ios14Array.AddDict().SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork");
            // FAN
            ios14Array.AddDict().SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
            ios14Array.AddDict().SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
            // ADMOB
            ios14Array.AddDict().SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork");
            // END
            //---------------------------------

            File.WriteAllText(plistPath, plist.WriteToString());

            addCalabilities(report);
#endif
        }
    }

    public void addCalabilities(BuildReport report) {
#if UNITY_EDITOR_OSX
        if (report.summary.platform == BuildTarget.iOS)
        {
            string pathToBuiltProject = report.summary.outputPath;
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string targetGUID = project.GetUnityMainTargetGuid();// PBXProject.GetUnityTargetName(); // note, not "project." ...
            string unityTergetFrameworkGuid = project.GetUnityFrameworkTargetGuid();
            //string targetGUID = project.TargetGuidByName(targetName);

            //AddFrameworks(project, targetName);
            //project.AddFrameworkToProject(targetGUID, "Security.framework", false);
            project.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
            //project.AddFrameworkToProject(targetGUID, "iAd.framework", false);
            project.AddFrameworkToProject(targetGUID, "UserNotifications.framework", false);
            project.AddFrameworkToProject(targetGUID, "StoreKit.framework", false);

            // 
            // Fix bug: ERROR ITMS-90206: "Invalid Bundle. The bundle at 'matching.app/Frameworks/UnityFramework.framework' contains disallowed file 'Frameworks'."
            //UnityFramework
            // Always Embed Swift Standard Libraries
            project.SetBuildProperty(unityTergetFrameworkGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            project.SetBuildProperty(targetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            // Write.
            File.WriteAllText(projectPath, project.WriteToString());

            // Capability
            ProjectCapabilityManager manager = new ProjectCapabilityManager(
                projectPath,
                "Entitlements.entitlements",
                targetGuid: project.GetUnityMainTargetGuid()
            );
            manager.AddGameCenter();
            manager.AddPushNotifications(false);
            manager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
            manager.AddInAppPurchase();
            manager.WriteToFile();
        }
#endif
    }
}