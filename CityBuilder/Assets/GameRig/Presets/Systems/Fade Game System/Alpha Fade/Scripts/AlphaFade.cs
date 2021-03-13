using System.Threading.Tasks;
using DG.Tweening;
using GameRig.Scripts.Systems.FadeGameSystem;
using GameRig.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameRig.Presets.Systems.Fade_Game_System.Alpha_Fade.Scripts
{
	public class AlphaFade : FadeBase
	{
		[SerializeField] private Image fadeImage;

		[SerializeField] private float fadeDurationAnim;
		[SerializeField] private Ease fadeEase;

		private Tween tweenFade;
		
		public override void StartFadeIn(TaskCompletionSource<bool> ts)
		{
			tweenFade.Kill();
			fadeImage.SetTransparency(0);
			tweenFade = fadeImage.DOFade(1, fadeDurationAnim).SetEase(fadeEase).OnComplete(() =>
			{
				ts.TrySetResult(true);
				FadeInDone();
			});
		}

		public override void StartFadeOut(TaskCompletionSource<bool> ts)
		{
			tweenFade.Kill();
			fadeImage.SetTransparency(1);
			tweenFade = fadeImage.DOFade(0, fadeDurationAnim).SetEase(fadeEase).OnComplete(() =>
			{
				ts.TrySetResult(true);
				FadeOutDone();
			});
		}
	}
}