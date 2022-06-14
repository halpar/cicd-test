#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace VP.Nest
{
    [CustomEditor(typeof(GameConfigsSO))]
    public class GameConfigsEditor : Editor
    {
        private SerializedProperty companyName, teamID, productName, bundleID, elephantGameID, elephantGameSecret, facebookAppID, version, buildNo, icon;

        private void OnEnable()
        {
            companyName = serializedObject.FindProperty("companyName");
            teamID = serializedObject.FindProperty("teamID");
            productName = serializedObject.FindProperty("productName");
            bundleID = serializedObject.FindProperty("bundleID");
            facebookAppID = serializedObject.FindProperty("facebookAppID");
            elephantGameID = serializedObject.FindProperty("elephantGameID");
            elephantGameSecret = serializedObject.FindProperty("elephantGameSecret");
            version = serializedObject.FindProperty("version");
            buildNo = serializedObject.FindProperty("buildNo");
            icon = serializedObject.FindProperty("icon");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Screen();

            serializedObject.ApplyModifiedProperties();
        }

        private void Screen()
        {
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("Company Settings", EditorStyles.boldLabel);


            //  companyName.stringValue = EditorGUILayout.TextField("Company Name:", companyName.stringValue);
            // teamID.stringValue = EditorGUILayout.TextField("Team ID:", teamID.stringValue);

            EditorGUILayout.LabelField("Company Name:", companyName.stringValue);
            EditorGUILayout.LabelField("Team ID:", teamID.stringValue);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Product Settings", EditorStyles.boldLabel);
            productName.stringValue = EditorGUILayout.TextField("Product Name:", productName.stringValue);
            bundleID.stringValue = EditorGUILayout.TextField("Bundle ID:", bundleID.stringValue);
            elephantGameID.stringValue = EditorGUILayout.TextField("Elephant Game ID:", elephantGameID.stringValue);
            elephantGameSecret.stringValue = EditorGUILayout.TextField("Elephant Game Secret:", elephantGameSecret.stringValue);
            facebookAppID.stringValue = EditorGUILayout.TextField("Facebook App ID:", facebookAppID.stringValue);

            GUILayout.BeginHorizontal(GUILayout.Width(100));
            version.stringValue = EditorGUILayout.TextField("Version/BuildNo:", version.stringValue);
            buildNo.intValue = EditorGUILayout.IntField(buildNo.intValue, GUILayout.Width(25));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            var IconLayoutSize = EditorGUIUtility.currentViewWidth * 0.95f;
            icon.objectReferenceValue =
                EditorGUILayout.ObjectField("Icon:", icon.objectReferenceValue, typeof(Texture2D), true,
                    new GUILayoutOption[]
                    {
                        GUILayout.Height(IconLayoutSize),
                        GUILayout.Width(IconLayoutSize)
                    });

            GUILayout.EndVertical();
        }
    }
}