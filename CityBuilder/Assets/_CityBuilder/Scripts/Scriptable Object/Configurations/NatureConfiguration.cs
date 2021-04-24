using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    [CreateAssetMenu(fileName = "Nature Configuration", menuName = "Structure Configuration/Nature Configuration")]
    public class NatureConfiguration : StructureConfiguration
    {
        [SerializeField] private int destroyPrice;

        public int DestroyPrice => destroyPrice;


        private void Awake()
        {
            ConfigType = ConfigType.Natural;
        }
    }
}