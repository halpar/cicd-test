using UnityEditor;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    public static class TemplatePrefabBuilder
    {
        [MenuItem("Assets/Create/Empty Prefab", priority = 0)]
        private static void CreateEmptyPrefab()
        {
            const string name = "NewPrefab";
            string projectWindowPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            
            GameObject newPrefab = BuildEmptyGameObject(name);
            GameObject createdPrefab = SavePrefabAsset(newPrefab, projectWindowPath);
            
            Object.DestroyImmediate(newPrefab);
            SetContextToRename(createdPrefab);
        }

        private static GameObject SavePrefabAsset(GameObject obj, string projectWindowPath)
        {
            return PrefabUtility.SaveAsPrefabAsset(obj, projectWindowPath + '/' + obj.name + ".prefab");
        }
        
        private static GameObject BuildEmptyGameObject(string name)
        {
            GameObject prefabParent = new GameObject(name);

            GameObject modelHolder = new GameObject("ModelHolder");
            GameObject colliderHolder = new GameObject("Colliders");
            GameObject triggers = new GameObject("Triggers");
            GameObject particles = new GameObject("Particles");
            
            modelHolder.transform.parent = prefabParent.transform;
            colliderHolder.transform.parent = prefabParent.transform;
            triggers.transform.parent = prefabParent.transform;
            particles.transform.parent = prefabParent.transform;

            return prefabParent;
        }

        private static void SetContextToRename(Object obj)
        {
            Selection.SetActiveObjectWithContext(obj, obj);
            Selection.selectionChanged += SendRenameEvent;
        }
        
        private static void SendRenameEvent()
        {
            Selection.selectionChanged -= SendRenameEvent;
            Event rename = new Event() { keyCode = KeyCode.Return, type = EventType.KeyDown };
            EditorWindow.focusedWindow.SendEvent(rename);
        }
    }
}
