using System;
using System.Collections.Generic;
using System.Linq;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object;
using BitBenderGames;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _CityBuilder.Scripts.Test_Script
{
    [Serializable]
    public struct MoneyTextData
    {
        public TextMeshProUGUI moneyAmount;
        public ResourceType resourceType;
        
    }
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private TouchInputController touchInputController;
        [SerializeField] private List<MoneyTextData> needMoney;
        [SerializeField] private List<MoneyTextData> currentMoney;
        [SerializeField] private Item itemPrefab;
        [SerializeField] private Transform selection;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject needMoneyPanel;
        [SerializeField] private GameObject descriptionPanel;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI descrption;
        private List<ShopItemContainer> shopItemContainerList;
        private List<Item> instantiatedItem;
        private ShopItemContainer selectedShopItemContainer;
        [SerializeField] private GameManager gameManager;
        private void Awake()
        {
            GameResourcesManager.OnGameResourcesChange+= OnGameResourcesChange;
            
            shopItemContainerList = Resources.LoadAll<ShopItemContainer>("ShopItem").ToList();
            
            foreach (ShopItemContainer shopItemContainer in shopItemContainerList)
            {
                Item currentItem = Instantiate(itemPrefab,content);
                currentItem.Initialize( shopItemContainer, this);
            }
            
            foreach (var moneyTextData in currentMoney)
            {
                moneyTextData.moneyAmount.text = GameResourcesManager.GetDisplayString(moneyTextData.resourceType);
            }
            
            Disable();
            GameResourcesManager.AddResourceAmount(ResourceType.Gold, 100);
            GameResourcesManager.AddResourceAmount(ResourceType.Wood, 300);
        }
        private void OnDestroy()
        {
            GameResourcesManager.OnGameResourcesChange -= OnGameResourcesChange;
        }

        private void OnGameResourcesChange(ResourceType resourceType, int amount)
        {
            currentMoney.Find(e => e.resourceType == resourceType).moneyAmount.text =
                GameResourcesManager.GetDisplayString(resourceType);
        }


        public void SelectButton(Transform item, ShopItemContainer shopItemContainer)
        {
            selection.parent = item;
            selection.localPosition= Vector3.zero;

            if (selectedShopItemContainer == shopItemContainer)
            {
                Disable();
                selectedShopItemContainer = null;
            }
            else
            {
                selection.localPosition= Vector3.one;
                selectedShopItemContainer = shopItemContainer;
                ShowAll();

                foreach (var necessaryResourcesData in selectedShopItemContainer.NecessaryResourcesDataList)
                {
                    MoneyTextData currentNeedMoney=  needMoney.Find(e => e.resourceType == necessaryResourcesData.Resource);
                    currentNeedMoney.moneyAmount.text = GameResourcesManager.GetDisplayString(necessaryResourcesData.Amount);
                    currentNeedMoney.moneyAmount.transform.parent.gameObject.SetActive(true);
                }

                descrption.text = shopItemContainer.ItemDescription;
            }
           
        }

        public void Confirm()
        {
            foreach (var necessaryResourcesData in selectedShopItemContainer.NecessaryResourcesDataList)
            {
                if (necessaryResourcesData.Amount > GameResourcesManager.GetResourceAmount(necessaryResourcesData.Resource))
                {
                    return;
                }
            }

            gameManager.GenericPlacementHandler(selectedShopItemContainer);
            Disable();
            panel.SetActive(false);
            touchInputController.enabled = false;
        }

        public void Cancel()
        {
            panel.SetActive(true);
            touchInputController.enabled = true;
        }

        private void Disable()
        {
            selection.gameObject.SetActive(false);
            needMoneyPanel.SetActive(false);
            descriptionPanel.SetActive(false);
        }

        private void ShowAll()
        {
            selection.gameObject.SetActive(true);
            descriptionPanel.SetActive(true);
            needMoneyPanel.SetActive(true);
            foreach (var moneyTextData in needMoney)
            {
                moneyTextData.moneyAmount.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
