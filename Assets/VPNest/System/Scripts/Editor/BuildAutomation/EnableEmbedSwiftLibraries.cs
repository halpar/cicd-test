using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace VP.Nest.System.Editor
{
    public static class EnableEmbedSwiftLibraries
    {
        [PostProcessBuild(1000)]
        public static void PostProcessBuildAttribute(BuildTarget target, string pathToBuildProject)
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
                pbxProject.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
                pbxProject.WriteToFile(projectPath);

                var projectInString = File.ReadAllText(projectPath);

                projectInString = projectInString.Replace("ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES = NO;",
                    $"ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES = YES;");
                File.WriteAllText(projectPath, projectInString);
            }
        }
    }
}