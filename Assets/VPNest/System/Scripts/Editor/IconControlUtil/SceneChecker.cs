using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    public class SceneChecker : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            CheckScenePaths();
        }

        private static void CheckScenePaths()
        {
            var sceneSettings = GameSettings.SelectedSceneLoadSettings;

            if (!IsSceneValid(sceneSettings.splashScene, "splashScene"))
                return;

            if (!IsSceneValid(sceneSettings.loadingScene, "loadingScene"))
                return;

            if (!IsSceneValid(sceneSettings.levelSelectScene, "levelSelectScene"))
                return;

            for (int i = 0; i < sceneSettings.tutorialScenes.Count; i++)
            {
                if (!IsSceneValid(sceneSettings.tutorialScenes[i], "tutorialScene_" + i))
                    return;
            }

            for (int i = 0; i < sceneSettings.gameSceneList.Count; i++)
            {
                if (!IsSceneValid(sceneSettings.gameSceneList[i], "gameScene_" + i))
                    return;
            }

            for (int i = 0; i < sceneSettings.otherScenes.Count; i++)
            {
                if (!IsSceneValid(sceneSettings.otherScenes[i], "otherScene" + i))
                    return;
            }
        }

        private static bool IsSceneValid(SceneReference sceneReference, string errorId)
        {
            if (sceneReference.IsValidSceneAsset)
            {
                return true;
            }
            else
            {
                LogError(errorId);
                return false;
            }
        }

        private static void LogError(string sceneIndex)
        {
            string message = "Scene missing : ";
            message += sceneIndex;
            throw new BuildFailedException(message);
        }
    }
}