using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;
using UnityEditor.SceneManagement;

namespace VP.Nest.SceneManagement
{
	[InitializeOnLoad]
	public static class EditorBuildSceneRefresh
	{
		static bool m_enabled;

		static bool Enabled {
			get { return m_enabled; }
			set { m_enabled = value; }
		}

		static EditorBuildSceneRefresh()
		{
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.Space(25f);

			var tex = EditorGUIUtility.IconContent("Refresh").image;

			GUI.changed = false;

			GUILayout.Toggle(m_enabled, new GUIContent(null, tex, "Refreshing Scene List"),
				"Command");
			if (GUI.changed) {
				string[] gameplayScenesPaths = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();

				List<EditorBuildSettingsScene> editorBuildScenes =
					new List<EditorBuildSettingsScene>();


				for (int i = 0; i < gameplayScenesPaths.Length; i++) {
					TryAddNewBuildSceneByPath(ref editorBuildScenes, gameplayScenesPaths[i]);
				}

				EditorBuildSettings.scenes = editorBuildScenes.ToArray();

				GameConfigsSO.GetGameConfigsSO().ManualValidate();

				AssetDatabase.SaveAssets();

				//Debug.Log(GameSettings.SelectedSceneLoadSettings.splashScene.ScenePath);


				Debug.Log("EditorBuildSettings and PLayer Settings Refresh");
			}
		}

		private static bool TryAddNewBuildSceneByPath(ref List<EditorBuildSettingsScene> sceneList,
			string path)
		{
			EditorBuildSettingsScene buildScene = new EditorBuildSettingsScene(path, true);
			if (!IsContainsSceneInSceneList(ref sceneList, buildScene)) {
				sceneList.Add(buildScene);
				return true;
			}

			return false;
		}

		private static bool IsContainsSceneInSceneList(ref List<EditorBuildSettingsScene> sceneList,
			EditorBuildSettingsScene targetScene)
		{
			for (int i = 0; i < sceneList.Count; i++) {
				if (sceneList[i].path == targetScene.path) {
					return true;
				}
			}

			return false;
		}
	}
}