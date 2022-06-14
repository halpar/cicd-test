using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VP.Nest.SceneManagement;

namespace VP.Nest
{
    [CustomEditor(typeof(SceneLoadSettingsSO))]
    public class SceneLoadSettingsEditor : Editor
    {
        private readonly GUIContent btnReset =
            new GUIContent("Initialize Scenes", "Resets the window to default values");

        public override void OnInspectorGUI()
        {
            // Rect lastRect = GUILayoutUtility.GetLastRect();
            // if (GUI.Button(new Rect(200, 10, 140, 25), btnReset))
            if (GUI.Button(new Rect((EditorGUIUtility.currentViewWidth - 140) * 0.5f, 10, 140, 25), btnReset))
            {
                List<string> gameScenes = new List<string>();
                List<string> tutorialScenes = new List<string>();
                List<string> otherScenes = new List<string>();

                var scenes = AssetDatabase.FindAssets("t:scene", new[] { "Assets/_Main/Scenes/Build" });
                foreach (var sceneguid in scenes)
                {
                    var path = AssetDatabase.GUIDToAssetPath(sceneguid);
                    var fileName = Path.GetFileName(path);
                    switch (fileName.Substring(0, 2).ToUpper())
                    {
                        case "G_":
                            gameScenes.Add(path);
                            break;
                        case "T_":
                            tutorialScenes.Add(path);
                            break;
                        case "O_":
                            otherScenes.Add(path);
                            break;
                    }
                }

                var settings = (SceneLoadSettingsSO)serializedObject.targetObject;
                settings.SetGameSceneList(gameScenes);
                settings.SetTutorialSceneList(tutorialScenes);
                settings.SetOtherSceneList(otherScenes);

                NestEditorRefresh.Refresh();
            }

            GUILayout.Space(50);
            DrawDefaultInspector();
        }
    }
}