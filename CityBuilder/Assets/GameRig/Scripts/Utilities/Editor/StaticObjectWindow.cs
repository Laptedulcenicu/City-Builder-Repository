using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Utilities.Editor
{
	public class StaticObjectWindow : EditorWindow
	{
		internal struct ClassPathData
		{
			public string ClassAssetName;
			public string ClassName;
			public string ClassPath;
			public bool HasVariables;
			public List<FieldInfo> AllStaticFields;
			public ClassButtonState ClassButtonState;
		}

		internal class ClassButtonState
		{
			public AnimBool AnimBool;
			public bool IsActive;
		}

		private StaticDataFinder findStatic;

		private Dictionary<string, ClassPathData> classesDataDictionary;
		private HashSet<string> assetsNamesHashSet;

		private bool drawEmptyClass;
		private int displayAssets;

		private Vector2 scrollPosition = Vector2.zero;

		[MenuItem("GameRig/Tools/Static Debugger", priority = 20)]
		public static void CreateObserverWindow()
		{
			StaticObjectWindow window = CreateWindow<StaticObjectWindow>();
			window.titleContent = new GUIContent("Static Debugger");
			window.minSize = new Vector2(450f, 500f);
			window.Show();
		}

		private void OnEnable()
		{
			ResetData();
		}

		private void ResetData()
		{
			findStatic = new StaticDataFinder();
			drawEmptyClass = false;
			displayAssets = 1;

			GetAllAssetsNames();
		}

		private void GetAllAssetsNames()
		{
			assetsNamesHashSet = new HashSet<string>();
			classesDataDictionary = new Dictionary<string, ClassPathData>();

			foreach (StaticDataFinder.StaticClassInfo st in findStatic.GetAllStaticClassesInfo())
			{
				string[] classPathSplit = st.ClassType.ToString().Split('.');
				string classAssetName = classPathSplit[0];
				string className = classPathSplit.Last();

				if (!classesDataDictionary.ContainsKey(className))
				{
					ClassButtonState classButtonState = new ClassButtonState
					{
						IsActive = false,
						AnimBool = new AnimBool(false)
					};

					classButtonState.AnimBool.valueChanged.AddListener(Repaint);

					ClassPathData classPathData = new ClassPathData
					{
						ClassName = className,
						ClassAssetName = classAssetName,
						ClassPath = st.Path,
						HasVariables = st.AllStaticFields.Count > 0,
						AllStaticFields = st.AllStaticFields,
						ClassButtonState = classButtonState
					};

					classesDataDictionary.Add(className, classPathData);
				}

				assetsNamesHashSet.Add(classAssetName);
			}
		}

		private void OnGUI()
		{
			DrawTopContent();

			DrawUILine(Color.gray);

			DrawMiddleContent();

			GUILayout.FlexibleSpace();
		}

		private void Update()
		{
			if (EditorApplication.isPlaying && !EditorApplication.isPaused)
			{
				Repaint();
			}
		}

		private void DrawTopContent()
		{
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Display assets", EditorStyles.boldLabel, GUILayout.Width(200f));
			GUILayout.Space(25f);
			displayAssets = EditorGUILayout.MaskField(displayAssets, assetsNamesHashSet.ToArray(), GUILayout.Width(150f));

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Draw class without variables", EditorStyles.boldLabel, GUILayout.Width(200f));
			GUILayout.Space(25f);
			drawEmptyClass = EditorGUILayout.Toggle(drawEmptyClass, GUILayout.Width(15f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		private void DrawMiddleContent()
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
			EditorGUIUtility.wideMode = true;

			if (1 << displayAssets < 0)
			{
				DrawSelectedAssetClasses("", true);
			}
			else
			{
				string[] assetsNames = assetsNamesHashSet.ToArray();
				for (int i = 0; i < assetsNames.Length; i++)
				{
					if (1 << i == displayAssets)
					{
						DrawSelectedAssetClasses(assetsNames[i], false);
					}
				}
			}

			GUILayout.EndScrollView();
		}

		private void DrawSelectedAssetClasses(string assetName, bool drawAllAssets)
		{
			foreach (ClassPathData classPathData in classesDataDictionary.Values)
			{
				if (!drawAllAssets)
				{
					if (classPathData.ClassAssetName != assetName)
						continue;
				}

				if (drawEmptyClass)
				{
					DrawStaticClasses(classPathData);
				}
				else
				{
					if (classPathData.HasVariables)
					{
						DrawStaticClasses(classPathData);
					}
				}
			}
		}

		private void DrawStaticClasses(ClassPathData classPathData)
		{
			GUILayout.Space(5f);

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton);

			GUILayout.Space(5f);

			EditorGUILayout.BeginHorizontal();

			GUIStyle style = new GUIStyle("label")
			{
				normal = new GUIStyleState
				{
					textColor = new Color(0.29f, 0.55f, 1f)
				}
			};

			GUILayout.Label(classPathData.ClassAssetName, style);
			GUILayout.Label(classPathData.ClassName, "label");

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Open Class", EditorStyles.miniButtonMid, GUILayout.Width(90f)))
			{
				Object obj = AssetDatabase.LoadAssetAtPath<Object>(classPathData.ClassPath);
				int codeLine = 0;
				AssetDatabase.OpenAsset(obj, codeLine);
			}

			GUILayout.Space(5f);

			EditorGUILayout.EndHorizontal();

			if (classPathData.HasVariables)
			{
				if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					classPathData.ClassButtonState.IsActive = !classPathData.ClassButtonState.IsActive;
					classPathData.ClassButtonState.AnimBool.target = classPathData.ClassButtonState.IsActive;
				}
			}

			DrawStaticVariablesContainer(classPathData);
		}

		private void DrawStaticVariablesContainer(ClassPathData classPathData)
		{
			if (EditorGUILayout.BeginFadeGroup(classPathData.ClassButtonState.AnimBool.faded))
			{
				EditorGUILayout.BeginVertical("box");

				DrawFoldedStaticVariables(classPathData);

				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndFadeGroup();
		}

		private void DrawFoldedStaticVariables(ClassPathData classPathData)
		{
			GUIStyle styleVariable = new GUIStyle("label")
			{
				normal = new GUIStyleState
				{
					textColor = new Color(0.22f, 1f, 0.61f)
				}
			};

			foreach (FieldInfo v in classPathData.AllStaticFields)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(20f);

				string variableNameSimplified = VariableNameSimplified(v.Name);

				GUILayout.Label(variableNameSimplified, "label", GUILayout.MaxWidth(200f));
				GUILayout.Label("=", "label", GUILayout.Width(50f));
				GUILayout.Label(v.GetValue(null).ToString(), styleVariable);

				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel--;
			}
		}

		private static string VariableNameSimplified(string variableName)
		{
			string[] separatingStrings = {"<", ">"};
			string variableNameSimplified = variableName.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries)[0];
			return variableNameSimplified;
		}

		public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height = thickness;
			r.y += padding / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, color);
		}
	}
}