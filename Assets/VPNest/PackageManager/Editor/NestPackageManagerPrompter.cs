using System.Collections.Generic;
using System.Reflection;
using NestPackageManager.Model;
using NestPackageManager.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace NestPackageManager
{
    [InitializeOnLoad]
    public class NestPackageManagerPrompter
    {
        private const string KeyVersion = "key_version";

        static NestPackageManagerPrompter()
        {
            FetchManifestStatus();
        }

        private static void FetchManifestStatus()
        {
            var request = UnityWebRequest.Get(ManifestSource.ManifestURL);
            request.SendWebRequest();
            while (!request.isDone && !request.isHttpError && !request.isNetworkError)
            {
                // no-op
            }

            if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error))
            {
                Debug.LogError("Couldn't finish opening request!");
                return;
            }

            var responseJson = request.downloadHandler.text;
            request.Dispose();
            HandleResponse(responseJson);
        }

        private static void HandleResponse(string responseJson)
        {
            var manifest = JsonUtility.FromJson<Manifest>(responseJson);

            var bundles = manifest.bundles;


            List<PackageData> packageData = new List<PackageData>();

            for (int i = 0; i < bundles.Count; i++)
            {
                string[] temp = bundles[i].bundle_id.Split('_');

                //   Debug.Log(temp.Length);

                PackageData data = new PackageData();
                data.name = temp[0];
                data.version = temp[1];
                data.source = temp[2];
                packageData.Add(data);
            }


            for (int i = 0; i < packageData.Count; i++)
            {
                if (IsReadyToShow(packageData[i]))
                {
                    if (packageData[i] != null)
                        PlayerPrefs.SetString(KeyVersion + packageData[i].name, packageData[i].version);
                }
            }


            //NestPackageManager.ShowPackageManager();
        }

        private static bool IsReadyToShow(PackageData packageData)
        {
            if (packageData == null)
            {
                return false;
            }

            return true;
        }

        private static bool HasShown(PackageData packageData)
        {
            return packageData.version.Equals(PlayerPrefs.GetString(KeyVersion + packageData.name, ""));
        }
    }
}