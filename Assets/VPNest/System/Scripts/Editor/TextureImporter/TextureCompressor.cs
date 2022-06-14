using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    internal class TextureCompressor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            if (assetPath.Contains("Icon"))
            {
                var importer = (TextureImporter)assetImporter;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.textureType = TextureImporterType.Default;
                importer.mipmapEnabled = false;
                importer.maxTextureSize = 1024;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
                return;
            }
            

            if (assetPath.Contains("VP_NC")) return;

            var textureImporter = (TextureImporter)assetImporter;
            if (textureImporter.textureType == TextureImporterType.Default &&
                textureImporter.textureShape == TextureImporterShape.Texture2D)
            {
                
                string assetName = assetPath.Split("/").Last();
                if (assetName.StartsWith("s_") || assetName.StartsWith("S_")) textureImporter.textureType = TextureImporterType.Sprite;

                if (textureImporter.DoesSourceTextureHaveAlpha())
                {
                    textureImporter.alphaIsTransparency = true;
                }

                textureImporter.maxTextureSize = 512;
                textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.mipmapEnabled = false;
                textureImporter.SaveAndReimport();
            }
        }
    }
}