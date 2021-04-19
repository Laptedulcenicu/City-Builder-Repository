using System.Collections.Generic;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    public class NatureConfiguration : StructureConfiguration
    {
        [SerializeField] private int destroyPrice;
        [SerializeField] private List<NecessaryResourcesData> earnResourcesList;

        public int DestroyPrice => destroyPrice;

        public List<NecessaryResourcesData> EarnResourcesList => earnResourcesList;
    }
}