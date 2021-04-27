using System;
using System.Collections;
using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [Serializable]
    public struct EarnResourcesDelayData
    {
        [SerializeField] public int timerValue;
        [SerializeField] private NecessaryResourcesData earnResources;

        public int TimerValue => timerValue;

        public NecessaryResourcesData EarnResources => earnResources;
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
            earnResourcesDelayDataList = new List<EarnResourcesDelayData>(config.earnResourcesDelayDataList);
        }
    }
}