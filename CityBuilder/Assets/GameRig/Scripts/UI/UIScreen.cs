using GameRig.Scripts.Systems.GameStateSystem;
using UnityEngine;

namespace GameRig.Scripts.UI
{
	public class UIScreen : MonoBehaviour
	{
		[SerializeField] protected GameState state;
		public GameState State => state;
	}
}