using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace VP.Nest.System.Editor
{
    public static class DeleteFrameworkIOS
    {
        [PostProcessBuild(1000)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
        {
            if (target == BuildTarget.iOS)
            {
                var projectPath = PBXProject.GetPBXProjectPath(pathToBuildProject);

                var pbxProject = new PBXProject();
                pbxProject.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
                var targetGuid = pbxProject.GetUnityMainTargetGuid();
#else
            var targetName = PBXProject.GetUnityTargetName();
            var targetGuid = pbxProject.TargetGuidByName(targetName);
#endif
                string shellScriptPath = Path.GetDirectoryName(Application.dataPath) + "/Assets/VPNest/System/Scripts/Editor/BuildAutomation/DeleteFrameworkXCode.sh";


                pbxProject.AddShellScriptBuildPhase(targetGuid,"Run Script DeleteFrameworkXCode.sh", "/bin/sh", shellScriptPath);

                pbxProject.WriteToFile(projectPath);
            }
        }
    }
}