using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.GraphicsSystem
{
	public static class GraphicsManager
	{
		[InvokeOnAppLaunch]
		private static void Initialize()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			if (Application.platform == RuntimePlatform.Android)
			{
				GraphicsQualitySettings[] qualityProfiles = Resources.LoadAll<GraphicsQualitySettings>(GameRigResourcesPaths.GraphicsSettings);

				for (int index = qualityProfiles.Length - 1; index >= 0; index--)
				{
					GraphicsQualitySettings qualityProfile = qualityProfiles[index];

					if (SystemInfo.systemMemorySize > qualityProfile.MemoryTreshold)
					{
						Application.targetFrameRate = qualityProfile.TargetFrameRate;
						QualitySettings.SetQualityLevel(qualityProfile.QualityLevel, true);

						float resolutionScale = qualityProfile.ResolutionScale / 100f;

						Screen.SetResolution((int) (Screen.width * resolutionScale), (int) (Screen.height * resolutionScale), true);

						if (qualityProfile.AdditionalSettings != null)
						{
							qualityProfile.AdditionalSettings.Apply();
						}

						break;
					}
				}
			}
			else
			{
				Application.targetFrameRate = 60;
			}

			QualitySettings.vSyncCount = 0;
		}
	}
}