using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.StructureModel
{
    public class Structure : MonoBehaviour, IClickable
    {
        private StructureContainer structureContainer;
        private StructureConfiguration structureConfiguration;

        private GameObject currentVisualStructure;
        public StructureContainer Container => structureContainer;
        public StructureConfiguration Configuration => structureConfiguration;


        private void LoadDefaultConfig(StructureContainer container, StructureConfiguration configuration)
        {
            structureContainer = container;

            if (configuration.TypeConFiguration == ConfigType.Functional)
            {
                FunctionalConfiguration functionalConfiguration = (FunctionalConfiguration) configuration;
                SetUpgradeStage(functionalConfiguration.currentUpgradeLevel);
            }
            else
            {
                structureConfiguration = new StructureConfiguration(configuration);
                currentVisualStructure = Instantiate(container.DefaultPrefab, transform);
            }
        }

        public void SetUpgradeStage(int upgradeLevel)
        {
            FunctionalStructureContainer functionalStructureContainer =
                (FunctionalStructureContainer) structureContainer;

            if (currentVisualStructure)
            {
                Destroy(currentVisualStructure);
            }

            structureConfiguration =
                new FunctionalConfiguration(functionalStructureContainer.UpgradeStageList[upgradeLevel].Configuration);
            currentVisualStructure =
                Instantiate(functionalStructureContainer.UpgradeStageList[upgradeLevel].GameObjectPrefab, transform);
        }

        public void CreateModel(StructureContainer container, StructureConfiguration configuration)
        {
            LoadDefaultConfig(container, configuration);
        }

        public void CreateModel(StructureContainer container, StructureConfiguration configuration,
            RoadBuildingData roadBuildingData)
        {
            structureContainer = container;
            structureConfiguration = new StructureConfiguration(configuration);
            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);

            //   LoadDefaultConfig(); TODO It will be added if Road will be with upgrade
        }

        public void SwapModel(StructureContainer container, RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            structureContainer = container;
            structureConfiguration = new StructureConfiguration(container.DefaultStructureConfiguration);

            Destroy(currentVisualStructure);

            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);
            currentVisualStructure.transform.localPosition = new Vector3(0, 0, 0);
            currentVisualStructure.transform.localRotation = rotation;

            //   LoadDefaultConfig(); TODO It will be added if Road will be with upgrade
        }

        public void OnClick()
        {
            InfoBuildingPanel.ConfigBuildingContainer?.Invoke(this);
        }
    }
}