using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VP.Nest
{
	//[CustomEditor(typeof(GameConfigsSO))]
	//public class GameConfigsInspector : Editor
	//{


	//	// Start is called before the first frame update
	//	GameConfigsSO comp;
	//	static bool showTileEditor = false;

	//	public void OnEnable()
	//	{
	//		comp = (GameConfigsSO)target;

	//	}

	//	public override void OnInspectorGUI()
	//	{
	//		//MAP DEFAULT INFORMATION


	//		//WIDTH - HEIGHT

	//		DrawDefaultInspector();

	//		serializedObject.Update();

	//		// Automatically uses the according PropertyDrawer for the type
	//		EditorGUILayout.PropertyField(icon);

	//		if (comp.icon == null) {
	//			EditorGUILayout.BeginHorizontal();
	//			comp.icon = (Texture2D)EditorGUILayout.ObjectField(comp.icon, typeof(Texture2D), false, GUILayout.Width(65f), GUILayout.Height(65f));
	//			EditorGUILayout.EndHorizontal();

	//		}
	//		EditorUtility.SetDirty(comp);

	//	}

	//}
}
