using System.Threading.Tasks;
using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.FadeGameSystem
{
	public static class FadeGameManager
	{
		public delegate void FadeCompleteDelegate(FadeType fadeType);

		public static FadeCompleteDelegate OnFadeComplete = delegate { };

		private static bool canStartFade;
		
		private static FadeGameManagerBehaviour fadeGameManagerBehaviour;

		[InvokeOnAppLaunch(typeof(PubSubManager))]
		public static void Initialize()
		{
			InstantiateFadeImageObject();

			canStartFade = true;
		}

		private static void InstantiateFadeImageObject()
		{
			FadeGameManagerBehaviour fadeGameManagerPrefab = Resources.Load<FadeGameManagerBehaviour>(GameRigResourcesPaths.GameFadeUI);
			fadeGameManagerBehaviour = GameRigCore.InitializeManagerBehaviour(fadeGameManagerPrefab);
			fadeGameManagerBehaviour.DisableCanvas();
		}

		public static async void StartGameFade(FadeType fadeType, GameObject fadePrefab)
		{
			var ts = new TaskCompletionSource<bool>();

			if(!canStartFade)
				return;

			canStartFade = false;
			
			fadeGameManagerBehaviour.EnableCanvas();

			switch (fadeType)
			{
				case FadeType.FadeIn:
					fadeGameManagerBehaviour.StartFadeIn(ts, fadePrefab);
					break;

				case FadeType.FadeOut:
					fadeGameManagerBehaviour.StartFadeOut(ts, fadePrefab);
					break;
			}

			await ts.Task;
		}

		public static void FadeInDone()
		{
			canStartFade = true;
			OnFadeComplete(FadeType.FadeIn);
		}

		public static void FadeOutDone()
		{
			canStartFade = true;
			OnFadeComplete(FadeType.FadeOut);
			fadeGameManagerBehaviour.DisableCanvas();
		}
	}
}