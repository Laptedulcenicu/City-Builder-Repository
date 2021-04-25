﻿using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "NonFunctional Configuration",
        menuName = "Structure Configuration/NonFunctional Configuration")]
    public class NonFunctionalConfiguration : StructureConfiguration
    {
        private void Awake()
        {
            ConfigType = ConfigType.NonFunctional;
        }

        public NonFunctionalConfiguration(StructureConfiguration structure) : base(structure)
        {
            Debug.Log("NonFunctionalConfiguration");
        }
    }
}