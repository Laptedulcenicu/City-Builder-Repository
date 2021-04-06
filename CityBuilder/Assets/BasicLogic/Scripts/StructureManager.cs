using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public PlacementManager placementManager;

    private List<BuildingContainer> buildingContainerList= new List<BuildingContainer>();
    private void Awake()
    {
        buildingContainerList = Resources.LoadAll<BuildingContainer>("Buildings").ToList();
    }

    public void PlaceGeneric(Vector3Int position, ShopItemContainer shopItemContainer)
    {
        if (CheckPositionBeforePlacement(position))
        {
            foreach (var necessaryResourcesData in shopItemContainer.NecessaryResourcesDataList)
            {
                if (necessaryResourcesData.Amount > GameResourcesManager.GetResourceAmount(necessaryResourcesData.Resource))
                {
                    return;
                }
            }
            
            foreach (var necessaryResourcesData in shopItemContainer.NecessaryResourcesDataList)
            {
                GameResourcesManager.AddResourceAmount( necessaryResourcesData.Resource, -necessaryResourcesData.Amount);
            }
            
            
            if(CheckBigStructure(position, shopItemContainer.Container.Width , shopItemContainer.Container.Height))
            {
                //int randomIndex = GetRandomWeightedIndex(houseWeights);
                placementManager.PlaceObjectOnTheMap(position, shopItemContainer.Container.DefaultPrefab, CellType.Structure, shopItemContainer.Container.Width,shopItemContainer.Container.Height,  shopItemContainer.Container.Index);
                AudioPlayer.instance.PlayPlacementSound();
            }
            
           
        }
    }
    
    private bool CheckBigStructure(Vector3Int position, int width, int height)
    {
        bool nearRoad = false;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newPosition = position + new Vector3Int(x, 0, z);
                
                if (DefaultCheck(newPosition)==false)
                {
                    return false;
                }
                if (nearRoad == false)
                {
                    nearRoad = RoadCheck(newPosition);
                }
            }
        }
        return nearRoad;
    }
    
    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if (DefaultCheck(position) == false)
        {
            return false;
        }

        if (RoadCheck(position) == false)
            return false;
        
        return true;
    }

    private bool RoadCheck(Vector3Int position)
    {
        if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count <= 0)
        {
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }

    private bool DefaultCheck(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            //Debug.Log("This position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position) == false)
        {
            //Debug.Log("This position is not EMPTY");
            return false;
        }
        return true;
    }

    internal void PlaceLoadedStructure(Vector3Int position, int buildingPrefabindex)
    {
        placementManager.PlaceObjectOnTheMap(position,buildingContainerList.Find(e=>e.Index==buildingPrefabindex).DefaultPrefab, CellType.Structure, buildingPrefabIndex: buildingPrefabindex);
    }

    public Dictionary<Vector3Int, StructureModel> GetAllStructures()
    {
        return placementManager.GetAllStructures();
    }
    
    

    public void ClearMap()
    {
        placementManager.ClearGrid();
    }
}
