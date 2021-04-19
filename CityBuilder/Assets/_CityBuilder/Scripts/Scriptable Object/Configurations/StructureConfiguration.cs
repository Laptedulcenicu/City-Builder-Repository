using _CityBuilder.Scripts.StructureModel;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Configurations
{
    public class StructureConfiguration : ScriptableObject
    {
        [SerializeField] protected LifeStatusData lifeStatusData;

        public LifeStatusData StatusData => lifeStatusData;
    }
}
 