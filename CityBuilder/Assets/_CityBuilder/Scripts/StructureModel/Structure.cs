using _CityBuilder.Scripts.FunctionalStruct;
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

            switch (configuration.TypeConFiguration)
            {
                case ConfigType.Functional:
                {
                    FunctionalConfiguration functionalConfiguration = (FunctionalConfiguration) configuration;
                    structureConfiguration = new FunctionalConfiguration(functionalConfiguration);
                    LoadUpgrade(functionalConfiguration.currentUpgradeLevel);
                    break;
                }
                case ConfigType.NonFunctional:
                    structureConfiguration = new NonFunctionalConfiguration(configuration);
                    currentVisualStructure = Instantiate(container.DefaultPrefab, transform);
                    currentVisualStructure.transform.localPosition = Vector3.zero;
                    break;

                case ConfigType.Natural:
                    structureConfiguration = new NatureConfiguration(configuration);
                    currentVisualStructure = Instantiate(container.DefaultPrefab, transform);
                    currentVisualStructure.transform.localPosition = Vector3.zero;
                    break;
            }
        }


        private void LoadUpgrade(int upgradeLevel)
        {
            FunctionalStructureContainer functionalStructureContainer =
                (FunctionalStructureContainer) structureContainer;

            if (currentVisualStructure)
            {
                Destroy(currentVisualStructure);
            }

            FunctionalConfiguration functionalStructureConfiguration = (FunctionalConfiguration) structureConfiguration;

            functionalStructureConfiguration.currentUpgradeLevel = upgradeLevel;
            FunctionalStructureOperation functionalStructureOperation =
                Instantiate(functionalStructureContainer.StructureOperation, transform);
            functionalStructureOperation.transform.localPosition = Vector3.zero;
            functionalStructureOperation.StartOperation(functionalStructureConfiguration.EarnResourcesDelayDataList, this);
            currentVisualStructure = Instantiate(functionalStructureContainer.UpgradeStageList[upgradeLevel].GameObjectPrefab, transform);
            currentVisualStructure.transform.localPosition = Vector3.zero;
        }

        public void SetUpgradeStage(int upgradeLevel)
        {
            FunctionalStructureContainer functionalStructureContainer =
                (FunctionalStructureContainer) structureContainer;

            if (currentVisualStructure)
            {
                Destroy(currentVisualStructure);
            }

            FunctionalConfiguration functionalStructureConfiguration = new FunctionalConfiguration(functionalStructureContainer.UpgradeStageList[upgradeLevel].Configuration);
            structureConfiguration = functionalStructureConfiguration;

            functionalStructureConfiguration.currentUpgradeLevel = upgradeLevel;
            FunctionalStructureOperation functionalStructureOperation = Instantiate(functionalStructureContainer.StructureOperation, transform);
            functionalStructureOperation.transform.localPosition = Vector3.zero;
            functionalStructureOperation.StartOperation(functionalStructureConfiguration.EarnResourcesDelayDataList, this);
            currentVisualStructure = Instantiate(functionalStructureContainer.UpgradeStageList[upgradeLevel].GameObjectPrefab, transform);
            currentVisualStructure.transform.localPosition = Vector3.zero;
        }

        public void CreateModel(StructureContainer container, StructureConfiguration configuration)
        {
            LoadDefaultConfig(container, configuration);
        }

        public void CreateModel(StructureContainer container, StructureConfiguration configuration,
            RoadBuildingData roadBuildingData)
        {
            structureContainer = container;
            structureConfiguration = new NonFunctionalConfiguration(configuration);
            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);
            currentVisualStructure.transform.localPosition = Vector3.zero;
        }

        public void SwapModel(StructureContainer container, RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            structureContainer = container;
            structureConfiguration = new NonFunctionalConfiguration(container.DefaultStructureConfiguration);

            Destroy(currentVisualStructure);

            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);
            currentVisualStructure.transform.localPosition = Vector3.zero;
            currentVisualStructure.transform.localRotation = rotation;
        }

        public void OnClick()
        {
            InfoBuildingPanel.ConfigBuildingContainer?.Invoke(this);
        }
    }
}