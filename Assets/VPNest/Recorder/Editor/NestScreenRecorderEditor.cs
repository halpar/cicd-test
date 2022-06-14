using System.IO;
using UnityEditor;
using UnityEngine;
using VP.Nest.System.NestScreenRecorder;

[CustomEditor(typeof(NestScreenRecorder))]
[CanEditMultipleObjects]
public class NestScreenRecorderEditor : Editor
{
    private Rect screenRect;
    private Rect vertRect;

    private NestScreenRecorder nestScreenRecorder;

    private void Awake()
    {
        nestScreenRecorder = NestScreenRecorder.Instance;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(5);

        screenRect = GUILayoutUtility.GetRect(1, 1);
        vertRect = EditorGUILayout.BeginVertical();

        if (Application.isPlaying)
        {
            if (nestScreenRecorder != null)
            {
                if (nestScreenRecorder.IsDeviceSimulatorRunning == false)
                {
                    if (nestScreenRecorder.IsRecording == true)
                    {
                        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 25, vertRect.height + 14), Color.red);
                        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                        EditorGUILayout.LabelField("Recording...", style);
                        Repaint();
                    }
                    else
                    {
                        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 25, vertRect.height + 14), Color.green);
                        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                        EditorGUILayout.LabelField("Ready for recording.", style);
                        Repaint();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Please switch to Gameview. Recorder can not work with Simulator.", MessageType.Error);
                    Repaint();
                }
                
            }               
        }
        else
        {
            EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 25, vertRect.height + 14), Color.yellow);
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField("Waiting for play mode...", style);
        }
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Open Recordings Folder"))
        {
            if (!Directory.Exists("Recordings"))
            {
                Debug.LogError($"{GetType().Name} -> Recordings folder is empty! Please record something first.");
            }
            else
            {
                EditorUtility.RevealInFinder("Recordings");
            }
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("How to use?");
        EditorGUILayout.HelpBox("Press 0 to take screenshots while game is running", MessageType.Info);
        EditorGUILayout.HelpBox("Press 9 to start/stop video recording while game is running", MessageType.Info);
    }
}