using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Containers
{
    [Serializable]
    public struct UpgradeStage
    {
        [SerializeField] private GameObject gamePrefab;
        [SerializeField] private StructureConfiguration structureConfiguration;
        [SerializeField] private List<NecessaryResourcesData> necessaryResourcesDataList;

        public GameObject GameObjectPrefab => gamePrefab;

        public List<NecessaryResourcesData> NecessaryResourcesDataList => necessaryResourcesDataList;

        public StructureConfiguration Configuration => structureConfiguration;
    }

    [CreateAssetMenu(fileName = "Functional Structure", menuName = "Structure Container/Functional Structure")]
    public class FunctionalStructureContainer : StructureContainer
    {
        [SerializeField] private List<UpgradeStage> upgradeStageList;
        public List<UpgradeStage> UpgradeStageList => upgradeStageList;

        private void Awake()
        {
            if (defaultPrefab)
            {
                defaultPrefab.hideFlags = HideFlags.HideInInspector;
            }

            if (defaultStructureConfiguration)
            {
                defaultStructureConfiguration.hideFlags = HideFlags.HideInInspector;
            }
        }
    }
}