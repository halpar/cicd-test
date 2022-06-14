using UnityEditor;

namespace VP.Nest.System.Editor.EmptyAtZero
{
    public static class EmptyChildAtGlobalZeroCreator
    {
        private const string Space = EmptyCreator.Space;

        private const string Global = "Global";
        private const string FeatureName = EmptyCreator.CreateEmptyChildAt + Global + Space + EmptyCreator.Zero;

        private const string PathName =
            EmptyCreator.GameObjectStr + EmptyCreator.Slash + FeatureName + Space + ShortcutName;

        private const string ShortcutName =
            EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(PathName, false)]
        public static void CreateEmptyChildAtGlobalZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(FeatureName, false, false, menuCommand);
        }
    }
}