#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

namespace VP.Nest
{
    [Serializable]
    public class GameConfigsSO : ScriptableObject
    {
        private const string CONFIGSFILEPATH = "GameConfigs";

        [Header("Company Settings")] public string companyName;
        public string teamID;

        [Header("Product Settings")] public string productName;
        public string bundleID;
        public string version;
        public int buildNo;

        [Header("Icon Settings")] public Texture2D icon;

        public static GameConfigsSO GetGameConfigsSO()
        {
            return Resources.Load<GameConfigsSO>(CONFIGSFILEPATH);
        }

        public GameConfigsSO(GameConfigsSO gameConfigsSO)
        {
            this.buildNo = gameConfigsSO.buildNo;
            this.bundleID = gameConfigsSO.bundleID;
            this.companyName = gameConfigsSO.companyName;
            this.teamID = gameConfigsSO.teamID;
            this.productName = gameConfigsSO.productName;
            this.version = gameConfigsSO.version;
        }

#if UNITY_EDITOR

        public void ManualValidate()
        {
            PlayerSettings.companyName = companyName;
            PlayerSettings.bundleVersion = version;
            PlayerSettings.iOS.buildNumber = buildNo.ToString();

            PlayerSettings.productName = productName;
            PlayerSettings.iOS.appleDeveloperTeamID = teamID;

            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] { icon });
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleID);

            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [MenuItem("Nest/Game Settings/Game Configs", false)]
        public static void Settings()
        {
            var settings = Resources.Load<GameConfigsSO>(CONFIGSFILEPATH);
            if (settings != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settings;
            }
        }
#endif
    }
}