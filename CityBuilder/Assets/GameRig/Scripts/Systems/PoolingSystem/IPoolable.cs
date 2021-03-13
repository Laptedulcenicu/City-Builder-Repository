using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	public interface IPoolable
	{
		Component OriginalPrefab { get; set; }
	}
}