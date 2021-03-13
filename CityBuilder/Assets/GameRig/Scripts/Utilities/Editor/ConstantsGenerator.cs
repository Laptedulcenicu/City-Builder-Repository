using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	public static class ConstantsGenerator
	{
		[MenuItem("Assets/Refresh Constants")]
		[MenuItem("GameRig/Tools/Refresh Constants", priority = 5)]
		private static void Generate()
		{
			GenerateTags();
			GenerateLayers();
		}

		private static void GenerateTags()
		{
			string[] tags = InternalEditorUtility.tags;

			string fileContents = "namespace GameRig.Scripts.Utilities.ConstantValues\n{\n\t/// <summary>\n\t/// This class stores generated Tags\n\t/// </summary>\n\tpublic static class Tags\n\t{\n";

			foreach (string tag in tags)
			{
				fileContents += "\t\tpublic const string " + ToPascalCase(tag) + " = \"" + tag + "\";\n";
			}

			fileContents += "\t}\n}";

			WriteToFile(fileContents, "Assets\\GameRig\\Scripts\\Utilities\\ConstantValues\\Tags.cs");
		}

		private static void GenerateLayers()
		{
			string fileContents =
				"namespace GameRig.Scripts.Utilities.ConstantValues\n{\n\t/// <summary>\n\t/// This class stores generated Layers\n\t/// </summary>\n\tpublic static class Layers\n\t{\n";

			for (int i = 0; i <= 31; i++)
			{
				string layerName = LayerMask.LayerToName(i);

				if (layerName.Length > 0)
				{
					fileContents += "\t\tpublic const string " + ToPascalCase(layerName) + " = \"" + layerName + "\";\n";
				}
			}

			fileContents += "\t}\n}";

			WriteToFile(fileContents, "Assets\\GameRig\\Scripts\\Utilities\\ConstantValues\\Layers.cs");
		}

		private static string ToPascalCase(string s)
		{
			return Regex.Replace(s, "(\\B[A-Z])", " $1").Replace(" ", "");
		}

		private static void WriteToFile(string content, string path)
		{
			File.WriteAllText(path, content);
			AssetDatabase.Refresh();
		}
	}
}