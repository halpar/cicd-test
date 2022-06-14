using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace VP.Nest.SceneManagement
{
	// Quickly testing specific levels
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif

	public static class EditorLevelTestDropdown
	{
#if UNITY_EDITOR
		private static string previousScenePath;

		static EditorLevelTestDropdown()
		{
			EditorApplication.playModeStateChanged += ModeChanged;
		}

		[InitializeOnLoadMethod]
		private static void ShowStartSceneButton()
		{
			UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Add(() =>
			{
				// GUILayout.Space(20f);
				GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;

				EditorGUI.BeginChangeCheck();

				var gameSceneCount = GameSettings.SelectedSceneLoadSettings.gameSceneList.Count;
				var tutorialSceneCount = GameSettings.SelectedSceneLoadSettings.tutorialScenes.Count;

				string[] dropdown = new string[gameSceneCount + tutorialSceneCount + 1];

				dropdown[0] = "Load Level";
				int index = 1;

				for (int i = 1; i <= tutorialSceneCount; i++, index++)
					dropdown[index] = i + " - Tutorial Scene";

				for (int i = 1; i <= gameSceneCount; i++, index++)
					dropdown[index] = i + " - Game Scene";

				int value = EditorGUILayout.Popup(0, dropdown, "Dropdown", GUILayout.Width(90f));

				if (EditorGUI.EndChangeCheck())
				{
					if (value > 0)
					{
						PlayerPrefKeys.CurrentLevel = value;

						EditorWindow.GetWindow(typeof(SceneView).Assembly.GetType("UnityEditor.GameView")).ShowNotification(new GUIContent("Testing Level " + value));

						EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

						framesToWaitUntilPlayMode = 0;
						EditorApplication.update -= EnterPlayMode;
						EditorApplication.update += EnterPlayMode;
					}
				}

				GUI.enabled = true;
			});
		}

		private static int framesToWaitUntilPlayMode; // Wait for the notification to be displayed

		private static void EnterPlayMode()
		{
			if (framesToWaitUntilPlayMode-- <= 0)
			{
				EditorApplication.update -= EnterPlayMode;

				EditorSceneManager.OpenScene(GameSettings.SelectedSceneLoadSettings.splashScene.ScenePath);

				EditorPrefs.SetInt("TestingLevel", 1);

				EditorApplication.EnterPlaymode();
			}
		}

		private static void ModeChanged(PlayModeStateChange state)
		{
			// save scene before entering play mode
			if (state == PlayModeStateChange.ExitingEditMode)
				EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

			// variable for opening the active scene
			if (state == PlayModeStateChange.ExitingPlayMode)
				previousScenePath = EditorSceneManager.GetActiveScene().path;

			// opening the active scene
			if (state == PlayModeStateChange.EnteredEditMode)
				if (EditorPrefs.GetInt("TestingLevel").Equals(1))
				{
					EditorPrefs.SetInt("TestingLevel", 0);
					EditorSceneManager.OpenScene(previousScenePath);
				}
		}
#endif
	}
}