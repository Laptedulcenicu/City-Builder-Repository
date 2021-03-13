using System.IO;
using System.Linq;
using GameRig.Scripts.Systems.UpgradesSystem;
using GameRig.Scripts.Utilities.Progressions;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	public static class UpgradesUtility
	{
		private static string upgradePath;
		private static string valuePath;
		private static string pricePath;
		private static string valueProgressionPath;
		private static string priceProgressionPath;

		private static bool isInitialized;
		private static UpgradeSettings selectedSetting;

		public static UpgradeSettings[] UpgradeSettingsList { get; private set; }

		public static UpgradeSettings SelectedSetting
		{
			get
			{
				if (selectedSetting == null)
				{
					if (UpgradeSettingsList.Length > 0)
						selectedSetting = UpgradeSettingsList[0];
				}

				return selectedSetting;
			}

			set => selectedSetting = value;
		}

		private static string GetProjectName()
		{
			string[] s = Application.dataPath.Split('/');
			string projectName = s[s.Length - 2];
			projectName = "_" + projectName.Replace('_', ' ');
			return projectName;
		}

		private static void PathsValidation()
		{
			string projectName = GetProjectName();

			upgradePath = "Assets/" + projectName + "/Resources/Upgrade Settings";
			valuePath = "Assets/" + projectName + "/Resources/Upgrade Settings/Value Settings";
			pricePath = "Assets/" + projectName + "/Resources/Upgrade Settings/Price Settings";
			valueProgressionPath = "Assets/" + projectName + "/Resources/Upgrade Settings/Value Progressions";
			priceProgressionPath = "Assets/" + projectName + "/Resources/Upgrade Settings/Price Progressions";
		}

		private static void FoldersValidation()
		{
			if (!Directory.Exists(upgradePath)) Directory.CreateDirectory(upgradePath);
			if (!Directory.Exists(valuePath)) Directory.CreateDirectory(valuePath);
			if (!Directory.Exists(pricePath)) Directory.CreateDirectory(pricePath);
			if (!Directory.Exists(valueProgressionPath)) Directory.CreateDirectory(valueProgressionPath);
			if (!Directory.Exists(priceProgressionPath)) Directory.CreateDirectory(priceProgressionPath);
		}

		private static bool NameValidation(string name)
		{
			return UpgradeSettingsList.All(upgrade => upgrade.UpgradeName != name);
		}

		public static void UpdateUpgradeSettingsList()
		{
			UpgradeSettingsList = Resources.LoadAll<UpgradeSettings>("Upgrade Settings");
		}

		public static void Initialize()
		{
			if (isInitialized)
				return;

			PathsValidation();

			isInitialized = true;
		}

		public static void CreateUpgradeSetting(string upgradeName)
		{
			if (string.IsNullOrEmpty(upgradeName))
			{
				Debug.Log("Enter upgrade name first!");
				return;
			}

			if (!NameValidation(upgradeName))
			{
				Debug.Log("Upgrade Settings with name " + upgradeName + " already exists!");
				return;
			}

			FoldersValidation();

			UpgradeSettings upgrade = CreateAsset<UpgradeSettings>(upgradePath, "Upgrade " + upgradeName);
			UpgradeValueSettings value = CreateAsset<UpgradeValueSettings>(valuePath, "Value " + upgradeName);
			UpgradePriceSettings price = CreateAsset<UpgradePriceSettings>(pricePath, "Price " + upgradeName);
			ArithmeticProgression valueProgression = CreateAsset<ArithmeticProgression>(valueProgressionPath, "Value Progression " + upgradeName);
			GeometricProgression priceProgression = CreateAsset<GeometricProgression>(priceProgressionPath, "Price Progression " + upgradeName);

			EditorUtility.SetDirty(upgrade);
			EditorUtility.SetDirty(value);
			EditorUtility.SetDirty(price);
			EditorUtility.SetDirty(valueProgression);
			EditorUtility.SetDirty(priceProgression);

			SerializedObject serializedUpgrade = new SerializedObject(upgrade);
			serializedUpgrade.FindProperty("upgradeName").stringValue = upgradeName;
			SerializedProperty valueProgressionArray = serializedUpgrade.FindProperty("valueProgressions");
			valueProgressionArray.arraySize++;
			serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();
			valueProgressionArray.GetArrayElementAtIndex(0).objectReferenceValue = value;
			serializedUpgrade.FindProperty("priceSettings").objectReferenceValue = price;
			serializedUpgrade.FindProperty("upgradeSaveKey").stringValue = upgradeName.GetHashCode().ToString("X");
			serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();

			SerializedObject serializedValue = new SerializedObject(value);
			serializedValue.FindProperty("valueProgression").objectReferenceValue = valueProgression;
			serializedValue.ApplyModifiedPropertiesWithoutUndo();

			SerializedObject serializedPrice = new SerializedObject(price);
			serializedPrice.FindProperty("priceProgression").objectReferenceValue = priceProgression;
			serializedPrice.ApplyModifiedPropertiesWithoutUndo();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			UpdateUpgradeSettingsList();
		}

		public static void DeleteSelectedUpgrade()
		{
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting.PriceSettings.PriceProgression));
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting.PriceSettings));
			foreach (var selectedSettingUpgradeValueProgression in SelectedSetting.UpgradeValueProgressions)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSettingUpgradeValueProgression.ValueProgression));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSettingUpgradeValueProgression));
			}

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting));

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			UpdateUpgradeSettingsList();
		}

		public static void ChangeProgressionType(UpgradeValueSettings upgradeValueSettings, ProgressionType progressionType)
		{
			string previousName = upgradeValueSettings.ValueProgression.name;
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings.ValueProgression));

			BaseProgression valueProgression;
			if (progressionType == ProgressionType.Arithmetic)
				valueProgression = CreateAsset<ArithmeticProgression>(valueProgressionPath, previousName);
			else
				valueProgression = CreateAsset<GeometricProgression>(valueProgressionPath, previousName);

			EditorUtility.SetDirty(upgradeValueSettings);

			SerializedObject serializedUpgradeValueSettings = new SerializedObject(upgradeValueSettings);
			serializedUpgradeValueSettings.FindProperty("valueProgression").objectReferenceValue = valueProgression;
			serializedUpgradeValueSettings.ApplyModifiedPropertiesWithoutUndo();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void ChangeProgressionType(UpgradePriceSettings upgradePriceSettings, ProgressionType progressionType)
		{
			string previousName = upgradePriceSettings.PriceProgression.name;

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradePriceSettings.PriceProgression));
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			BaseProgression valueProgression;
			if (progressionType == ProgressionType.Arithmetic)
				valueProgression = CreateAsset<ArithmeticProgression>(valueProgressionPath, previousName);
			else
				valueProgression = CreateAsset<GeometricProgression>(valueProgressionPath, previousName);

			EditorUtility.SetDirty(upgradePriceSettings);

			SerializedObject serializedUpgradePriceSettings = new SerializedObject(upgradePriceSettings);
			serializedUpgradePriceSettings.FindProperty("priceProgression").objectReferenceValue = valueProgression;
			serializedUpgradePriceSettings.ApplyModifiedPropertiesWithoutUndo();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void AddValueSettings(UpgradeSettings upgradeSettings)
		{
			EditorUtility.SetDirty(upgradeSettings);

			ArithmeticProgression valueProgression = CreateAsset<ArithmeticProgression>
				(valueProgressionPath, "Value Progression " + upgradeSettings.UpgradeName + (upgradeSettings.UpgradeValueProgressions.Length + 1));

			UpgradeValueSettings value = CreateAsset<UpgradeValueSettings>
				(valuePath, "Value " + upgradeSettings.UpgradeName + (upgradeSettings.UpgradeValueProgressions.Length + 1));

			EditorUtility.SetDirty(value);
			EditorUtility.SetDirty(upgradeSettings);

			SerializedObject serializedValue = new SerializedObject(value);
			serializedValue.FindProperty("valueProgression").objectReferenceValue = valueProgression;
			serializedValue.ApplyModifiedPropertiesWithoutUndo();

			SerializedObject serializedUpgrade = new SerializedObject(upgradeSettings);
			SerializedProperty valueProgressionArray = serializedUpgrade.FindProperty("valueProgressions");
			valueProgressionArray.arraySize++;
			serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();
			valueProgressionArray.GetArrayElementAtIndex(valueProgressionArray.arraySize - 1).objectReferenceValue = value;
			serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void RemoveMissingValueSettings()
		{
			SerializedObject serializedUpgrade = new SerializedObject(SelectedSetting);
			SerializedProperty valueProgressionArray = serializedUpgrade.FindProperty("valueProgressions");
			int valueProgressionCount = valueProgressionArray.arraySize;
			for (int i = 0; i < valueProgressionCount; i++)
			{
				if (valueProgressionArray.GetArrayElementAtIndex(i).objectReferenceValue == null)
				{
					valueProgressionArray.DeleteArrayElementAtIndex(i);
					serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();
					valueProgressionArray.DeleteArrayElementAtIndex(i);
					break;
				}
			}

			serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();
		}

		public static void RemoveValueSettings(UpgradeValueSettings upgradeValueSettings)
		{
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings.ValueProgression));
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings));

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private static T CreateAsset<T>(string path, string name) where T : ScriptableObject
		{
			if (string.IsNullOrEmpty(path))
			{
				path = "Assets";
			}

			if (!name.EndsWith(".asset"))
			{
				name += ".asset";
			}

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name);

			var asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset, assetPathAndName);
			AssetDatabase.SaveAssets();

			return asset;
		}
	}
}