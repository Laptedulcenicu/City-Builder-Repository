using System;
using _CityBuilder.Scripts.Scriptable_Object;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CityBuilder.Scripts
{
    public class InfoBuildingPanel : MonoBehaviour
    {
        public static Action<StructureContainer> ConfigBuildingContainer;

        [SerializeField] private Image buildingImage;
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI healthLife;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Buttons Panel Data")] [SerializeField]
        private Button upgradeButton;

        [SerializeField] private Button repairButton;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button destroyButton;
        [SerializeField] private Button cancelButton;

        private StructureContainer currentContainer;

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

        private void ConfigCurrentBuildingContainer(StructureContainer structureContainer)
        {
            // currentContainer = buildingContainer;
            //
            // buildingImage  = buildingContainer.
            //     healthImage
            //         
            // descriptionText
            //     healthLife
            // levelText
        }

        private void DestroyBuildingButton()
        {
        }

        private void UpgradeBuildingButton()
        {
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
        }
    }
}