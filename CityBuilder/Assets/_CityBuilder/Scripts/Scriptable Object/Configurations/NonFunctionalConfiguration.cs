using System.Collections.Generic;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "NonFunctional Configuration", menuName = "Structure Configuration/NonFunctional Configuration")]
    public class NonFunctionalConfiguration:StructureConfiguration
    { 
        [SerializeField] private List<NecessaryResourcesData> earnResourcesList;

        public List<NecessaryResourcesData> EarnResourcesList => earnResourcesList;
    }
}