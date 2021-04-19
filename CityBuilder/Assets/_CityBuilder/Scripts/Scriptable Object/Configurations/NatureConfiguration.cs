﻿using System.Collections.Generic;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "Nature Configuration", menuName = "Structure Configuration/Nature Configuration")]
    public class NatureConfiguration : StructureConfiguration
    {
        [SerializeField] private int destroyPrice;
        [SerializeField] private List<NecessaryResourcesData> earnResourcesList;

        public int DestroyPrice => destroyPrice;

        public List<NecessaryResourcesData> EarnResourcesList => earnResourcesList;
    }
}