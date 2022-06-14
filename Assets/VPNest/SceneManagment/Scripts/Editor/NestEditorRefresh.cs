using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityToolbarExtender;

namespace VP.Nest.SceneManagement
{
    [InitializeOnLoad]
    public static class NestEditorRefresh
    {
        static bool m_enabled;

        static bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        static NestEditorRefresh()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        public static UnityAction OnEditorRefreshButtonClicked;

        static void OnToolbarGUI()
        {
            GUILayout.Space(25f);

            var tex = EditorGUIUtility.IconContent("Refresh").image;

            GUI.changed = false;

            GUILayout.Toggle(m_enabled, new GUIContent(null, tex, "Refreshing Scene List"),
                "Command");
            if (GUI.changed)
            {
                Refresh();
                //OnEditorRefreshButtonClicked?.Invoke();
            }
        }

        public static void Refresh()
        {
            var selectedSceneSettings = GameSettings.SelectedSceneLoadSettings;
            string[] gameplayScenesPaths = selectedSceneSettings.GetUniqueScenesPaths();

            List<EditorBuildSettingsScene> editorBuildScenes = new List<EditorBuildSettingsScene>();

            for (int i = 0; i < gameplayScenesPaths.Length; i++)
            {
                TryAddNewBuildSceneByPath(ref editorBuildScenes, gameplayScenesPaths[i]);
            }

            if (string.IsNullOrEmpty(selectedSceneSettings.splashScene.ScenePath))
            {
                const string message = "No splash scene has been assigned!";
                if (EditorUtility.DisplayDialog("Error", message, "Assing"))
                {
                    SceneLoadSettingsSO.SetSplashScene();
                }
            }

            EditorBuildSettings.scenes = editorBuildScenes.ToArray();

            GameConfigsSO.GetGameConfigsSO().ManualValidate();

            SplashLoadingScreen.SetupPrefab();

            AssetDatabase.SaveAssets();

            Debug.Log("EditorBuildSettings and Player Settings Refresh");
        }

        private static bool TryAddNewBuildSceneByPath(ref List<EditorBuildSettingsScene> sceneList,
            string path)
        {
            EditorBuildSettingsScene buildScene = new EditorBuildSettingsScene(path, true);
            if (!IsContainsSceneInSceneList(ref sceneList, buildScene))
            {
                sceneList.Add(buildScene);
                return true;
            }

            return false;
        }

        private static bool IsContainsSceneInSceneList(ref List<EditorBuildSettingsScene> sceneList,
            EditorBuildSettingsScene targetScene)
        {
            for (int i = 0; i < sceneList.Count; i++)
            {
                if (sceneList[i].path == targetScene.path)
                {
                    return true;
                }
            }

            return false;
        }
    }
}