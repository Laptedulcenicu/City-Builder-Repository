using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "Nature Configuration", menuName = "Structure Configuration/Nature Configuration")]
    public class NatureConfiguration : StructureConfiguration
    {
        private void Awake()
        {
            ConfigType = ConfigType.Natural;
        }

        public NatureConfiguration(StructureConfiguration structure) : base(structure)
        {
            NatureConfiguration natureConfiguration = (NatureConfiguration) structure;
        }
    }
}