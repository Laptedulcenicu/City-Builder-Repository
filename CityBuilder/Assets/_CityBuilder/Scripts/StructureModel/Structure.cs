using _CityBuilder.Scripts.Scriptable_Object;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.StructureModel
{
    public class Structure : MonoBehaviour
    {
        private StructureContainer structureContainer;
        private StructureConfiguration structureConfiguration;

        private GameObject currentVisualStructure;
        public StructureContainer Container => structureContainer;
        public StructureConfiguration Configuration => structureConfiguration;

        public void CreateModel(StructureContainer container, StructureConfiguration configuration)
        {
            structureContainer = container;
            structureConfiguration = configuration;
            currentVisualStructure = Instantiate(container.DefaultPrefab, transform);

            LoadDefaultConfig();
        }

        public void CreateModel(StructureContainer container, StructureConfiguration configuration,
            RoadBuildingData roadBuildingData)
        {
            structureContainer = container;
            structureConfiguration = configuration;
            currentVisualStructure = Instantiate(roadBuildingData.RoadPrefab, transform);

            LoadDefaultConfig();
        }


        public void SwapModel(StructureContainer container, RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            structureContainer = container;
            structureConfiguration = container.DefaultStructureConfiguration;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var structure = Instantiate(roadBuildingData.RoadPrefab, transform);
            structure.transform.localPosition = new Vector3(0, 0, 0);
            structure.transform.localRotation = rotation;

            LoadDefaultConfig();
        }


        private void SetUpgradeStage()
        {
        }


        private void LoadDefaultConfig()
        {
            if (structureConfiguration.TypeConFiguration == ConfigType.Functional)
            {
                SetUpgradeStage();
            }
        }
    }
}