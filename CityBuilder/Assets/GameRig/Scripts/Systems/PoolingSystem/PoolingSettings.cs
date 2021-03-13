using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;

#endif

namespace GameRig.Scripts.Systems.PoolingSystem
{
	public class PoolingSettings : ScriptableObject
	{
		[SerializeField] private List<PoolSettings> poolsSettings;

		public IEnumerable<PoolSettings> PoolsSettings => poolsSettings;

#if UNITY_EDITOR
		private void OnValidate()
		{
			foreach (PoolSettings poolSettings in poolsSettings.Where(poolSettings => poolSettings.Original != null && !(poolSettings.Original is IPoolable)))
			{
				if (poolSettings.Original.gameObject.TryGetComponent(out IPoolable poolableComponent))
				{
					poolSettings.Original = (Component) poolableComponent;
				}
				else
				{
					Debug.LogError(poolSettings.Original.name + " does not have any Poolable component");

					poolSettings.Original = null;
				}
			}
		}
#endif
	}
}