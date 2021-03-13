using System.Threading.Tasks;
using UnityEngine;

namespace GameRig.Scripts.Systems.FadeGameSystem
{
	public class FadeGameManagerBehaviour : MonoBehaviour
	{
		private GameObject fadeObject;
		private FadeBase fadeBase;

		public void StartFadeIn(TaskCompletionSource<bool> ts, GameObject fadePrefab)
		{
			SpawnFadePrefab(fadePrefab);
			fadeBase.StartFadeIn(ts);
		}

		public void StartFadeOut(TaskCompletionSource<bool> ts, GameObject fadePrefab)
		{
			SpawnFadePrefab(fadePrefab);
			fadeBase.StartFadeOut(ts);
		}

		private void SpawnFadePrefab(GameObject fadePrefab)
		{
			DeleteLastFadeObject();

			fadeObject = Instantiate(fadePrefab, transform);
			fadeBase = fadeObject.GetComponent<FadeBase>();
		}

		private void DeleteLastFadeObject()
		{
			if (fadeObject != null)
			{
				Destroy(fadeObject);
			}
		}

		public void DisableCanvas()
		{
			gameObject.SetActive(false);
			DeleteLastFadeObject();
		}

		public void EnableCanvas()
		{
			gameObject.SetActive(true);
		}
	}
}