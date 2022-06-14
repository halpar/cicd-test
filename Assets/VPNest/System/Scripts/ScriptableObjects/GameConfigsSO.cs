#if UNITY_EDITOR
using Facebook.Unity.Settings;
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace VP.Nest
{
    public class GameConfigsSO : ScriptableObject
    {
        private const string CONFIGSFILEPATH = "GameConfigs";
        //private List<string> appIDs = new List<string>(1); 


        [Header("Company Settings")] public string companyName = "DT Games";


        public string teamID = "S49JT96D3D";

        [Header("Product Settings")] 
        public string productName;
        public string bundleID;
        public string version;
        public string elephantGameID;
        public string elephantGameSecret;
        public string facebookAppID;
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
            this.elephantGameID = gameConfigsSO.elephantGameID;
            this.facebookAppID = gameConfigsSO.facebookAppID;
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
            
            ElephantSettings.GetElephantSettingsSO().SetGameID(elephantGameID);
            ElephantSettings.GetElephantSettingsSO().SetGameSecret(elephantGameSecret);
            FacebookSettings.AppIds = new List<string>(){facebookAppID};
            FacebookSettings.AppLabels = new List<string>(){productName};
            
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [MenuItem("Nest/Game Settings/Game Configs", false,16)]
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