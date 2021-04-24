using System;
using System.Collections.Generic;
using UnityEngine;

namespace _CityBuilder.Scripts.Scriptable_Object.Containers
{
    public enum RoadType
    {
        DeadEnd = 0,
        RoadStraight = 1,
        Corner = 2,
        ThreeWay = 3,
        FourWay = 4
    }

    [Serializable]
    public class RoadBuildingData
    {
        [SerializeField] private int indexRoad;
        [SerializeField] private RoadType roadType;
        [SerializeField] private GameObject roadPrefab;

        public int IndexRoad => indexRoad;

        public RoadType RoadType1 => roadType;

        public GameObject RoadPrefab => roadPrefab;
    }

    [CreateAssetMenu(fileName = "Road Structure", menuName = "Structure Container/Road Structure")]
    public class RoadStructureContainer : StructureContainer
    {
        [SerializeField] private List<RoadBuildingData> roadBuildingDataList;

        public List<RoadBuildingData> RoadBuildingDataList => roadBuildingDataList;
    }
}