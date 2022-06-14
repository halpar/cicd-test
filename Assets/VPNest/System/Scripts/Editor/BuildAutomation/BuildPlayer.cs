using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using VP.Nest.SceneManagement;

namespace VP.Nest.System.Editor
{
    public class BuildPlayer : MonoBehaviour
    {
        private const string lunarConsolePath = "Assets/VPNest/Debug/LunarConsole/Scripts/LunarConsole.prefab";
        private const string debugCanvasPath = "Assets/VPNest/Debug/Prefabs/DebugCanvasSystem.prefab";

        [MenuItem("Nest/Build/iOS Submit", false, 2)]
        public static void BuildiOSRelease()
        {
            NestEditorRefresh.Refresh();

            GameSettings.GameSettingsSo.IsDebugBuild = false;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();
            buildPlayerOptions.locationPathName = "Builds"; //included to gitignore
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder("Builds");
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");

                GameConfigsSO gameConfigsSo = GameConfigsSO.GetGameConfigsSO();
                if (gameConfigsSo)
                {
                    gameConfigsSo.buildNo++;

                    gameConfigsSo.ManualValidate();

                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(gameConfigsSo);
                }
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        [MenuItem("Nest/Build/iOS TF", false, 0)]
        public static void BuildiOSTF()
        {
            NestEditorRefresh.Refresh();
            GameSettings.GameSettingsSo.IsDebugBuild = true;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();
            buildPlayerOptions.locationPathName = "Builds"; //included to gitignore
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;


            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder("Builds");
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");

                GameConfigsSO gameConfigsSo = GameConfigsSO.GetGameConfigsSO();
                if (gameConfigsSo)
                {
                    gameConfigsSo.buildNo++;

                    gameConfigsSo.ManualValidate();

                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(gameConfigsSo);
                }
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }


        [MenuItem("Nest/Build/iOS Profiler", false, 1)]
        public static void BuildiOSProfiler()
        {
            NestEditorRefresh.Refresh();
            
            GameSettings.GameSettingsSo.IsDebugBuild = true;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();
            buildPlayerOptions.locationPathName = "Builds"; //included to gitignore
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging |
                                         BuildOptions.ConnectWithProfiler;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder("Builds");
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }


        private static void SetActivePrefabEditor(bool active, string path)
        {
            GameObject contentsRoot =
                UnityEditor.PrefabUtility.LoadPrefabContents(
                    path);

            contentsRoot.SetActive(active);

            UnityEditor.PrefabUtility.SaveAsPrefabAsset(contentsRoot,
                path);
            UnityEditor.PrefabUtility.UnloadPrefabContents(contentsRoot);
        }


        [MenuItem("Nest/Build/Open Builds Folder", false, 13)]
        public static void OpenBuildsFolder()
        {
#if UNITY_EDITOR_OSX
            EditorUtility.RevealInFinder("Builds");
#elif UNITY_EDITOR_WIN
            //TODO: Open in File Explorer
#endif
        }
    }
}