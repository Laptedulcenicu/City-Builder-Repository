using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "NonFunctional Configuration",
        menuName = "Structure Configuration/NonFunctional Configuration")]
    public class NonFunctionalConfiguration : StructureConfiguration
    {
        [SerializeField] private List<NecessaryResourcesData> obtainResourceList;

        public List<NecessaryResourcesData> ObtainResourceList => obtainResourceList;

        private void Awake()
        {
            ConfigType = ConfigType.NonFunctional;
        }


        public NonFunctionalConfiguration(StructureConfiguration structure) : base(structure)
        {
            NonFunctionalConfiguration config = (NonFunctionalConfiguration) structure;
            obtainResourceList = new List<NecessaryResourcesData>();
            
            foreach (NecessaryResourcesData necessaryResourcesData in config.ObtainResourceList)
            {
                NecessaryResourcesData newResourcesData = new NecessaryResourcesData();

                newResourcesData.Initialize(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
                obtainResourceList.Add(newResourcesData);
            }
        }
    }
}