using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

namespace VP.Nest.System.Editor
{
	public class BuildPlayer : MonoBehaviour
	{
		[MenuItem("Nest/Build/iOS Release", false, 0)]
		public static void BuildiOSRelease()
		{
			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

			buildPlayerOptions.scenes = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();
			buildPlayerOptions.locationPathName = "Builds/iOSRelease"; //included to gitignore
			buildPlayerOptions.target = BuildTarget.iOS;
			buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;



			BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
			BuildSummary summary = report.summary;

			if (summary.result == BuildResult.Succeeded) {
				EditorUtility.RevealInFinder("Builds/iOSRelease");
				Debug.Log("Build succeeded: " + summary.totalSize + " bytes");

				GameConfigsSO gameConfigsSo = GameConfigsSO.GetGameConfigsSO();
				if (gameConfigsSo) {


					gameConfigsSo.buildNo++;

					gameConfigsSo.ManualValidate();


					AssetDatabase.Refresh();

					EditorUtility.SetDirty(gameConfigsSo);

				}
			}

			if (summary.result == BuildResult.Failed) {
				Debug.Log("Build failed");
			}


		}

		[MenuItem("Nest/Build/iOS Debug", false, 1)]
		public static void BuildiOSDebug()
		{
			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

			buildPlayerOptions.scenes = GameSettings.SelectedSceneLoadSettings.GetUniqueScenesPaths();
			buildPlayerOptions.locationPathName = "Builds/iOSDebug"; //included to gitignore
			buildPlayerOptions.target = BuildTarget.iOS;
			buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging |
				BuildOptions.ConnectWithProfiler;

			BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
			BuildSummary summary = report.summary;

			if (summary.result == BuildResult.Succeeded) {
				EditorUtility.RevealInFinder("Builds/iOSDebug");
				Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
			}

			if (summary.result == BuildResult.Failed) {
				Debug.Log("Build failed");
			}
		}

		[MenuItem("Nest/Build/Open Builds Folder", false, 100)]
		public static void OpenBuildsFolder()
		{
#if UNITY_EDITOR_OSX
			EditorUtility.RevealInFinder("Builds");
#elif UNITY_EDITOR_WIN
            //TODO: Open in File Explorer
#endif
		}
	}
}