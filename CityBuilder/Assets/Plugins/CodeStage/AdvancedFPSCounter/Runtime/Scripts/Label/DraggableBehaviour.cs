using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugins.CodeStage.AdvancedFPSCounter.Runtime.Scripts.Label
{
	public class DraggableBehaviour : MonoBehaviour, IDragHandler, IPointerDownHandler
	{
		private static RectTransform thisRectTransform;
		private static Vector2 startOffset;
		private static float canvasScale;

		private void Awake()
		{
			thisRectTransform = GetComponent<RectTransform>();

			canvasScale = 1f / transform.parent.localScale.x;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			startOffset = thisRectTransform.anchoredPosition - eventData.position * canvasScale;
		}

		public void OnDrag(PointerEventData eventData)
		{
			thisRectTransform.anchoredPosition = eventData.position * canvasScale + startOffset;
		}
	}
}