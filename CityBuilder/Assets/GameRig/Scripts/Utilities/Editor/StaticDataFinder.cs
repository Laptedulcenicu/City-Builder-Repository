using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace GameRig.Scripts.Utilities.Editor
{
	public class StaticDataFinder
	{
		private readonly List<StaticClassInfo> allStaticClassInfo = new List<StaticClassInfo>();

		public StaticDataFinder()
		{
			IEnumerable<Type> staticObjects = GetAllStaticClasses();
			string[] allProjectPaths = AssetDatabase.GetAllAssetPaths();

			foreach (Type obj in staticObjects)
			{
				foreach (string path in allProjectPaths)
				{
					Match text = Regex.Match(path, obj.Name + ".cs");
					if (!string.IsNullOrEmpty(text.ToString()))
					{
						List<FieldInfo> statClassFields = GetAllStaticFieldsInClass(obj);
						allStaticClassInfo.Add(new StaticClassInfo(path, obj, statClassFields));
					}
				}
			}
		}

		public IEnumerable<StaticClassInfo> GetAllStaticClassesInfo()
		{
			return allStaticClassInfo;
		}

		public static IEnumerable<Type> GetAllStaticClasses()
		{
			return from t in Assembly.Load("Assembly-CSharp").GetTypes().Where(t => t.IsClass && t.IsSealed && t.IsAbstract) select t;
		}

		public static List<FieldInfo> GetAllStaticFieldsInClass(Type classToParse)
		{
			IEnumerable<FieldInfo>
				allFields = from t in classToParse.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(t => t.FieldType.IsValueType)
					select t;

			return allFields.ToList();
		}

		public class StaticClassInfo
		{
			public StaticClassInfo(string classPath, Type type, List<FieldInfo> allStatFields)
			{
				Path = classPath;
				ClassType = type;
				AllStaticFields = allStatFields;
			}

			public readonly string Path;
			public List<FieldInfo> AllStaticFields;
			public readonly Type ClassType;
		}
	}
}