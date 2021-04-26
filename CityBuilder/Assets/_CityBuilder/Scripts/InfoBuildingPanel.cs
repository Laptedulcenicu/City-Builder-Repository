using System;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using _CityBuilder.Scripts.Test_Script;
using _CityBuilder.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CityBuilder.Scripts
{
    public class InfoBuildingPanel : MonoBehaviour
    {
        public static Action<Structure> ConfigBuildingContainer;
        [SerializeField] private InputController inputController;
        [SerializeField] private InfoStructurePopUpList infoStructurePopUp;
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private StructureManager structureManager;
        [SerializeField] private GameObject panel;
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthLifeText;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Buttons Panel Data")] [SerializeField]
        private Button upgradeButton;

        [SerializeField] private Button repairButton;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button destroyButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] [Range(0.1f, 1f)] private float percentOffsetRepairPrice;

        private Structure currentStructure;
        private ShopItemContainer currentShopItem;

        public StructureManager StructureManager1 => structureManager;

        public Structure CurrentStructure => currentStructure;

        public ShopItemContainer CurrentShopItem => currentShopItem;

        public float PercentOffsetRepairPrice => percentOffsetRepairPrice;

        public void Awake()
        {
            ConfigBuildingContainer += ConfigCurrentBuildingContainer;
            ConfigPanelDataButton();
        }

        private void ConfigPanelDataButton()
        {
            upgradeButton.onClick.AddListener(() => infoStructurePopUp.ShowPopUp(PopUpInfoStructureType.Upgrade));
            repairButton.onClick.AddListener(() => infoStructurePopUp.ShowPopUp(PopUpInfoStructureType.Repair));
            destroyButton.onClick.AddListener(() => infoStructurePopUp.ShowPopUp(PopUpInfoStructureType.Destroy));

            moveButton.onClick.AddListener(MoveBuildingButton);
            rotateButton.onClick.AddListener(RotateBuildingButton);
            cancelButton.onClick.AddListener(CancelButton);
        }

        private void OnDestroy()
        {
            ConfigBuildingContainer -= ConfigCurrentBuildingContainer;
        }

        private void ConfigCurrentBuildingContainer(Structure structure)
        {
            panel.SetActive(true);
            currentStructure = structure;
            currentShopItem = shopManager.GetShopItem(structure.Container);
            CurrentLife();
            SetCurrentConfigType();
        }

        private void SetCurrentConfigType()
        {
            repairButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            moveButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            destroyButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);


            switch (currentStructure.Configuration.TypeConFiguration)
            {
                case ConfigType.Functional:
                    CurrentUpgradeLevel();
                    CheckToRepair();
                    moveButton.gameObject.SetActive(true);
                    rotateButton.gameObject.SetActive(true);
                    levelText.gameObject.SetActive(true);
                    break;
                case ConfigType.NonFunctional:
                    CheckToRepair();
                    moveButton.gameObject.SetActive(true);

                    if (currentStructure.Container.CellTypeStructure != CellType.Road)
                    {
                        rotateButton.gameObject.SetActive(true);
                    }

                    break;
            }
        }

        public void CurrentUpgradeLevel()
        {
            FunctionalConfiguration currentFunctionalConfiguration =
                (FunctionalConfiguration) currentStructure.Configuration;
            FunctionalStructureContainer functionalStructureContainer =
                (FunctionalStructureContainer) currentStructure.Container;

            int currentLevel = currentFunctionalConfiguration.currentUpgradeLevel;

            levelText.text = currentLevel + "/" + (functionalStructureContainer.UpgradeStageList.Count - 1) + "level";

            if (currentLevel < functionalStructureContainer.UpgradeStageList.Count - 1)
            {
                upgradeButton.gameObject.SetActive(true);
            }
            else
            {
                upgradeButton.gameObject.SetActive(false);
            }
        }


        public void CurrentLife()
        {
            StructureConfiguration currentConfig = currentStructure.Configuration;
            healthLifeText.text = currentConfig.StatusData.CurrentHealth + "/" + currentConfig.StatusData.MAXHealth;

            CurrentHealthBarProgress(currentConfig.StatusData.CurrentHealth, currentConfig.StatusData.MAXHealth);
        }

        private void CurrentHealthBarProgress(int currentValue, int maxValue)
        {
            healthImage.fillAmount = (float) currentValue / maxValue;
        }

        private void CheckToRepair()
        {
            StructureConfiguration currentConfig = currentStructure.Configuration;

            if (currentConfig.StatusData.CurrentHealth < currentConfig.StatusData.MAXHealth)
            {
                repairButton.gameObject.SetActive(true);
            }
            else
            {
                repairButton.gameObject.SetActive(false);
            }
        }


        #region BuildingButtons

        private void MoveBuildingButton()
        {
            Point point = new Point((int) currentStructure.transform.position.x, (int) currentStructure.transform.position.z);
            structureManager.placementManager.RemoveStructureAtPosition(point, currentStructure);
            inputController.MoveStructure(currentStructure);
            CancelButton();
        }

        private void RotateBuildingButton()
        {
            Transform transform1 = currentStructure.transform;
            Vector3 currentRotation = transform1.eulerAngles;
            currentRotation = currentRotation.y == 270f ? Vector3.zero : new Vector3(0, currentRotation.y + 90, 0);
            transform1.eulerAngles = currentRotation;
        }

        public void CancelButton()
        {
            panel.SetActive(false);
        }

        #endregion
    }
}