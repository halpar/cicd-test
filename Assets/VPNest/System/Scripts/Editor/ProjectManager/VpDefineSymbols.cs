using System;
using System.Linq;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

#endif

namespace VP.Nest.System.Editor
{
#if UNITY_EDITOR

	[InitializeOnLoad]
	public class VpDefineSymbols : UnityEditor.Editor
	{
		private const string VpFbExists = "VP_FB_EXISTS";
		private const string VpElephantExists = "VP_ELEPHANT_EXISTS";
		//private const string VpByrdExists = "VP_BYRD_EXISTS";

		private static readonly List<string> Symbols = new List<string>();

		static VpDefineSymbols()
		{
			if (CheckSdkStatus("Facebook.Unity.Settings")) Symbols.Add("VP_FB_EXISTS");
			if (CheckSdkStatus("ElephantSDK")) Symbols.Add("VP_ELEPHANT_EXISTS");
			//if (CheckSdkStatus("ByrdSDK")) Symbols.Add("VP_BYRD_EXISTS");

			var scriptingDefinesString =
				PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var scriptingDefinesStringList = scriptingDefinesString.Split(';').ToList();

			if (scriptingDefinesStringList.Contains(VpFbExists) && !Symbols.Contains(VpFbExists)) {
				scriptingDefinesStringList.Remove(VpFbExists);
			}

			if (scriptingDefinesStringList.Contains(VpElephantExists) && !Symbols.Contains(VpElephantExists)) {
				scriptingDefinesStringList.Remove(VpElephantExists);
			}

			// if (scriptingDefinesStringList.Contains(VpByrdExists) && !Symbols.Contains(VpByrdExists)) {
			// 	scriptingDefinesStringList.Remove(VpByrdExists);
			// }

			scriptingDefinesStringList.AddRange(Symbols.Except(scriptingDefinesStringList));

			PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
				string.Join(";", scriptingDefinesStringList.ToArray()));
		}

		private static bool CheckSdkStatus(string desiredNamespace)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Any(assembly => assembly.GetTypes().Any(type => type.Namespace == desiredNamespace));
		}
	}
#endif
}