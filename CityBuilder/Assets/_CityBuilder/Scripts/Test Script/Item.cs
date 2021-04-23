using _CityBuilder.Scripts.Scriptable_Object;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CityBuilder.Scripts.Test_Script
{
  

    public class Item : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image image;
        private ShopManager shopManager;
        private ShopItemContainer shopItemContainer;
        
        public void Initialize(ShopItemContainer container, ShopManager shop )
        {
            shopItemContainer = container;
            shopManager = shop;
            GetComponent<Button>().onClick.AddListener(Select);
            image.sprite = container.ShopItemSprite;
            name.text = container.ItemName;
        }


        private void Select()
        {
            shopManager.SelectButton(transform, shopItemContainer);
        }
    }
}
