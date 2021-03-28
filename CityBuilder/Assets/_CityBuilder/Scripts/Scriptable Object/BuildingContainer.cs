using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "BuildingContainer", menuName = "GameData/BuildingContainer")]
    public class BuildingContainer : ScriptableObject
    {
        [SerializeField] int index;
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private bool isRoad;
    
    
        [SerializeField]  private int width=1;
        [SerializeField]  private int height=1;
        
        public GameObject DefaultPrefab => defaultPrefab;
        public int Index => index;

        public bool IsRoad => isRoad;
        public int Height => height;

        public int Width => width;
    }
}
