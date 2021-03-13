using System.Collections.Generic;
using System.Linq;
using GameRig.Scripts.Systems.FadeGameSystem;
using GameRig.Scripts.Systems.GameStateSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.ConstantValues;
using UnityEngine;

namespace GameRig.Scripts.UI
{
	public class UIManager : MonoBehaviour
	{
		private Dictionary<GameState, UIScreen> screens;
		[SerializeField] private GameObject notification;
		[SerializeField] private GameObject fadeType;
		[SerializeField] private GameObject shop;

		private void Awake()
		{
			FadeGameManager.StartGameFade(FadeType.FadeOut,fadeType);
			
			List<UIScreen> screensList = GetComponentsInChildren<UIScreen>(true).ToList();
			screens = screensList.ToDictionary(screen => screen.State, screen => screen);

			GameStateManager.OnGameStateChange += HandleGameStateChange;

			GameStateManager.CurrentState = GameState.Menu;

			if (SaveManager.Load(SaveKeys.Notification, false))
			{
				notification.SetActive(true);
			}
			else
			{
				notification.SetActive(false);
			}
		}

		private void HandleGameStateChange(GameState gameState)
		{
			if (screens.ContainsKey(gameState))
			{
				foreach (KeyValuePair<GameState, UIScreen> screen in screens)
				{
					screen.Value.gameObject.SetActive(false);
				}

				screens[gameState].gameObject.SetActive(true);
			}
		}

		private void OnDestroy()
		{
			GameStateManager.OnGameStateChange -= HandleGameStateChange;
		}

		public void OpenCloseShop()
		{
			shop.SetActive(!shop.activeSelf);
			notification.SetActive(false);
			SaveManager.Save(SaveKeys.Notification, false);
		}
	}
}