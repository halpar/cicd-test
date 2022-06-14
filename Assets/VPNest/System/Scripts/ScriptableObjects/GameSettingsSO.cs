using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using SRDebugger.Editor;
#endif

namespace VP.Nest
{
    // [CreateAssetMenu(fileName = "Game Settings", menuName = "ScriptableObjects/GameSettings", order = 2)]

    public class GameSettingsSO : ScriptableObject
    {
        public SceneLoadSettingsSO SelectedSceneLoadSettings;
        public bool IsUsingLevelScene = false;

        public bool IsDebugBuild
        {
            set
            {
#if UNITY_EDITOR
                SRDebugEditor.SetEnabled(value);
#endif
            }
        }

#if UNITY_EDITOR

        [MenuItem("Nest/Game Settings/General", false, 14)]
        public static void Settings()
        {
            var settings = Resources.Load<GameSettingsSO>(GameSettings.SETTINGSFILEPATH);
            if (settings != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settings;
            }
        }

#endif
    }
}