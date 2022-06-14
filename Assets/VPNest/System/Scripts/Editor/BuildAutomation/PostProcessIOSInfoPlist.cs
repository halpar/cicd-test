
#if UNITY_IOS
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using SystemIO = System.IO;


namespace VP.Nest.System.Editor
{
    public class PostProcessIOSInfoPlist : MonoBehaviour
    {
        [PostProcessBuildAttribute]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
                return;

            var plistPath = pathToBuiltProject + "/Info.plist";
            
            var file = new List<string>(SystemIO.File.ReadAllLines(plistPath));

            var removeStartIndex = -1;
            var removeLineCount = -1;

            for (var i = 0; i < file.Count; i++)
            {
                if (removeStartIndex == -1)
                {
                    if (file[i].Contains("UIRequiredDeviceCapabilities"))
                    {
                        removeStartIndex = i;
                    }
                }
                else
                {
                    // On iOS, we're looking for the end of the array, while on tvOS
                    // that array is empty marked with a tag with a slash at the end
                    if (file[i].Contains("<array/>") || file[i].Contains("</array>"))
                    {
                        removeLineCount = i - removeStartIndex + 1; //+1 for current line
                        break;
                    }
                }
            }

            file.RemoveRange(removeStartIndex, removeLineCount);
            SystemIO.File.WriteAllLines(plistPath, file.ToArray());
        }
    }
}
#endif