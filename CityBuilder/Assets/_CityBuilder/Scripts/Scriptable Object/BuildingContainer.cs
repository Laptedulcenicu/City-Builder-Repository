using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "BuildingContainer", menuName = "GameData/BuildingContainer")]
    public class BuildingContainer : ScriptableObject
    {
        
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private bool isRoad;
        [SerializeField] [Range(0,1)] private float weight=0.25f;
        
        public GameObject DefaultPrefab => defaultPrefab;
        public float Weight => weight;

        public bool IsRoad => isRoad;
    }
}
