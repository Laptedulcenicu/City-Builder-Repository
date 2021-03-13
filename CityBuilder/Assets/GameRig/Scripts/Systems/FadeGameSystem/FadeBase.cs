using System.Threading.Tasks;
using UnityEngine;

namespace GameRig.Scripts.Systems.FadeGameSystem
{
	public abstract class FadeBase : MonoBehaviour
	{
		public abstract void StartFadeIn(TaskCompletionSource<bool> ts);
		public abstract void StartFadeOut(TaskCompletionSource<bool> ts);

		protected void FadeInDone()
		{
			FadeGameManager.FadeInDone();
		}

		protected void FadeOutDone()
		{
			FadeGameManager.FadeOutDone();
		}
	}
}