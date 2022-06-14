using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VP.Nest.SceneManagement
{
	[CustomEditor(typeof(GameSettingsSO))]
	public class GameSettingsEditorWindow : Editor
	{
		private SceneLoadSettingsSO selectedSettings;
		private bool isLevelSceneActive;

		//[MenuItem("Tools/Game Settings/General")]
		private static void ShowWindow()
		{
			//GetWindow<GameSettingsEditorWindow>(false, "Game Settings", true);
		}

		void OnGUI()
		{
			GUILayout.Label("Game Settings", EditorStyles.boldLabel);

			DrawSceneLoadSettings();

			DrawEnableLevelScene();
		}

		private void DrawEnableLevelScene()
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Enable Level Scene :", EditorStyles.label);
			var value = GameSettings.IsUsingLevelScene;
			isLevelSceneActive = EditorGUILayout.Toggle("", value);
			GameSettings.IsUsingLevelScene = isLevelSceneActive;

			EditorGUILayout.EndHorizontal();
		}

		private void DrawSceneLoadSettings()
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Select Scene Load Settings :", EditorStyles.label);

			SceneLoadSettingsSO scene = GameSettings.SelectedSceneLoadSettings;

#pragma warning disable 618
			selectedSettings = EditorGUILayout.ObjectField(scene,
#pragma warning restore 618
					 typeof(SceneLoadSettingsSO)) as
				SceneLoadSettingsSO;

			if (selectedSettings)
				GameSettings.SelectedSceneLoadSettings = selectedSettings;

			EditorGUILayout.EndHorizontal();
		}
	}
}