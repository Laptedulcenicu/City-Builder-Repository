using CodeStage.AdvancedFPSCounter;
using UnityEngine;

namespace GameRig.Scripts.Systems.DebuggingSystem
{
	public class DebuggingManagerBehaviour : MonoBehaviour
	{
		private const float TouchesTravelDistance = 0.5f;

		private float firstTouchStartPosition;
		private float secondTouchStartPosition;

		private Touch firstTouch;
		private Touch secondTouch;

		private void Update()
		{
			if (Input.touchCount == 2)
			{
				firstTouch = Input.GetTouch(0);
				secondTouch = Input.GetTouch(1);

				if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
				{
					ResetTouches();
				}

				float firstTouchDeltaPosition = (firstTouch.position.y - firstTouchStartPosition) / Screen.height;
				float secondTouchDeltaPosition = (secondTouch.position.y - secondTouchStartPosition) / Screen.height;

				if (firstTouchDeltaPosition > TouchesTravelDistance && secondTouchDeltaPosition > TouchesTravelDistance)
				{
					switch (AFPSCounter.Instance.OperationMode)
					{
						case OperationMode.Disabled:
							AFPSCounter.Instance.OperationMode = OperationMode.Normal;

							break;
						case OperationMode.Normal:
							AFPSCounter.Instance.OperationMode = OperationMode.Disabled;

							break;
					}

					ResetTouches();
				}

				if (firstTouchDeltaPosition < -TouchesTravelDistance && secondTouchDeltaPosition < -TouchesTravelDistance)
				{
				//	LunarConsole.Show();

					ResetTouches();
				}
			}
		}

		private void ResetTouches()
		{
			firstTouchStartPosition = firstTouch.position.y;
			secondTouchStartPosition = secondTouch.position.y;
		}
	}
}