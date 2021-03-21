using System;
using System.Collections.Generic;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [Serializable]
    public struct UpgradeStage
    {
        [SerializeField] private GameObject gamePrefab;
        [SerializeField] private BuildingConfiguration buildingConfiguration;
        [SerializeField] private List<NecessaryResourcesData> necessaryResourcesDataList;
        
        public GameObject GameObjectPrefab => gamePrefab;

        public List<NecessaryResourcesData> NecessaryResourcesDataList => necessaryResourcesDataList;

        public BuildingConfiguration Configuration => buildingConfiguration;
    }
    
    [CreateAssetMenu(fileName = "FunctionalBuildingContainer", menuName = "GameData/FunctionalBuildingContainer")]
    public class FunctionalBuildingContainer : BuildingContainer
    {
        [SerializeField] private BuildingConfiguration defaultConfiguration;
        [SerializeField] private List<UpgradeStage> upgradeStageList;

        public List<UpgradeStage> UpgradeStageList => upgradeStageList;
        public BuildingConfiguration DefaultConfiguration => defaultConfiguration;
    }
}
