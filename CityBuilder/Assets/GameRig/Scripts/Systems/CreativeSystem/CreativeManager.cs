using GameRig.Scripts.Systems.InputSystem;
using GameRig.Scripts.Utilities.Attributes;
using InputManager = _CityBuilder.Scripts.InputManager;
#if UNITY_STANDALONE
using UnityEngine;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using GameRig.Scripts.Systems.SaveSystem;
#endif

namespace GameRig.Scripts.Systems.CreativeSystem
{
	/// <summary>
	/// This class is used to store game changes required by Creative Team
	/// </summary>
	public static class CreativeManager
	{
#if UNITY_STANDALONE
		private static CreativeManagerBehaviour creativeManagerBehaviour;
		private static ScreenshotSettings[] screenshotsSettings;
#endif

		[InvokeOnAppLaunch(typeof(InputManager))]
		private static void Initialize()
		{
#if UNITY_STANDALONE
			creativeManagerBehaviour = GameRigCore.InitializeManagerBehaviour<CreativeManagerBehaviour>();

			screenshotsSettings = Resources.LoadAll<ScreenshotSettings>(GameRigResourcesPaths.ScreenshotsSettings);

		//	InputManager.Subscribe(KeyEventType.KeyDown, KeyCode.P, SaveScreenshots);
		//	InputManager.Subscribe(KeyEventType.KeyDown, KeyCode.R, ResetTheResolution);
		//	InputManager.Subscribe(KeyEventType.KeyDown, KeyCode.E, ResetGame);
#endif
		}

#if UNITY_STANDALONE
		private static void SaveScreenshots()
		{
			if (Application.platform != RuntimePlatform.WindowsEditor)
			{
				creativeManagerBehaviour.SaveScreenshots(screenshotsSettings);
			}
		}

		private static void ResetTheResolution()
		{
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
		}

		private static void ResetGame()
		{
			SaveManager.DeleteAll();
			Application.Quit();
		}
#endif
	}
}