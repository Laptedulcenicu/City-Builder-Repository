using System.Collections.Generic;
using System.Linq;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using UnityEngine;

namespace BasicLogic.Scripts
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
        private Grid placementGrid;
        private Dictionary<Vector3Int, Structure> temporaryRoadobjects = new Dictionary<Vector3Int, Structure>();
        private Dictionary<Vector3Int, Structure> structureDictionary = new Dictionary<Vector3Int, Structure>();

        public int Width => width;
        public int Height => height;


        private void Awake()
        {
            placementGrid = new Grid(Width, Height);
        }

        internal CellType[] GetNeighbourTypesFor(Vector3Int position)
        {
            return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
        }

        internal bool CheckIfPositionInBound(Vector3Int position)
        {
            if (position.x >= 0 && position.x < Width && position.z >= 0 && position.z < Height)
            {
                return true;
            }

            return false;
        }

        internal void PlaceObjectOnTheMap(Vector3Int position, StructureContainer structureContainer,
            StructureConfiguration defaultConfig)
        {
            Structure structure = CreateANewStructureModel(position, structureContainer, defaultConfig);

            structureDictionary.Add(position, structure);
            for (int x = 0; x < structureContainer.Width; x++)
            {
                for (int z = 0; z < structureContainer.Height; z++)
                {
                    var newPosition = position + new Vector3Int(x, 0, z);
                    placementGrid[newPosition.x, newPosition.z] = structureContainer.CellTypeStructure;

                    DestroyNatureAt(newPosition);
                }
            }
        }

        private Vector3Int? GetNearestRoad(Vector3Int position, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var newPosition = position + new Vector3Int(x, 0, y);
                    var roads = GetNeighboursOfTypeFor(newPosition, CellType.Road);
                    if (roads.Count > 0)
                    {
                        return roads[0];
                    }
                }
            }

            return null;
        }

        private void DestroyNatureAt(Vector3Int position)
        {
            RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f),
                transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
            foreach (var item in hits)
            {
                Destroy(item.collider.gameObject);
            }
        }

        internal bool CheckIfPositionIsFree(Vector3Int position)
        {
            return CheckIfPositionIsOfType(position, CellType.Empty);
        }

        private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
        {
            return placementGrid[position.x, position.z] == type;
        }

        internal void PlaceTemporaryStructure(Vector3Int position, StructureContainer structureContainer,
            RoadBuildingData roadBuildingData)
        {
            placementGrid[position.x, position.z] = structureContainer.CellTypeStructure;
            Structure structure = CreateANewStructureModel(position, structureContainer,
                structureContainer.DefaultStructureConfiguration, roadBuildingData);
            temporaryRoadobjects.Add(position, structure);
        }

        internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
        {
            List<Point> neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
            List<Vector3Int> neighbours = new List<Vector3Int>();
            foreach (var point in neighbourVertices)
            {
                neighbours.Add(new Vector3Int(point.X, 0, point.Y));
            }

            return neighbours;
        }

        private Structure CreateANewStructureModel(Vector3Int position, StructureContainer structureContainer,
            StructureConfiguration defaultConfig, RoadBuildingData roadBuildingData)
        {
            GameObject structure = new GameObject(structureContainer.CellTypeStructure.ToString());
            structure.AddComponent<BoxCollider>();
            structure.transform.SetParent(transform);
            structure.transform.localPosition = position;
            var structureModel = structure.AddComponent<Structure>();
            structureModel.CreateModel(structureContainer, defaultConfig, roadBuildingData);
            return structureModel;
        }

        private Structure CreateANewStructureModel(Vector3Int position, StructureContainer structureContainer,
            StructureConfiguration defaultConfig)
        {
            GameObject structure = new GameObject(structureContainer.CellTypeStructure.ToString());
            structure.AddComponent<BoxCollider>();
            structure.transform.SetParent(transform);
            structure.transform.localPosition = position;
            var structureModel = structure.AddComponent<Structure>();
            structureModel.CreateModel(structureContainer, defaultConfig);
            return structureModel;
        }

        internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
        {
            var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z),
                new Point(endPosition.x, endPosition.z));
            List<Vector3Int> path = new List<Vector3Int>();
            foreach (Point point in resultPath)
            {
                path.Add(new Vector3Int(point.X, 0, point.Y));
            }

            return path;
        }

        internal void RemoveAllTemporaryStructures()
        {
            foreach (var structure in temporaryRoadobjects.Values)
            {
                var position = Vector3Int.RoundToInt(structure.transform.position);
                placementGrid[position.x, position.z] = CellType.Empty;
                Destroy(structure.gameObject);
            }

            temporaryRoadobjects.Clear();
        }

        internal void AddtemporaryStructuresToStructureDictionary()
        {
            foreach (var structure in temporaryRoadobjects)
            {
                structureDictionary.Add(structure.Key, structure.Value);
                DestroyNatureAt(structure.Key);
            }

            temporaryRoadobjects.Clear();
        }

        public void ModifyStructureModel(Vector3Int position, StructureContainer structureContainer,
            RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            if (temporaryRoadobjects.ContainsKey(position))
                temporaryRoadobjects[position].SwapModel(structureContainer, roadBuildingData, rotation);
            else if (structureDictionary.ContainsKey(position))
                structureDictionary[position].SwapModel(structureContainer, roadBuildingData, rotation);
        }

        private Structure GetStructureAt(Point point)
        {
            if (point != null)
            {
                return structureDictionary[new Vector3Int(point.X, 0, point.Y)];
            }

            return null;
        }

        public Structure GetStructureAt(Vector3Int position)
        {
            if (structureDictionary.ContainsKey(position))
            {
                return structureDictionary[position];
            }

            return null;
        }

        public CellType GetCellType(int x, int y)
        {
            return placementGrid[x, y];
        }

        public void RemoveStructureAtPosition(Point point, Structure str)
        {
            placementGrid[point.X, point.Y] = CellType.Empty;

            switch (str.Container.CellTypeStructure)
            {
                case CellType.Road:
                {
                    KeyValuePair<Vector3Int, Structure> element = new KeyValuePair<Vector3Int, Structure>();
                    foreach (KeyValuePair<Vector3Int, Structure> keyValuePair in temporaryRoadobjects)
                    {
                        if (Vector3.Distance(new Vector3(point.X, 0, point.Y),
                            new Vector3(keyValuePair.Key.x, 0, keyValuePair.Key.z)) < 1f)
                        {
                            element = keyValuePair;
                            break;
                        }
                    }

                    temporaryRoadobjects.Remove(element.Key);

                    element = structureDictionary.First(e => e.Value == str);
                    structureDictionary.Remove(element.Key);
                    break;
                }
                case CellType.Structure:
                {
                    KeyValuePair<Vector3Int, Structure> element = structureDictionary.First(e => e.Value == str);
                    structureDictionary.Remove(element.Key);
                    break;
                }
            }
        }

        public Dictionary<Vector3Int, Structure> GetAllStructures()
        {
            return structureDictionary;
        }

        public void ClearGrid()
        {
            placementGrid = new Grid(Width, Height);
            foreach (var item in structureDictionary.Values)
            {
                Destroy(item.gameObject);
            }

            structureDictionary.Clear();
        }
    }
}