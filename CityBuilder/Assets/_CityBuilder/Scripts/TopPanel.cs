using UnityEngine;

namespace _CityBuilder.Scripts
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;

        public void ActivateShopPanelButton()
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }
}
