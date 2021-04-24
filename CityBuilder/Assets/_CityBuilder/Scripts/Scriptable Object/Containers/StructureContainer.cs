using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Containers
{
    [CreateAssetMenu(fileName = "Structure", menuName = "Structure Container/Structure")]
    public class StructureContainer : ScriptableObject
    {
        [SerializeField] int index;
        [SerializeField] protected GameObject defaultPrefab;
        [SerializeField] protected StructureConfiguration defaultStructureConfiguration;
        [SerializeField] private CellType cellType;
        [SerializeField] private int width = 1;
        [SerializeField] private int height = 1;
        public GameObject DefaultPrefab => defaultPrefab;
        public StructureConfiguration DefaultStructureConfiguration => defaultStructureConfiguration;
        public int Index => index;
        public int Height => height;
        public int Width => width;
        public CellType CellTypeStructure => cellType;
    }
}