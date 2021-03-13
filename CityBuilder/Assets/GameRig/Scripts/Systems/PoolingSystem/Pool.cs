using System.Collections.Generic;
using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	public readonly struct Pool
	{
		public Stack<Component> Objects { get; }
		public Transform PoolParent { get; }

		public Pool(Transform poolsParent, string originalObjectName)
		{
			PoolParent = new GameObject(originalObjectName).transform;
			PoolParent.SetParent(poolsParent);

			Objects = new Stack<Component>();
		}
	}
}