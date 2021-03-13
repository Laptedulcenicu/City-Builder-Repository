using UnityEngine;

namespace GameRig.Scripts.Systems.UpgradesSystem
{
	public class UpgradesSystemSettings : ScriptableObject
	{
		[SerializeField] private bool isSystemEnabled;

		public bool IsSystemEnabled => isSystemEnabled;
	}
}