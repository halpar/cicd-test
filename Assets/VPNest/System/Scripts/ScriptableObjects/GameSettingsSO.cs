using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace VP.Nest
{
    // [CreateAssetMenu(fileName = "Game Settings", menuName = "ScriptableObjects/GameSettings", order = 2)]

    public class GameSettingsSO : ScriptableObject
    {
        public SceneLoadSettingsSO SelectedSceneLoadSettings;
        public bool IsUsingLevelScene = false;


#if UNITY_EDITOR

        [MenuItem("Nest/Game Settings/General", false, 9)]
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