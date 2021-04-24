using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private InputManager inputManager;
        [FormerlySerializedAs("gameManager")] [SerializeField] private InputController inputController;
        public void ActivateShopPanelButton()
        {
            inputManager.ClearEvents();
            
            shopPanel.SetActive(!shopPanel.activeSelf);

            if (shopPanel.activeSelf == false)
            {
                inputController.ActivateStructureSelection();
            }
        }
    }
}
