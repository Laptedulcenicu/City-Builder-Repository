using System;
using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	[Serializable]
	public class PoolSettings
	{
		[SerializeField] private Component original;
		[SerializeField] private int amount;

		public Component Original
		{
			get => original;
			set => original = value;
		}

		public int Amount => amount;
	}
}