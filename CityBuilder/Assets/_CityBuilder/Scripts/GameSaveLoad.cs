using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.StructureModel;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.ConstantValues;
using UnityEngine;

namespace _CityBuilder.Scripts
{
    public class GameSaveLoad : MonoBehaviour
    {
        public RoadManager roadManager;
        public StructureManager structureManager;

        public void SaveGame()
        {
            List<SaveValue> saveList = new List<SaveValue>();

            for (int width = 0; width < structureManager.placementManager.Width; width++)
            {
                for (int height = 0; height < structureManager.placementManager.Height; height++)
                {
                    if (structureManager.placementManager.GetCellType(width, height) != CellType.Empty)
                    {
                        SaveValue newSaveValue = new SaveValue();
                        newSaveValue.position = new Vector3Int(width, 0, height);
                        Structure intermediaryStructure =
                            structureManager.placementManager.GetStructureAt(newSaveValue.position);

                        newSaveValue.buildingPrefabindex = intermediaryStructure.Container.Index;
                        newSaveValue.buildingType = structureManager.placementManager.GetCellType(width, height);
                        //  newSaveValue.upgradeState = intermediaryStructure.UpgradeState;


                        saveList.Add(newSaveValue);
                    }
                }
            }

            SaveManager.Save(SaveKeys.Cell, saveList);
        }

        public void LoadGame()
        {
            structureManager.ClearMap();

            List<SaveValue> newSaveValue = SaveManager.Load(SaveKeys.Cell, new List<SaveValue>());

            foreach (var saveValue in newSaveValue)
            {
                Vector3Int position = Vector3Int.RoundToInt(saveValue.position);
                if (saveValue.buildingType == CellType.Road)
                {
                    RoadFixer.ConfigRoadData(
                        structureManager.BuildingContainerList.Find(e => e.Index == saveValue.buildingPrefabindex));
                    roadManager.PlaceRoad(position);
                    roadManager.FinishPlacingRoad();
                }
                else
                {
                    structureManager.PlaceLoadedStructure(position, saveValue.buildingPrefabindex,
                        saveValue.upgradeState);
                }
            }
        }

        [Serializable]
        public class SaveValue
        {
            public int upgradeState;
            public int buildingPrefabindex;
            public CellType buildingType;
            public Vector3Int position;
        }
    }
}