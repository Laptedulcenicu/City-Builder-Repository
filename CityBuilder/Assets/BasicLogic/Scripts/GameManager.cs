using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object;
using _CityBuilder.Scripts.Test_Script;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.ConstantValues;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ShopManager ShopManager;

    public RoadManager roadManager;
    public InputManager inputManager;
    public StructureManager structureManager;
    public ObjectDetector objectDetector;

    void Start()
    {
        inputManager.OnEscape += HandleEscape;
    }

    private void HandleEscape()
    {
        ClearInputActions();
    }

    public void GenericPlacementHandler(ShopItemContainer shopItemContainer)
    {
        if (shopItemContainer.Container.IsRoad)
        {
            RoadPlacementHandler();
        }
        else
        {
            ClearInputActions();

            inputManager.OnMouseClick += (pos) =>
            {
                ProcessInputAndCall(structureManager.PlaceGeneric, pos, shopItemContainer);
            };
            inputManager.OnEscape += HandleEscape;
        }
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) => { ProcessInputAndCall(roadManager.PlaceRoad, pos); };
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        inputManager.OnMouseHold += (pos) => { ProcessInputAndCall(roadManager.PlaceRoad, pos); };
        inputManager.OnEscape += HandleEscape;
    }

    public void ClearInputActions()
    {
        inputManager.ClearEvents();
    }

    private void ProcessInputAndCall(Action<Vector3Int> callback, Ray ray)
    {
        Vector3Int? result = objectDetector.RaycastGround(ray);
        if (result.HasValue)
            callback.Invoke(result.Value);
    }

    private void ProcessInputAndCall(Action<Vector3Int, ShopItemContainer> callback, Ray ray,
        ShopItemContainer shopItemContainer)
    {
        Vector3Int? result = objectDetector.RaycastGround(ray);
        if (result.HasValue)
            callback.Invoke(result.Value, shopItemContainer);
    }

    public void SaveGame()
    {
        List<SaveValue> saveList = new List<SaveValue>();
        
        for (int width = 0; width < structureManager.placementManager.Width; width++)
        {
            for (int height = 0; height < structureManager.placementManager.Height; height++)
            {
                SaveValue newSaveValue = new SaveValue();


                newSaveValue.position = new Vector3Int(width, 0, height);

                StructureModel intermediaryStructure =
                    structureManager.placementManager.GetStructureAt(newSaveValue.position);


                if (intermediaryStructure)
                {
                    newSaveValue.buildingPrefabindex = intermediaryStructure.BuildingPrefabIndex;
                    newSaveValue.buildingType = structureManager.placementManager.GetCellType(width, height);
                }

                saveList.Add(newSaveValue);
            }
        }
        
        SaveManager.Save(SaveKeys.Cell, saveList);
    }

    public void LoadGame()
    {
        structureManager.ClearMap();

        List<SaveValue> newSaveValue = SaveManager.Load(SaveKeys.Cell,new List<SaveValue>());
        
        foreach (var saveValue in newSaveValue)
        {
            if (saveValue.buildingType != CellType.Empty)
            {
                Vector3Int position = Vector3Int.RoundToInt(saveValue.position);
                if (saveValue.buildingType == CellType.Road)
                {
                    roadManager.PlaceRoad(position);
                    roadManager.FinishPlacingRoad();
                }
                else
                {
                    structureManager.PlaceLoadedStructure(position, saveValue.buildingPrefabindex);
                }
            }

        }
    }

    [Serializable]
    public class SaveValue
    {
        public int buildingPrefabindex;
        public CellType buildingType;
        public Vector3Int position;
    }
}