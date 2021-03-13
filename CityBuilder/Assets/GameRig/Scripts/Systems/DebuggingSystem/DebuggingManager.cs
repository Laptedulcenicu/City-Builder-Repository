using UnityEngine;

namespace GameRig.Scripts.Systems.DebuggingSystem
{
	public static class DebuggingManager
	{
		public static void InitializeDebuggers()
		{
			if (Debug.unityLogger.logEnabled)
			{
				Object.Instantiate(Resources.Load<GameObject>("Debugging System/Advanced FPS Counter"));

				GameRigCore.InitializeManagerBehaviour<DebuggingManagerBehaviour>();
			}
		}
	}
}