using UnityEditor;

namespace VP.Nest.System.Editor.EmptyAtZero
{
    public static class EmptyAtZeroCreator
    {
        private const string Space = EmptyCreator.Space;

        private const string FeatureName = EmptyCreator.CreateEmpty + EmptyCreator.At + Space + EmptyCreator.Zero;

        private const string PathName =
            EmptyCreator.GameObjectStr + EmptyCreator.Slash + FeatureName + Space + ShortcutName;

        private const string ShortcutName = EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(PathName, false, -1)]
        public static void CreateEmptyAtZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(FeatureName, true, false, menuCommand);
        }
    }
}