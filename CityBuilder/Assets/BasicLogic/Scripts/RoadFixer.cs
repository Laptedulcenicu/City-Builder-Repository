using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _CityBuilder.Scripts.Scriptable_Object;
using BasicLogic.Scripts;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public static Action<BuildingContainer> ConfigRoadData;
        
    private RoadBuildingData deadEnd, roadStraight, corner, threeWay, fourWay;
    private RoadBuildingContainer roadBuildingContainer;

    public RoadBuildingData DeadEnd => deadEnd;

    public RoadBuildingData RoadStraight => roadStraight;

    public RoadBuildingData Corner => corner;

    public RoadBuildingData ThreeWay => threeWay;

    public RoadBuildingData FourWay => fourWay;

    public RoadBuildingContainer Container => roadBuildingContainer;

    
    
    private void Awake()
    {
        ConfigRoadData += ConfigureRoadData;
    }

    private void OnDestroy()
    {
        ConfigRoadData -= ConfigureRoadData;
    }

    public void ConfigureRoadData(BuildingContainer buildingContainer )
    {
        roadBuildingContainer =(RoadBuildingContainer) buildingContainer;
        deadEnd = Container.RoadBuildingDataList.Find(e => e.RoadType1 == RoadType.DeadEnd);
        roadStraight = Container.RoadBuildingDataList.Find(e => e.RoadType1 == RoadType.RoadStraight);
        corner = Container.RoadBuildingDataList.Find(e => e.RoadType1 == RoadType.Corner);
        threeWay = Container.RoadBuildingDataList.Find(e => e.RoadType1 == RoadType.ThreeWay);
        fourWay = Container.RoadBuildingDataList.Find(e => e.RoadType1 == RoadType.FourWay);
        
    }
    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        //[right, up, left, down]
        var result = placementManager.GetNeighbourTypesFor(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        if(roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(placementManager, result, temporaryPosition);
        }else if(roadCount == 2)
        {
            if (CreateStraightRoad(placementManager, result, temporaryPosition))
                return;
            CreateCorner(placementManager, result, temporaryPosition);
        }else if(roadCount == 3)
        {
            Create3Way(placementManager, result, temporaryPosition);
        }
        else
        {
            Create4Way(placementManager, result, temporaryPosition);
        }
    }

    private void Create4Way(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        placementManager.ModifyStructureModel(temporaryPosition,Container,  FourWay, Quaternion.identity);
    }

    //[left, up, right, down]
    private void Create3Way(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if(result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, ThreeWay, Quaternion.identity);
        }else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, Container, ThreeWay, Quaternion.Euler(0,90,0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, Container, ThreeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, ThreeWay, Quaternion.Euler(0, 270, 0));
        }

    }

    //[left, up, right, down]
    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, Corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, Corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, Corner, Quaternion.Euler(0, 270, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, Corner, Quaternion.identity);
        }
    }

    //[left, up, right, down]
    private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, RoadStraight, Quaternion.identity);
            return true;
        }else if (result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, RoadStraight, Quaternion.Euler(0,90,0));
            return true;
        }
        return false;
    }

    //[left, up, right, down]
    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, DeadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result[2] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, DeadEnd, Quaternion.identity);
        }
        else if (result[3] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, DeadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result[0] == CellType.Road )
        {
            placementManager.ModifyStructureModel(temporaryPosition,Container, DeadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
