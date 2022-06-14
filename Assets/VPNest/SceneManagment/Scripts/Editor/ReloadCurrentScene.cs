#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace VP.Nest.SceneManagement
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public static class ReloadCurrentScene
	{
#if UNITY_EDITOR

		public static UnityAction OnEditorReloadButtonClicked;

		private static readonly GUIContent content = new GUIContent();

		private const string text = "Reload";
		private const string iconID = "d_RotateTool On";

		static ReloadCurrentScene()
		{
			content.image = EditorGUIUtility.IconContent(iconID).image;
			content.text = text;

			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

			OnEditorReloadButtonClicked += Reload;
		}

		private static void OnToolbarGUI()
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			GUILayout.Space(30f);
			EditorGUI.BeginChangeCheck();

			EditorGUI.BeginDisabledGroup(!EditorApplication.isPlayingOrWillChangePlaymode);

			GUI.changed = false;
			GUI.Button(new Rect(5, 0, 72, 22), content);
			GUILayout.Space(50);

			if (GUI.changed)
			{
				OnEditorReloadButtonClicked?.Invoke();
			}
		}

		private static void Reload()
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
#endif
	}
}