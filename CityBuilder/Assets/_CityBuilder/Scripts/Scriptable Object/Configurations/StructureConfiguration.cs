using _CityBuilder.Scripts.StructureModel;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    public enum ConfigType
    {
        Functional=0,
        NonFunctional=1,
        Natural=2,
    }
    public class StructureConfiguration : ScriptableObject
    {
        [SerializeField] protected LifeStatusData lifeStatusData;
        
        protected ConfigType ConfigType;
        public LifeStatusData StatusData => lifeStatusData;

        public ConfigType TypeConFiguration => ConfigType;
    }
}
 