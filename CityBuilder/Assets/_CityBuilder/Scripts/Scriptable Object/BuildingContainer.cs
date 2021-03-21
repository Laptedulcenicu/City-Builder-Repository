using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "BuildingContainer", menuName = "GameData/BuildingContainer")]
    public class BuildingContainer : ScriptableObject
    {
        [SerializeField] private GameObject defaultPrefab;
        public GameObject DefaultPrefab => defaultPrefab;
    }
}
