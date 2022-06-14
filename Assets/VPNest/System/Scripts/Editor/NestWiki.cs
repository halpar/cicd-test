using UnityEditor;
using UnityEngine;

public static class NestWiki 
{
	[MenuItem("Nest/Nest Wiki",false,40)]
	public  static void OpenWiki()
	{
		Application.OpenURL("https://www.notion.so/Nest-Wiki-f1f6d231a9fd4a27825e9c1ada31b915");
	}
}
