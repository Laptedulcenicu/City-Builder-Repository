using System.Threading.Tasks;
using DG.Tweening;
using GameRig.Scripts.Systems.FadeGameSystem;
using GameRig.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameRig.Presets.Systems.Fade_Game_System.Diagonal_Fade.Scripts
{
	public class DiagonalFade : FadeBase
	{
		[SerializeField] private RectTransform fadeSquare1;
		[SerializeField] private RectTransform fadeSquare2;

		[SerializeField] private Image fadeImage1;
		[SerializeField] private Image fadeImage2;
		[SerializeField] private float fadeDurationAnim;
		[SerializeField] private Ease backgroundEase;
		[SerializeField] private Ease diagonalEase;

		private Vector2 targetFadeSquare;
		private Vector2 hidePosition;

		private void Awake()
		{
			targetFadeSquare = Vector2.one * fadeSquare1.sizeDelta.x / 2f;
			hidePosition = fadeSquare1.sizeDelta.x / 1.4f * Vector2.one;
		}

		public override void StartFadeIn(TaskCompletionSource<bool> ts)
		{
			fadeImage1.SetTransparency(0);
			fadeImage2.SetTransparency(0);
			fadeSquare1.anchoredPosition = Vector2.zero;
			fadeSquare2.anchoredPosition = Vector2.zero;

			fadeImage1.DOFade(1, fadeDurationAnim).SetEase(backgroundEase);
			fadeImage2.DOFade(1, fadeDurationAnim).SetEase(backgroundEase).OnComplete(() =>
			{
				ts.TrySetResult(true);
				FadeInDone();
			});
			fadeSquare1.DOAnchorPos(-targetFadeSquare, fadeDurationAnim).SetEase(diagonalEase).SetDelay(fadeDurationAnim / 2f);
			fadeSquare2.DOAnchorPos(targetFadeSquare, fadeDurationAnim).SetEase(diagonalEase).SetDelay(fadeDurationAnim / 2f);
		}

		public override void StartFadeOut(TaskCompletionSource<bool> ts)
		{
			fadeImage1.SetTransparency(1);
			fadeImage2.SetTransparency(1);
			fadeSquare1.anchoredPosition = targetFadeSquare;
			fadeSquare2.anchoredPosition = -targetFadeSquare;

			fadeImage1.DOFade(0, fadeDurationAnim).SetEase(backgroundEase).SetDelay(fadeDurationAnim / 2f);
			fadeImage2.DOFade(0, fadeDurationAnim).SetEase(backgroundEase).SetDelay(fadeDurationAnim / 2f).OnComplete(() =>
			{
				ts.TrySetResult(true);
				FadeOutDone();
			});
			fadeSquare1.DOAnchorPos(Vector2.zero, fadeDurationAnim).SetEase(diagonalEase);
			fadeSquare2.DOAnchorPos(Vector2.zero, fadeDurationAnim).SetEase(diagonalEase);
		}
	}
}