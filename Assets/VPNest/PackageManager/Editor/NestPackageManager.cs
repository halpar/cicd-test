using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NestPackageManager.Model;
using NestPackageManager.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace NestPackageManager
{
    public class NestPackageManager : EditorWindow
    {
        private const string AssetsPathPrefix = "Assets/";
        private const string DownloadDirectory = AssetsPathPrefix;

        private List<PackageData> packageList;
        private List<BundlePackage> bundleList;

        private EditorCoroutines.EditorCoroutine _editorCoroutine;
        private EditorCoroutines.EditorCoroutine _editorCoroutineSelfUpdate;
        private UnityWebRequest _downloader;
        private string _activity;

        private string _selfUpdateStatus;
        private bool _canUpdateSelf = false;
        private bool manuallySetActivity = false;

        private GUIStyle _labelStyle;
        private GUIStyle _headerStyle;
        private GUIStyle _sourceStyle;
        private readonly GUILayoutOption _fieldWidth = GUILayout.Width(60);

        private Vector2 _scrollPos;

        [MenuItem("Nest/Manage Nest Packages")]
        public static void ShowPackageManager()
        {
            var win = GetWindow<NestPackageManager>("Manage Packages");
            win.titleContent = new GUIContent("Nest Packages");
            win.Focus();
            win.CancelOperation();
            win.OnEnable();
        }

        public void Awake()
        {
            _labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };
            _headerStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                fixedHeight = 18
            };
            _sourceStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                fixedHeight = 18
            };

            CancelOperation();
        }

        public void OnEnable()
        {
            _editorCoroutine = this.StartCoroutine(FetchManifest());
        }

        void OnDisable()
        {
            CancelOperation();
        }

        public void OnGUI()
        {
            var stillWorking = _editorCoroutine != null || _downloader != null || manuallySetActivity == true;

            if (packageList != null && packageList.Count > 0)
            {
                using (new EditorGUILayout.VerticalScope("box"))
                using (var s = new EditorGUILayout.ScrollViewScope(_scrollPos, false, false))
                {
                    _scrollPos = s.scrollPosition;

                    var groupedPackageList = packageList.OrderBy(p => p.source).GroupBy(p => p.source).ToList();

                    PopulateGroupPackages(groupedPackageList, "Virtual Projects Packages");
                }
            }

            // Indicate async operation in progress.
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                EditorGUILayout.LabelField(stillWorking ? _activity : " ");
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                if (!stillWorking)
                {
                    if (GUILayout.Button("Done", _fieldWidth))
                        Close();
                }
                else
                {
                    if (GUILayout.Button("Cancel", _fieldWidth))
                        CancelOperation();
                }
            }

            GUILayout.Space(10);
        }

        private void PopulateGroupPackages(List<IGrouping<string, PackageData>> groupedPackagesList, string groupTitle)
        {
            GUILayout.Space(10);
            DragAndDropFunction();
            EditorGUILayout.LabelField(groupTitle, _sourceStyle, GUILayout.Height(20));

            using (new EditorGUILayout.VerticalScope("box"))
            {
                foreach (var VARIABLE in groupedPackagesList)
                {
                    PackageSourceHeaders(VARIABLE.Key);
                    PackagesHeaders();
                    foreach (var packageData in VARIABLE)
                    {
                        PackageRow(packageData);
                    }

                    GUILayout.Space(10);
                }
            }
        }

        private void PackageSourceHeaders(string sourceName)
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("SOURCE --> " + sourceName.ToUpper(), _sourceStyle);
            }

            GUILayout.Space(5);
        }

        private void PackagesHeaders()
        {
            GUILayout.Space(5);
        }

        private void PackageRow(PackageData packageInfo, Func<bool, bool> customButton = null)
        {
            var packageName = packageInfo.name;
            var latestVersion = packageInfo.version.Replace("v", string.Empty);


            var stillWorking = _editorCoroutine != null || _downloader != null;

            GUILayout.Space(4);
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField(new GUIContent { text = packageName });
                // GUILayout.Button(new GUIContent
                // {
                //     text = string.IsNullOrEmpty(latestVersion) ? "--" : latestVersion,
                // }, true ? EditorStyles.boldLabel : EditorStyles.label);
                // GUILayout.Space(3);
                GUILayout.Button(new GUIContent
                {
                    text = latestVersion ?? "--",
                }, true ? EditorStyles.boldLabel : EditorStyles.label);
                GUILayout.Space(3);
                if (customButton == null || !customButton(true))
                {
                    GUI.enabled = !stillWorking && (true);
                    if (GUILayout.Button(new GUIContent
                        {
                            text = false ? "Upgrade" : "Install",
                        }, _fieldWidth))
                        this.StartCoroutine(DownloadPackage(packageInfo));
                    GUI.enabled = true;
                }

                GUILayout.Space(5);
            }

            GUILayout.Space(4);
        }

        private IEnumerator FetchManifest()
        {
            yield return null;
            _activity = "Downloading package version manifest...";

            var unityWebRequest = new UnityWebRequest(ManifestSource.ManifestURL)
            {
                downloadHandler = new DownloadHandlerBuffer(),
                timeout = 10,
            };

            if (!string.IsNullOrEmpty(unityWebRequest.error))
            {
                Debug.LogError(unityWebRequest.error);
            }

            yield return unityWebRequest.SendWebRequest();

            Debug.Log(unityWebRequest.responseCode);

            var responseJson = unityWebRequest.downloadHandler.text;

            if (string.IsNullOrEmpty(responseJson))
            {
                Debug.LogError("Unable to retrieve Package version manifest.  Showing installed Packages only.");

                yield break;
            }

            unityWebRequest.Dispose();

            var manifest = JsonUtility.FromJson<Manifest>(responseJson);

            bundleList = manifest.bundles;

            //   Debug.Log(bundleList.Count);

            if (packageList == null)
                packageList = new List<PackageData>();
            else
                packageList.Clear();


            for (int i = 0; i < bundleList.Count; i++)
            {
                string name = Path.GetFileNameWithoutExtension(bundleList[i].bundle_id);
                string[] temp = name.Split('_');

                PackageData data = new PackageData();
                data.name = temp[0];
                data.version = temp[1];
                data.source = temp[2];
                data.fullName = bundleList[i].bundle_id;
                packageList.Add(data);
            }

            CheckVersions();
        }

        private void CheckVersions()
        {
            _editorCoroutine = null;
            Repaint();
        }


        private IEnumerator DownloadPackage(PackageData packageInfo)
        {
            var path = Path.Combine(DownloadDirectory, packageInfo.fullName);
            _activity = $"Downloading {packageInfo.name}...";
            // Start the async download job.

            var body = new PostData();
            body.bundle_name = packageInfo.fullName;

            Debug.Log("bundle_name : " + packageInfo.fullName);

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(body));

            _downloader = new UnityWebRequest(ManifestSource.ManifestURL, UnityWebRequest.kHttpVerbPOST);

            _downloader.uploadHandler = new UploadHandlerRaw(bodyRaw);
            _downloader.downloadHandler = new DownloadHandlerBuffer();
            _downloader.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("(Nest) Request Data: " + packageInfo.fullName);

            yield return _downloader.SendWebRequest();

            DownlandURL downloadUrl = JsonUtility.FromJson<DownlandURL>(_downloader.downloadHandler.text);
            Debug.Log(downloadUrl.download_url);

            _downloader = new UnityWebRequest(downloadUrl.download_url)
            {
                downloadHandler = new DownloadHandlerFile(path),
            };

            _downloader.downloadHandler = new DownloadHandlerBuffer();
            _downloader.SetRequestHeader("Content-Type", "application/json");

            yield return _downloader.SendWebRequest();

            // Pause until download done/cancelled/fails, keeping progress bar up to date.
            while (!_downloader.isDone)
            {
                yield return null;
                var progress = Mathf.FloorToInt(_downloader.downloadProgress * 100);
                if (EditorUtility.DisplayCancelableProgressBar("Nest Package Manager", _activity, progress))
                {
                    Debug.LogWarning("Aborted!");
                    _downloader.Abort();
                }
            }

            Debug.Log("(Nest) Request Data: " + _downloader.responseCode);

            EditorUtility.ClearProgressBar();

            if (string.IsNullOrEmpty(_downloader.error))
            {
                if (Directory.Exists(path))
                {
                    FileUtil.DeleteFileOrDirectory(path);
                }

                File.WriteAllBytes(path, _downloader.downloadHandler.data);
                AssetDatabase.ImportPackage(path, true);

                //  EditorUtility.RevealInFinder(path);
                FileUtil.DeleteFileOrDirectory(path);
            }

            _downloader.Dispose();
            _downloader = null;
            _editorCoroutine = null;

            yield return null;
        }

        private void CancelOperation()
        {
            // Stop any async action taking place.
            if (_downloader != null)
            {
                _downloader.Abort(); // The coroutine should resume and clean up.
                return;
            }

            if (_editorCoroutine != null)
                this.StopCoroutine(_editorCoroutine.routine);

            if (_editorCoroutineSelfUpdate != null)
                this.StopCoroutine(_editorCoroutineSelfUpdate.routine);

            _editorCoroutineSelfUpdate = null;
            _editorCoroutine = null;
            _downloader = null;
            manuallySetActivity = false;
        }

        void DragAndDropFunction()
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                for (int i = 0; i < DragAndDrop.paths.Length; i++)
                {
                    var objPath = DragAndDrop.paths[i];
                    var obj = DragAndDrop.objectReferences[i];
                    var fileName = Path.GetFileName(objPath);
                    Debug.Log(fileName);
                    var extension = Path.GetExtension(objPath);


                    if (!string.Equals(extension, ".unitypackage"))
                    {
                        manuallySetActivity = true;
                        _activity = "Extension must be .unitypackage";
                        return;
                    }

                    string name = Path.GetFileNameWithoutExtension(objPath);
                    string[] temp = name.Split('_');
                    if (temp.Length != 3)
                    {
                        manuallySetActivity = true;
                        _activity = "File name must be Filename_Vx.x.x_Header";
                        return;
                    }

                    if (!temp[1].StartsWith('V'))
                    {
                        manuallySetActivity = true;
                        _activity = "File name must be Filename_Vx.x.x_Header";
                        return;
                    }
                }
            }
        }
    }
}