using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [Serializable]
    public class EarnResourcesDelayData
    {
        [SerializeField] public int timerSecondsValue;
        [SerializeField] private NecessaryResourcesData earnResources;
        public NecessaryResourcesData EarnResources => earnResources;

        public void Initialize(int secondTime, NecessaryResourcesData necessaryResourcesData)
        {
            timerSecondsValue = secondTime;
            earnResources = new NecessaryResourcesData();
            earnResources.Initialize(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
        }
    }

    [CreateAssetMenu(fileName = "Functional Configuration",
        menuName = "Structure Configuration/Functional Configuration")]
    public class FunctionalConfiguration : StructureConfiguration
    {
        [HideInInspector] public int currentUpgradeLevel;
        [SerializeField] private List<EarnResourcesDelayData> earnResourcesDelayDataList;

        public List<EarnResourcesDelayData> EarnResourcesDelayDataList => earnResourcesDelayDataList;

        private void Awake()
        {
            ConfigType = ConfigType.Functional;
        }

        public FunctionalConfiguration(StructureConfiguration structure) : base(structure)
        {
            FunctionalConfiguration config = (FunctionalConfiguration) structure;
            currentUpgradeLevel = config.currentUpgradeLevel;
            earnResourcesDelayDataList = new List<EarnResourcesDelayData>();
            
            foreach (EarnResourcesDelayData earnResourcesDelayData in config.earnResourcesDelayDataList)
            { 
                EarnResourcesDelayData newResource = new EarnResourcesDelayData();
                NecessaryResourcesData necessaryResourcesData = new NecessaryResourcesData();
                necessaryResourcesData.Initialize(earnResourcesDelayData.EarnResources.Resource, earnResourcesDelayData.EarnResources.Amount);
                newResource.Initialize(earnResourcesDelayData.timerSecondsValue, necessaryResourcesData);
                earnResourcesDelayDataList.Add(newResource);
            }

        }
    }
}