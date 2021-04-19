using _CityBuilder.Scripts.Scriptable_Object;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.StructureModel
{
    public class Structure : MonoBehaviour
    {
        protected LifeStatusData LifeStatusData;
        protected StructureContainer StructureContainer;
        protected StructureConfiguration StructureConfiguration;
        public StructureContainer Container => StructureContainer;

        protected StructureConfiguration Configuration => StructureConfiguration;

        protected LifeStatusData StatusData => LifeStatusData;

        public virtual void CreateModel(StructureContainer structureContainer, StructureConfiguration structureConfiguration)
        {
            StructureContainer = structureContainer;
            StructureConfiguration = structureConfiguration;
            LifeStatusData = structureConfiguration.StatusData;
        }
        
        
        //
        // public void CreateModel(BuildingContainer container, int upgradeLevel)
        // {
        //     var structure = Instantiate(container.DefaultPrefab, transform);
        // }
        //
        // public void CreateModel(BuildingContainer container, RoadBuildingData roadBuildingData)
        // {
        //     buildingType = container.CellType1;
        //     currentRoadBuildingData = roadBuildingData;
        //     buildingContainer = container;
        //
        //     var structure = Instantiate(currentRoadBuildingData.RoadPrefab, transform);
        // }
        //
        // private void SetUpgradeState()
        // {
        //     FunctionalBuildingContainer functionalBuildingContainer = (FunctionalBuildingContainer) Container;
        // }
        //
        // public void SwapModel(BuildingContainer container, RoadBuildingData roadBuildingData, Quaternion rotation)
        // {
        //     buildingContainer = container;
        //     currentRoadBuildingData = roadBuildingData;
        //
        //     foreach (Transform child in transform)
        //     {
        //         Destroy(child.gameObject);
        //     }
        //
        //     var structure = Instantiate(currentRoadBuildingData.RoadPrefab, transform);
        //     structure.transform.localPosition = new Vector3(0, 0, 0);
        //     structure.transform.localRotation = rotation;
        // }
    }
}