using UnityEngine;
using VP.Nest.SceneManagement;

namespace VP.Nest
{
    public static class GameSettings
    {
        public const string SETTINGSFILEPATH = "GameSettings";

        public static SceneLoadSettingsSO SelectedSceneLoadSettings
        {
            get
            {
                GameSettingsSO gameSettingsSo = Resources.Load<GameSettingsSO>(SETTINGSFILEPATH);
                if (gameSettingsSo)
                    return gameSettingsSo.SelectedSceneLoadSettings;
                return null;
            }
            set
            {
                GameSettingsSO gameSettingsSo = Resources.Load<GameSettingsSO>(SETTINGSFILEPATH);
                gameSettingsSo.SelectedSceneLoadSettings = value;
            }
        }

        public static bool IsUsingLevelScene
        {
            get
            {
                GameSettingsSO gameSettingsSo = Resources.Load<GameSettingsSO>(SETTINGSFILEPATH);
                if (gameSettingsSo)
                    return gameSettingsSo.IsUsingLevelScene;
                return false;
            }
            set
            {
                GameSettingsSO gameSettingsSo = Resources.Load<GameSettingsSO>(SETTINGSFILEPATH);
                gameSettingsSo.IsUsingLevelScene = value;
            }
        }
    }
}