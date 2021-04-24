using UnityEngine;

namespace _CityBuilder.Scripts
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private GameManager gameManager;
        public void ActivateShopPanelButton()
        {
            inputManager.ClearEvents();
            shopPanel.SetActive(!shopPanel.activeSelf);

            if (shopPanel.activeSelf == false)
            {
                gameManager.ActivateStructureSelection();
            }
        }
    }
}
