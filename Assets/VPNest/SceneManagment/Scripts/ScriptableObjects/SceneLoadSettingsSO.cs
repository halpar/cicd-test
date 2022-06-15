using System.Collections.Generic;
using UnityEngine;
using VP.Nest;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu(fileName = "SceneLoadSettings", menuName = "ScriptableObjects/SceneLoadSettingsObject", order = 1)]
public class SceneLoadSettingsSO : ScriptableObject
{
    public SceneReference splashScene;
    public SceneReference levelSelectScene;
    public SceneReference loadingScene;

    public List<SceneReference> tutorialScenes;
    public List<SceneReference> gameSceneList;
    public List<SceneReference> otherScenes;


#if UNITY_EDITOR

    public string[] GetUniqueScenesPaths()
    {
        List<string> scenesPaths = new List<string>();

        scenesPaths.Add(splashScene.ScenePath);
        scenesPaths.Add(levelSelectScene.ScenePath);
        scenesPaths.Add(loadingScene.ScenePath);

        for (int i = 0; i < gameSceneList.Count; i++)
        {
            string path = gameSceneList[i].ScenePath;
            if (!scenesPaths.Contains(path) && gameSceneList[i].IsValidSceneAsset)
                scenesPaths.Add(path);
        }

        for (int i = 0; i < otherScenes.Count; i++)
        {
            string path = otherScenes[i].ScenePath;
            if (!scenesPaths.Contains(path) && otherScenes[i].IsValidSceneAsset)
                scenesPaths.Add(path);
        }

        for (int i = 0; i < tutorialScenes.Count; i++)
        {
            string path = tutorialScenes[i].ScenePath;
            if (!scenesPaths.Contains(path) && tutorialScenes[i].IsValidSceneAsset)
                scenesPaths.Add(path);
        }

        return scenesPaths.ToArray();
    }

    public SceneReference[] GetUniqueSceneAssets()
    {
        List<SceneReference> sceneAssets = new List<SceneReference>();

        sceneAssets.Add(splashScene);
        sceneAssets.Add(levelSelectScene);
        sceneAssets.Add(loadingScene);

        for (int i = 0; i < gameSceneList.Count; i++)
        {
            if (!sceneAssets.Contains(gameSceneList[i]) && gameSceneList[i].IsValidSceneAsset)
                sceneAssets.Add(gameSceneList[i]);
        }

        for (int i = 0; i < otherScenes.Count; i++)
        {
            if (!sceneAssets.Contains(otherScenes[i]) && otherScenes[i].IsValidSceneAsset)
                sceneAssets.Add(otherScenes[i]);
        }

        for (int i = 0; i < tutorialScenes.Count; i++)
        {
            if (!sceneAssets.Contains(tutorialScenes[i]) && tutorialScenes[i].IsValidSceneAsset)
                sceneAssets.Add(tutorialScenes[i]);
        }

        return sceneAssets.ToArray();
    }


    [MenuItem("Nest/Game Settings/Selected Scene Load Settings")]
    public static void Settings()
    {
        var settings = GameSettings.SelectedSceneLoadSettings;
        if (settings != null)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
        }
    }


#endif
}

#if UNITY_EDITOR
public class AssetModificationProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        CheckGameScenes();
    }

    private static void CheckGameScenes()
    {
        if (GameSettings.SelectedSceneLoadSettings != null)
        {
            for (int i = 0; i < GameSettings.SelectedSceneLoadSettings.gameSceneList.Count; i++)
            {
                if (!GameSettings.SelectedSceneLoadSettings.gameSceneList[i].IsValidSceneAsset)
                {
                    GameSettings.SelectedSceneLoadSettings.gameSceneList.Remove(GameSettings.SelectedSceneLoadSettings
                        .gameSceneList[i]);
                    i = 0;
                }
            }

            for (int i = 0; i < GameSettings.SelectedSceneLoadSettings.otherScenes.Count; i++)
            {
                if (!GameSettings.SelectedSceneLoadSettings.otherScenes[i].IsValidSceneAsset)
                {
                    GameSettings.SelectedSceneLoadSettings.otherScenes.Remove(GameSettings.SelectedSceneLoadSettings
                        .otherScenes[i]);
                    i = 0;
                }
            }

            for (int i = 0; i < GameSettings.SelectedSceneLoadSettings.tutorialScenes.Count; i++)
            {
                if (!GameSettings.SelectedSceneLoadSettings.tutorialScenes[i].IsValidSceneAsset)
                {
                    GameSettings.SelectedSceneLoadSettings.tutorialScenes.Remove(GameSettings.SelectedSceneLoadSettings
                        .tutorialScenes[i]);
                    i = 0;
                }
            }
        }
    }
}

#endif