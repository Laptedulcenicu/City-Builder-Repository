using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "BuildingContainer", menuName = "GameData/BuildingContainer")]
    public class BuildingContainer : ScriptableObject
    {
        [SerializeField] int index;
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private BuildingConfiguration defaultBuildingConfiguration;
        [SerializeField] private CellType cellType;
        [SerializeField] private bool isRoad;
        [SerializeField] private bool isUpgradable;
    
        [SerializeField]  private int width=1;
        [SerializeField]  private int height=1;
        
        public GameObject DefaultPrefab => defaultPrefab;
        public int Index => index;

        public bool IsRoad => isRoad;
        public int Height => height;

        public int Width => width;
        
        public CellType CellType1 => cellType;

        public bool IsUpgradable => isUpgradable;
    }
}
