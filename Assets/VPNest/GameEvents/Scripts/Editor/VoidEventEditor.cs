#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using VPNest.GameEvents.Scripts.Events;

namespace VPNest.GameEvents.Scripts.Editor
{
    [CustomEditor(typeof(VoidEvent))]
    public class VoidEventEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var e = target as VoidEvent;
            
            if (GUILayout.Button("Raise"))
            {
                if(e != null) e.Raise();
            }
        }
    }
}

#endif
