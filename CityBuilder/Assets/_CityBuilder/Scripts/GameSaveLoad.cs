using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
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
                    if (structureManager.placementManager.GetCellType(width, height) != CellType.None && structureManager.placementManager.GetCellType(width, height) != CellType.Empty)
                    {
                        SaveValue newSaveValue = new SaveValue();
                        newSaveValue.position = new Vector3Int(width, 0, height);


                        Structure intermediaryStructure = structureManager.placementManager.GetStructureAt(newSaveValue.position);
                        if (intermediaryStructure != null)
                        {
                            newSaveValue.rotation = intermediaryStructure.transform.eulerAngles;

                            switch (intermediaryStructure.Configuration.TypeConFiguration)
                            {
                                case ConfigType.Functional:
                                    newSaveValue.structureConfiguration = new FunctionalConfiguration(intermediaryStructure.Configuration);
                 
                                
                                    break;
                                case ConfigType.NonFunctional:
                                    newSaveValue.structureConfiguration =
                                        new NonFunctionalConfiguration(intermediaryStructure.Configuration);
                                    break;
                                case ConfigType.Natural:
                                    newSaveValue.structureConfiguration =
                                        new NatureConfiguration(intermediaryStructure.Configuration);
                                    break;
                            }

                            newSaveValue.buildingPrefabindex = intermediaryStructure.Container.Index;
                            newSaveValue.buildingType = structureManager.placementManager.GetCellType(width, height);
                            saveList.Add(newSaveValue);
                        }
                     
                    }
                }
            }

            SaveManager.Save(SaveKeys.Cell, saveList);
        }

        public void LoadGame()
        {
            structureManager.ClearMap();

            List<SaveValue> newSaveValue = SaveManager.Load(SaveKeys.Cell, new List<SaveValue>());

            foreach (SaveValue saveValue in newSaveValue)
            {
                Vector3Int position = Vector3Int.RoundToInt(saveValue.position);
                if (saveValue.buildingType == CellType.Road)
                {
                    RoadFixer.ConfigRoadData(structureManager.BuildingContainerList.Find(e => e.Index == saveValue.buildingPrefabindex));
                    roadManager.PlaceRoad(position, saveValue.structureConfiguration);
                    roadManager.FinishPlacingRoad(false);
                }
                else
                {
                    structureManager.PlaceLoadedStructure(position, saveValue.rotation, saveValue.buildingPrefabindex,
                        saveValue.structureConfiguration);
                }
            }
        }

        [Serializable]
        public class SaveValue
        {
            public int buildingPrefabindex;
            public CellType buildingType;
            public Vector3Int position;
            public Vector3 rotation;
            public StructureConfiguration structureConfiguration;
        }
    }
}