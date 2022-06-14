using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace VP.Nest.SceneManagement
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public static class EditorSelectSceneDropdown
	{
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static void ShowStartSceneButton()
		{
			UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Add(() =>
			{
				GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;

				EditorGUI.BeginChangeCheck();

				var gameSceneCount = GameSettings.SelectedSceneLoadSettings.gameSceneList.Count;
				var tutorialSceneCount = GameSettings.SelectedSceneLoadSettings.tutorialScenes.Count;

				string[] dropdown = new string[gameSceneCount + tutorialSceneCount + 2];

				dropdown[0] = "Select Scene";

				dropdown[1] = "Splash Scene";
				int index = 2;

				for (int i = 1; i <= tutorialSceneCount; i++, index++)
					dropdown[index] = i + " - Tutorial Scene";

				for (int i = 1; i <= gameSceneCount; i++, index++)
					dropdown[index] = i + " - Game Scene";
				
				int value = EditorGUILayout.Popup(0, dropdown, "Dropdown", GUILayout.Width(100f));

				if (EditorGUI.EndChangeCheck())
				{
					if (value > 0)
					{
						EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

						if (value.Equals(1))
						{
							EditorSceneManager.OpenScene(GameSettings.SelectedSceneLoadSettings.splashScene.ScenePath);
						}
						else
						{
							int sceneIndex = value - 2;

							EditorSceneManager.OpenScene(sceneIndex >= tutorialSceneCount
								? GameSettings.SelectedSceneLoadSettings.gameSceneList[sceneIndex - tutorialSceneCount].ScenePath
								: GameSettings.SelectedSceneLoadSettings.tutorialScenes[sceneIndex].ScenePath);
						}
					}
				}

				GUI.enabled = true;
			});
		}
#endif
	}
}