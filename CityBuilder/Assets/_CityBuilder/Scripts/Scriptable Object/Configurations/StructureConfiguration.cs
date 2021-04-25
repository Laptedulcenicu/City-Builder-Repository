using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    public enum ConfigType
    {
        Functional = 0,
        NonFunctional = 1,
        Natural = 2,
    }

    public class StructureConfiguration : ScriptableObject
    {
        [SerializeField] protected LifeStatusData lifeStatusData;
        [SerializeField] protected List<NecessaryResourcesData> destroyEarnResourcesList;
        public List<NecessaryResourcesData> DestroyEarnResourcesList => destroyEarnResourcesList;
        protected ConfigType ConfigType;

        public LifeStatusData StatusData => lifeStatusData;

        public ConfigType TypeConFiguration => ConfigType;

        public StructureConfiguration(StructureConfiguration structure)
        {
            lifeStatusData = new LifeStatusData();
            lifeStatusData.Initialize(structure.lifeStatusData.CurrentHealth, structure.lifeStatusData.MAXHealth);
            destroyEarnResourcesList = new List<NecessaryResourcesData>(structure.destroyEarnResourcesList);
            ConfigType = structure.ConfigType;
        }
    }
}