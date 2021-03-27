using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "BuildingContainer", menuName = "GameData/BuildingContainer")]
    public class BuildingContainer : ScriptableObject
    {
        
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private bool isRoad;
        [Range(0,1)] public float weight=0.25f;
    
        [SerializeField]  private int width=1;
        [SerializeField]  private int height=1;
        
        public GameObject DefaultPrefab => defaultPrefab;
        public float Weight => weight;

        public bool IsRoad => isRoad;
        public int Height => height;

        public int Width => width;
    }
}
