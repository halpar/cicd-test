using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    public static class ProjectManager
    {
        [InitializeOnLoadMethod]
        private static void InitializeProjectOnIOS()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS
                || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android
                || EditorApplication.isCompiling) return;

            //PlayerSettings.companyName = "Virtual Projects";

            //var path = Directory.GetCurrentDirectory();
            //var productName = Path.GetFileName(path);
            //productName = productName.Replace("-", " ");
            //productName = productName.Replace("_", " ");
            //productName = productName.Replace("!", "");


            // Delete Unity generated Scenes Folder
            FileUtil.DeleteFileOrDirectory("Assets/Scenes");
            FileUtil.DeleteFileOrDirectory("Assets/Scenes.meta");
            AssetDatabase.Refresh();

            // GameConfigsSO gameConfigsSo = GameConfigsSO.GetGameConfigsSO();
            // if (gameConfigsSo) {
            // 	gameConfigsSo.productName = productName;
            // 	gameConfigsSo.ManualValidate();
            // }


            //EditorSceneManager.OpenScene("Assets/_Main/Scenes/Build/A_SplashScene.unity");
            // Switch build target to iOS
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, BuildTarget.iOS);
        }

        public static void ContinueSettingsAfterPlatformSwitch(BuildTarget target)
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.accelerometerFrequency = 0;

            var buildTargetGroup = target switch
            {
                BuildTarget.iOS => BuildTargetGroup.iOS,
                BuildTarget.Android => BuildTargetGroup.Android,
                _ => BuildTargetGroup.Unknown
            };
            var bundleIdProductNamePart = PlayerSettings.productName.ToLower();
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup, $"com.asg.{bundleIdProductNamePart}");
            PlayerSettings.SetApiCompatibilityLevel(buildTargetGroup, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);

            // Platform Specific Settings
            if (target == BuildTarget.iOS)
            {
                PlayerSettings.iOS.appleEnableAutomaticSigning = true;
                PlayerSettings.iOS.buildNumber = "0";
                PlayerSettings.iOS.hideHomeButton = true;
            }
            else if (target == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = 1;
                PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

                //TODO: Nice Vibrations Permission to Manifest File
                // Probably to Prebuild Process
            }

            // Assign Temporary Icon
            string iconPath = AssetDatabase.GetAssetPath(GameConfigsSO.GetGameConfigsSO().icon);
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] { icon });

            // Edit & Assign Quality Settings
            var settingIndex = Array.IndexOf(QualitySettings.names, "Medium");
            QualitySettings.SetQualityLevel(settingIndex, true);
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            //QualitySettings.shadowProjection = ShadowProjection.StableFit;
            //QualitySettings.shadowDistance = 100;
            QualitySettings.shadowNearPlaneOffset = 0;
            QualitySettings.shadowCascades = 0;

            // Add initial scenes to build settings and Open Splash Scene
            // var currentScenes = EditorBuildSettings.scenes;
            // var guids = AssetDatabase.FindAssets("t:Scene");
            // foreach (var guid in guids)
            // {
            //     var scenePath = AssetDatabase.GUIDToAssetPath(guid);
            //     var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            //     if (!sceneAsset.name.Contains("A_SplashScene") && !sceneAsset.name.Contains("B_GameScene")) continue;
            //
            //     var canContiune = true;
            //     foreach (var buildSettingsScene in currentScenes)
            //     {
            //         if (buildSettingsScene.path.Contains(sceneAsset.name))
            //             canContiune = false;
            //     }
            //
            //     if (!canContiune)
            //         continue;
            //
            //     var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes)
            //     {
            //         new EditorBuildSettingsScene(scenePath, true)
            //     };
            //     EditorBuildSettings.scenes = scenes.ToArray();
            //
            //     if (sceneAsset.name.Contains("A_SplashScene"))
            //     {
            //         EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            //     }
            // }

            // Save
            SaveSettingsValues();
        }

        private static void SaveSettingsValues()
        {
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }
    }
}