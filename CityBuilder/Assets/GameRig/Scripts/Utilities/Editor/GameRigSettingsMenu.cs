using GameRig.Scripts.Systems.AdvertisingSystem;
using GameRig.Scripts.Systems.AnalyticsSystem;
using GameRig.Scripts.Systems.CreativeSystem;
using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Systems.GraphicsSystem;
using GameRig.Scripts.Systems.OfflineSystem;
using GameRig.Scripts.Systems.PoolingSystem;
using GameRig.Scripts.Systems.RankingSystem;
using GameRig.Scripts.Systems.StoreReviewSystem;
using GameRig.Scripts.Systems.UpgradesSystem;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// This class is used to offer a top bar option with shortcuts to all GameRig resources 
	/// </summary>
	public static class GameRigSettingsMenu
	{
		[MenuItem("GameRig/Settings/URP Settings Folder", priority = 0)]
		private static void OpenUniversalRenderPipelineSettings()
		{
		//	UniversalRenderPipelineAsset[] urpSettings = Resources.LoadAll<UniversalRenderPipelineAsset>(GameRigResourcesPaths.UniversalRenderPipelineSettings);

		//	if (urpSettings.Length > 0)
		//	{
		//		Selection.activeObject = urpSettings[0];
		//	}
		}

		[MenuItem("GameRig/Settings/Graphics Settings Folder", priority = 1)]
		private static void OpenGraphicsSettings()
		{
			GraphicsQualitySettings[] graphicsSettings = Resources.LoadAll<GraphicsQualitySettings>(GameRigResourcesPaths.GraphicsSettings);

			if (graphicsSettings.Length > 0)
			{
				Selection.activeObject = graphicsSettings[0];
			}
		}

		[MenuItem("GameRig/Settings/Upgrades Settings", priority = 2)]
		private static void OpenUpgradesSettings()
		{
			SelectSettings<UpgradesSystemSettings>(GameRigResourcesPaths.UpgradeSystemSettings);
		}

		[MenuItem("GameRig/Settings/Pooling Settings", priority = 3)]
		private static void OpenPoolingSettings()
		{
			SelectSettings<PoolingSettings>(GameRigResourcesPaths.PoolingSettings);
		}

		[MenuItem("GameRig/Settings/Offline Earnings Settings", priority = 50)]
		private static void OpenOfflineEarningsSettings()
		{
			SelectSettings<OfflineSettings>(GameRigResourcesPaths.OfflineSettings);
		}

		[MenuItem("GameRig/Settings/Currency Display Settings", priority = 51)]
		private static void OpenCurrencyDisplaySettings()
		{
			SelectSettings<GameCurrencySettings>(GameRigResourcesPaths.GameCurrencySettings);
		}

		[MenuItem("GameRig/Settings/Ranking Settings Folder", priority = 52)]
		private static void OpenRankingSettings()
		{
			RankSettings[] rankSettings = Resources.LoadAll<RankSettings>(GameRigResourcesPaths.Ranks);

			if (rankSettings.Length > 0)
			{
				Selection.activeObject = rankSettings[0];
			}
		}

		[MenuItem("GameRig/Settings/Store Review Settings", priority = 53)]
		private static void OpenStoreReviewSettings()
		{
			SelectSettings<StoreReviewSettings>(GameRigResourcesPaths.StoreReviewSettings);
		}

		[MenuItem("GameRig/Settings/Analytics Settings", priority = 100)]
		private static void OpenAnalyticsSettings()
		{
			SelectSettings<AnalyticsSettings>(GameRigResourcesPaths.AnalyticsSettings);
		}

		[MenuItem("GameRig/Settings/Advertising Settings", priority = 101)]
		private static void OpenAdvertisingSettings()
		{
			SelectSettings<AdvertisingSettings>(GameRigResourcesPaths.AdvertisingSettings);
		}

		[MenuItem("GameRig/Settings/Screenshots Settings Folder", priority = 102)]
		private static void OpenScreenshotsSettings()
		{
			ScreenshotSettings[] rankSettings = Resources.LoadAll<ScreenshotSettings>(GameRigResourcesPaths.ScreenshotsSettings);

			if (rankSettings.Length > 0)
			{
				Selection.activeObject = rankSettings[0];
			}
		}

		public static void SelectSettings<T>(string path) where T : ScriptableObject
		{
			T settings = Resources.Load<T>(path);

			if (!settings)
			{
				settings = ScriptableObject.CreateInstance<T>();

				EditorUtility.SetDirty(settings);

				AssetDatabase.CreateAsset(settings, "Assets/GameRig/Resources/" + path + ".asset");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			Selection.activeObject = settings;
		}
	}
}