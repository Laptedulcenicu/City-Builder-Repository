using GameRig.Scripts.Systems.GameStateSystem;
using UnityEngine;

namespace GameRig.Scripts.UI.Screens
{
	public class MainMenuScreen : UIScreen
	{
		[SerializeField] private GameObject[] turnedOffVibrationIcons;

		private void Awake()
		{
		//	VibrationManager.OnVibrationStateChange += HandleVibrationStateChange;

			HandleVibrationStateChange();
		}

		public void OnToggleVibrationButtonClick()
		{
	//		VibrationManager.CanVibrate = !VibrationManager.CanVibrate;
		}

		public void OnPlayButtonClick()
		{
			GameStateManager.CurrentState = GameState.Game;
		}

		private void HandleVibrationStateChange()
		{
		//	foreach (GameObject turnedOffVibrationIcon in turnedOffVibrationIcons)
		//	{
			//	turnedOffVibrationIcon.SetActive(!VibrationManager.CanVibrate);
		//	}
		}

		private void OnDestroy()
		{
		//	VibrationManager.OnVibrationStateChange -= HandleVibrationStateChange;
		}
	}
}