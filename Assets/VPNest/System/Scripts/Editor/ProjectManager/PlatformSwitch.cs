using UnityEditor;
using UnityEditor.Build;

namespace VP.Nest.System.Editor
{
    public class SwitchPlatformExample : IActiveBuildTargetChanged
    {
        public int callbackOrder => 0;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            if (newTarget == BuildTarget.iOS || newTarget == BuildTarget.Android)
            {
                ProjectManager.ContinueSettingsAfterPlatformSwitch(newTarget);
            }
        }
    }
}