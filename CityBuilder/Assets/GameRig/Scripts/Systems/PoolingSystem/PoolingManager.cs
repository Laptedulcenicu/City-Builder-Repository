using System.Collections.Generic;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	public static class PoolingManager
	{
		private static Dictionary<Component, Pool> pools;
		private static Transform poolsParent;

		[InvokeOnAppLaunch]
		private static void Initialize()
		{
			pools = new Dictionary<Component, Pool>();

			poolsParent = new GameObject("Pools").transform;

			Object.DontDestroyOnLoad(poolsParent);

			PoolingSettings poolingSettings = Resources.Load<PoolingSettings>(GameRigResourcesPaths.PoolingSettings);

			foreach (PoolSettings poolSettings in poolingSettings.PoolsSettings)
			{
				AddPool(poolSettings.Original, poolSettings.Amount);
			}
		}

		public static T GetObject<T>(T original) where T : Component, IPoolable
		{
			T clone;

			if (pools.TryGetValue(original, out Pool pool))
			{
				if (pool.Objects.Count > 0)
				{
					clone = (T) pool.Objects.Pop();

					return clone;
				}
			}
			else
			{
				pool = new Pool(poolsParent, original.name);

				pools.Add(original, pool);
			}

			InstantiateClone(original, pool.PoolParent, out clone);

			clone.OriginalPrefab = original;

			return clone;
		}

		public static T GetObject<T>(T original, Vector3 position, Quaternion rotation) where T : Component, IPoolable
		{
			T clone;

			if (pools.TryGetValue(original, out Pool pool))
			{
				if (pool.Objects.Count > 0)
				{
					clone = (T) pool.Objects.Pop();
				}
				else
				{
					InstantiateClone(original, pool.PoolParent, out clone);
				}
			}
			else
			{
				pool = new Pool(poolsParent, original.name);

				pools.Add(original, pool);

				InstantiateClone(original, pool.PoolParent, out clone);
			}

			clone.transform.SetPositionAndRotation(position, rotation);
			clone.OriginalPrefab = original;

			return clone;
		}

		public static T GetObject<T>(T original, Transform parent) where T : Component, IPoolable
		{
			T clone;

			if (!pools.TryGetValue(original, out Pool pool) || pool.Objects.Count == 0)
			{
				InstantiateClone(original, parent, out clone);
			}
			else
			{
				clone = (T) pool.Objects.Pop();

				clone.transform.SetParent(parent);
			}

			clone.OriginalPrefab = original;

			return clone;
		}

		public static T GetObject<T>(T original, Transform parent, Vector3 position, Quaternion rotation) where T : Component, IPoolable
		{
			T clone;

			if (!pools.TryGetValue(original, out Pool pool) || pool.Objects.Count <= 0)
			{
				InstantiateClone(original, parent, out clone);
			}
			else
			{
				clone = (T) pool.Objects.Pop();

				clone.transform.SetParent(parent);
			}

			clone.transform.SetPositionAndRotation(position, rotation);
			clone.OriginalPrefab = original;

			return clone;
		}

		public static void ReturnObject<T>(T clone) where T : Component, IPoolable
		{
			clone.gameObject.SetActive(false);

			if (!pools.TryGetValue(clone.OriginalPrefab, out Pool pool))
			{
				pool = new Pool(poolsParent, clone.OriginalPrefab.name);

				pools.Add(clone.OriginalPrefab, pool);
			}

			clone.transform.SetParent(pool.PoolParent);

			pool.Objects.Push(clone);
		}

		public static void AddPool<T>(T original, int amount) where T : Component, IPoolable
		{
			if (!pools.TryGetValue(original, out Pool pool))
			{
				pool = new Pool(poolsParent, original.name);

				pools.Add(original, pool);
			}

			original.gameObject.SetActive(false);

			for (int i = 0; i < amount; i++)
			{
				T clone = Object.Instantiate(original, pool.PoolParent);

				clone.OriginalPrefab = original;

				pool.Objects.Push(clone);
			}

			original.gameObject.SetActive(true);
		}

		public static int GetPoolSize<T>(T original) where T : Component, IPoolable
		{
			return pools.TryGetValue(original, out Pool pool) ? pool.Objects.Count : 0;
		}

		public static void RemovePool<T>(T original) where T : Component, IPoolable
		{
			if (pools.TryGetValue(original, out Pool pool))
			{
				DestroyGameObjectsFromPool(pool, pool.Objects.Count);

				Object.Destroy(pool.PoolParent.gameObject);

				pools.Remove(original);
			}
		}

		public static void DestroyPool<T>(T original) where T : Component, IPoolable
		{
			if (pools.TryGetValue(original, out Pool pool))
			{
				DestroyGameObjectsFromPool(pool, pool.Objects.Count);
			}
		}

		public static void DestroyPool<T>(T original, int amount) where T : Component, IPoolable
		{
			if (pools.TryGetValue(original, out Pool pool))
			{
				DestroyGameObjectsFromPool(pool, amount);
			}
		}

		private static void AddPool(Component original, int amount)
		{
			if (!pools.TryGetValue(original, out Pool pool))
			{
				pool = new Pool(poolsParent, original.name);

				pools.Add(original, pool);
			}

			original.gameObject.SetActive(false);

			for (int i = 0; i < amount; i++)
			{
				Component clone = Object.Instantiate(original, pool.PoolParent);

				((IPoolable) clone).OriginalPrefab = original;

				pool.Objects.Push(clone);
			}

			original.gameObject.SetActive(true);
		}

		private static void InstantiateClone<T>(T original, Transform parent, out T clone) where T : Component, IPoolable
		{
			original.gameObject.SetActive(false);

			clone = Object.Instantiate(original, parent);

			original.gameObject.SetActive(true);
		}

		private static void DestroyGameObjectsFromPool(Pool pool, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				Object.Destroy(pool.Objects.Pop().gameObject);
			}
		}
	}
}