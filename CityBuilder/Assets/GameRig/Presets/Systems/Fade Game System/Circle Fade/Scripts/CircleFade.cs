using System.Threading.Tasks;
using DG.Tweening;
using GameRig.Scripts.Systems.FadeGameSystem;
using GameRig.Scripts.Utilities;
using SoftMasking;
using UnityEngine;
using UnityEngine.UI;

namespace GameRig.Presets.Systems.Fade_Game_System.Circle_Fade.Scripts
{
	public class CircleFade : FadeBase
	{
		[SerializeField] private Image fadeImage;
		[SerializeField] private Transform circleTransform;

		[SerializeField] private float circleTargetScale;
		[SerializeField] private float fadeDurationAnim;
		
		[SerializeField] private Ease backgroundEase;
		[SerializeField] private Ease circledEase;

		[SerializeField] private SoftMask softMask;

		private Tween tweenFade;
		
		public override void StartFadeIn(TaskCompletionSource<bool> ts)
		{
			tweenFade.Kill();
			softMask.enabled = true;
			fadeImage.SetTransparency(0);
			circleTransform.localScale = Vector3.one * circleTargetScale;

			Sequence sequence = DOTween.Sequence();
			sequence.Append(fadeImage.DOFade(1, fadeDurationAnim).SetEase(backgroundEase));
			sequence.Append(circleTransform.DOScale(Vector3.one * 0.1f, 1f).SetEase(circledEase));
			sequence.OnComplete(() =>
			{
				softMask.enabled = false;
				ts.TrySetResult(true);
				FadeInDone();
			});

			tweenFade = sequence;
		}

		public override void StartFadeOut(TaskCompletionSource<bool> ts)
		{
			tweenFade.Kill();
			softMask.enabled = true;
			fadeImage.SetTransparency(1);
			circleTransform.localScale = Vector3.one * 0.1f;

			Sequence sequence = DOTween.Sequence();
			sequence.Append(circleTransform.DOScale(Vector3.one * circleTargetScale, 1f).SetEase(circledEase));
			sequence.Append(fadeImage.DOFade(0, fadeDurationAnim).SetEase(backgroundEase));
			sequence.OnComplete(() =>
			{
				softMask.enabled = false;
				ts.TrySetResult(true);
				FadeOutDone();
			});
			
			tweenFade = sequence;
		}
	}
}