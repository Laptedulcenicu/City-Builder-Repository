using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	public class PoolableComponentContainer : MonoBehaviour, IPoolable
	{
		[SerializeField] private Component component;

		public Component OriginalPrefab { get; set; }

		public T GetStoredComponent<T>() where T : Component
		{
			return (T) component;
		}
	}
}