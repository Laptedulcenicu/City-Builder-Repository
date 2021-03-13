using UnityEngine;

namespace GameRig.Scripts.Systems.AnalyticsSystem
{
	public class AnalyticsSettings : ScriptableObject
	{
		[SerializeField] private bool debugMode;

		public bool DebugMode => debugMode;
	}
}