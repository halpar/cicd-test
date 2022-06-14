using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace VP.Nest.System.Editor
{
    public class DisableSplashScreen : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_PRO_LICENSE
            PlayerSettings.SplashScreen.show = false;
#endif
        }
    }
}