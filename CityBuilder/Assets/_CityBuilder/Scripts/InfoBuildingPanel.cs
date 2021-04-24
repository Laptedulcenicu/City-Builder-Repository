using System;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CityBuilder.Scripts
{
    public class InfoBuildingPanel : MonoBehaviour
    {
        public static Action<Structure> ConfigBuildingContainer;
        [SerializeField] private StructureManager structureManager;
        [SerializeField] private GameObject panel;
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthLifeText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI repairPriceText;

        [Header("Buttons Panel Data")] [SerializeField]
        private Button upgradeButton;

        [SerializeField] private Button repairButton;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button destroyButton;
        [SerializeField] private Button cancelButton;

        private Structure currentStructure;
        private int repairPrice;

        public void Awake()
        {
            ConfigBuildingContainer += ConfigCurrentBuildingContainer;
            ConfigPanelDataButton();
        }

        private void ConfigPanelDataButton()
        {
            upgradeButton.onClick.AddListener(UpgradeBuildingButton);
            repairButton.onClick.AddListener(RepairBuildingButton);
            moveButton.onClick.AddListener(MoveBuildingButton);
            rotateButton.onClick.AddListener(RotateBuildingButton);
            destroyButton.onClick.AddListener(DestroyBuildingButton);
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
                    rotateButton.gameObject.SetActive(true);
                    break;
            }
        }

        private void CurrentUpgradeLevel()
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


        private void CurrentLife()
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
                repairPrice = 10;
                repairPriceText.text = repairPrice.ToString();
                repairButton.gameObject.SetActive(true);
            }
            else
            {
                repairButton.gameObject.SetActive(false);
            }
        }


        #region BuildingButtons

        private void DestroyBuildingButton()
        {
            foreach (NecessaryResourcesData necessaryResourcesData in currentStructure.Configuration
                .DestroyEarnResourcesList)
            {
                GameResourcesManager.AddResourceAmount(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
            }

            Point point = new Point((int)currentStructure.transform.position.x,(int) currentStructure.transform.position.z);
            structureManager.placementManager.SetCellTypeAtPosition(point, CellType.Empty );
            Destroy(currentStructure.gameObject);
            CancelButton();
        }

        private void UpgradeBuildingButton()
        {
            FunctionalConfiguration currentFunctionalConfiguration =
                (FunctionalConfiguration) currentStructure.Configuration;

            int currentLevel = currentFunctionalConfiguration.currentUpgradeLevel;

            currentLevel++;
            currentStructure.SetUpgradeStage(currentLevel);

            CurrentUpgradeLevel();
        }

        private void RepairBuildingButton()
        {
        }

        private void MoveBuildingButton()
        {
        }

        private void RotateBuildingButton()
        {
        }

        private void CancelButton()
        {
            panel.SetActive(false);
        }

        #endregion
    }
}