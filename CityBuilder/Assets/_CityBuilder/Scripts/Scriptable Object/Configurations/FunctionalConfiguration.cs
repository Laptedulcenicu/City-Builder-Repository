using System;
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
        [HideInInspector] public int currentLevel;
        [SerializeField] private EarnResourcesDelayData earnResourcesDelayDataList;

        public EarnResourcesDelayData EarnResourcesDelayDataList => earnResourcesDelayDataList;

        private void Awake()
        {
            ConfigType = ConfigType.Functional;
        }
    }
}