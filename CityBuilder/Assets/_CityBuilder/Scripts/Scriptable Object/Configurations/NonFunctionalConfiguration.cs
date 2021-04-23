using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "NonFunctional Configuration", menuName = "Structure Configuration/NonFunctional Configuration")]
    public class NonFunctionalConfiguration:StructureConfiguration
    {
        [SerializeField] private List<NecessaryResourcesData> earnResourcesList;
        public List<NecessaryResourcesData> EarnResourcesList => earnResourcesList;

        private void Awake()
        {
            ConfigType = ConfigType.NonFunctional;
        }
        
    }
}