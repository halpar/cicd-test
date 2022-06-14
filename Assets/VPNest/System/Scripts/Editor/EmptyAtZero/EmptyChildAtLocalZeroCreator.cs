using UnityEditor;

namespace VP.Nest.System.Editor.EmptyAtZero
{
    public static class EmptyChildAtLocalZeroCreator
    {
        private const string Space = EmptyCreator.Space;

        private const string Local = "Local";
        private const string FeatureName = EmptyCreator.CreateEmptyChildAt + Local + Space + EmptyCreator.Zero;

        private const string PathName =
            EmptyCreator.GameObjectStr + EmptyCreator.Slash + FeatureName + Space + ShortcutName;

        private const string ShortcutName = EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol +
                                            EmptyCreator.ShiftSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(PathName, false)]
        public static void CreateEmptyChildAtLocalZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(FeatureName, false, true, menuCommand);
        }
    }
}