using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    public class IconController : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            CheckIconSettings();
        }

        private static void CheckIconSettings()
        {
            var icon = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown)[0];

            if (icon == null)
            {
                const string message = "No icon has been assigned!";
                EditorUtility.DisplayDialog("Error", message, "OK");
                throw new BuildFailedException(message);
            }

            // if (!icon.name.Contains("VP_NC"))
            // {
            //     const string message = "Icon name must start with: \"VP_NC_\" to avoid compression!";
            //     EditorUtility.DisplayDialog("Error", message, "OK");
            //     throw new BuildFailedException(message);
            // }

            if (icon.width != 1024 || icon.height != 1024)
            {
                const string message = "Icon size is not 1024x1024!";
                EditorUtility.DisplayDialog("Error", message, "OK");
                throw new BuildFailedException(message);
            }

            var path = AssetDatabase.GetAssetPath(icon);
            if (!path.Contains("Assets/VPNest/Icon"))
            {
                const string message = "Icon texture must be at \"Assets/VPNest/Icon\"";
                EditorUtility.DisplayDialog("Error", message, "OK");
                throw new BuildFailedException(message);
            }

            var importer = (TextureImporter) AssetImporter.GetAtPath(path);
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureType = TextureImporterType.Default;
            importer.mipmapEnabled = false;
            importer.maxTextureSize = 1024;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
    }
}