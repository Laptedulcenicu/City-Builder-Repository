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
                    SetUpgradeStage(functionalConfiguration.currentUpgradeLevel);
                    break;
                }
                case ConfigType.NonFunctional:
                    structureConfiguration = new NonFunctionalConfiguration(configuration);
                    currentVisualStructure = Instantiate(container.DefaultPrefab, transform);
                    break;

                case ConfigType.Natural:
                    structureConfiguration = new NatureConfiguration(configuration);
                    currentVisualStructure = Instantiate(container.DefaultPrefab, transform);
                    break;
            }
        }

        public void SetUpgradeStage(int upgradeLevel)
        {
            FunctionalStructureContainer functionalStructureContainer =(FunctionalStructureContainer) structureContainer;

            if (currentVisualStructure)
            {
                Destroy(currentVisualStructure);
            }

            structureConfiguration = new FunctionalConfiguration(functionalStructureContainer.UpgradeStageList[upgradeLevel].Configuration);
            FunctionalConfiguration functionalStructureConfiguration = (FunctionalConfiguration) structureConfiguration;

            functionalStructureConfiguration.currentUpgradeLevel = upgradeLevel;
            currentVisualStructure = Instantiate(functionalStructureContainer.UpgradeStageList[upgradeLevel].GameObjectPrefab, transform);
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

        }

        public void SwapModel(StructureContainer container, RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            structureContainer = container;
            structureConfiguration = new NonFunctionalConfiguration(container.DefaultStructureConfiguration); 

            Destroy(currentVisualStructure);

            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);
            currentVisualStructure.transform.localPosition = new Vector3(0, 0, 0);
            currentVisualStructure.transform.localRotation = rotation;

        }

        public void OnClick()
        {
            InfoBuildingPanel.ConfigBuildingContainer?.Invoke(this);
        }
    }
}