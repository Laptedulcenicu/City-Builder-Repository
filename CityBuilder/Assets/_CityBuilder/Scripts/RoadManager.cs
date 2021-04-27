using System.Collections.Generic;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.Test_Script;
using BasicLogic.Scripts;
using SVS;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    public PlacementManager placementManager;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>();

    private Vector3Int startPosition;
    private bool placementMode = false;

    public RoadFixer roadFixer;


    public void PlaceRoad(Vector3Int position, StructureConfiguration structureConfiguration)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
            return;
        if (placementManager.CheckIfPositionIsFree(position) == false)
            return;
        if (placementMode == false)
        {
            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;

            temporaryPlacementPositions.Add(position);

            placementManager.PlaceTemporaryStructure(position, roadFixer.Container, structureConfiguration,
                roadFixer.DeadEnd);
        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionsToFix in roadPositionsToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);
            }

            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.GetPathBetween(startPosition, position);

            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (placementManager.CheckIfPositionIsFree(temporaryPosition) == false)
                {
                    roadPositionsToRecheck.Add(temporaryPosition);
                    continue;
                }

                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.Container, structureConfiguration,
                    roadFixer.DeadEnd);
            }
        }

        FixRoadPrefabs();
    }


    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
            return;
        if (placementManager.CheckIfPositionIsFree(position) == false)
            return;
        if (placementMode == false)
        {
            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;

            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.Container,
                roadFixer.Container.DefaultStructureConfiguration, roadFixer.DeadEnd);
        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionsToFix in roadPositionsToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);
            }

            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.GetPathBetween(startPosition, position);

            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (placementManager.CheckIfPositionIsFree(temporaryPosition) == false)
                {
                    roadPositionsToRecheck.Add(temporaryPosition);
                    continue;
                }

                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.Container,
                    roadFixer.Container.DefaultStructureConfiguration, roadFixer.DeadEnd);
            }
        }

        FixRoadPrefabs();
    }


    public void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfTypeFor(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
            {
                if (roadPositionsToRecheck.Contains(roadposition) == false)
                {
                    roadPositionsToRecheck.Add(roadposition);
                }
            }
        }

        foreach (var positionToFix in roadPositionsToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }


    public void FinishPlacingRoad(bool isWithMoney)
    {
        placementMode = false;
        if (isWithMoney)
        {
            if (IsEnoughMoney())
            {
                for (int i = 0; i < temporaryPlacementPositions.Count; i++)
                {
                    foreach (var necessaryResourcesData in shopManager.SelectedShopItemContainer
                        .NecessaryResourcesDataList)
                    {
                        GameResourcesManager.AddResourceAmount(necessaryResourcesData.Resource,
                            -necessaryResourcesData.Amount);
                    }
                }


                placementManager.AddtemporaryStructuresToStructureDictionary();

                if (temporaryPlacementPositions.Count > 0)
                {
                    AudioPlayer.instance.PlayPlacementSound();
                }

                temporaryPlacementPositions.Clear();
                startPosition = Vector3Int.zero;
            }
            else
            {
                placementManager.RemoveAllTemporaryStructures();
            }
        }
        else
        {
            placementManager.AddtemporaryStructuresToStructureDictionary();

            if (temporaryPlacementPositions.Count > 0)
            {
                AudioPlayer.instance.PlayPlacementSound();
            }

            temporaryPlacementPositions.Clear();
            startPosition = Vector3Int.zero;
        }
    }

    private bool IsEnoughMoney()
    {
        List<NecessaryResourcesData> necessaryResourcesDataList = new List<NecessaryResourcesData>();

        for (int i = 0; i < temporaryPlacementPositions.Count; i++)
        {
            foreach (NecessaryResourcesData necessaryResourcesData in shopManager.SelectedShopItemContainer
                .NecessaryResourcesDataList)
            {
                if (necessaryResourcesDataList.Count <= 0)
                {
                    NecessaryResourcesData necessaryResourcesData1 = new NecessaryResourcesData();
                    necessaryResourcesData1.Initialize(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
                    necessaryResourcesDataList.Add(necessaryResourcesData1);
                }
                else
                {
                    NecessaryResourcesData necessaryResourcesData1 = necessaryResourcesDataList.Find(e => e.Resource == necessaryResourcesData.Resource);
                    if (necessaryResourcesData1 == null)
                    {
                        NecessaryResourcesData necessaryResourcesData2 = new NecessaryResourcesData();
                        necessaryResourcesData2.Initialize(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
                        necessaryResourcesDataList.Add(necessaryResourcesData2);

                    }
                    else
                    {
                        necessaryResourcesData1.Initialize(necessaryResourcesData.Resource, necessaryResourcesData1.Amount + necessaryResourcesData.Amount);
                    }
                }
            }
        }


        foreach (var necessaryResourcesData in necessaryResourcesDataList)
        {
            if (necessaryResourcesData.Amount > GameResourcesManager.GetResourceAmount(necessaryResourcesData.Resource))
            {
                return false;
            }
        }

        return true;
    }
}