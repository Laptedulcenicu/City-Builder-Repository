using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object;
using UnityEngine;

namespace BasicLogic.Scripts
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
    
        private Grid placementGrid;

        private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
        private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

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
        internal void PlaceObjectOnTheMap(Vector3Int position, BuildingContainer buildingContainer, int upgradeState= 0)
        {
            StructureModel structure = CreateANewStructureModel(position, buildingContainer, upgradeState);

            var structureNeedingRoad = structure.GetComponent<INeedingRoad>();
            if (structureNeedingRoad != null)
            {
                structureNeedingRoad.RoadPosition = GetNearestRoad(position, buildingContainer.Width, buildingContainer.Height).Value;
            }

            structureDictionary.Add(position, structure);
            for (int x = 0; x < buildingContainer.Width; x++)
            {
                for (int z = 0; z < buildingContainer.Height; z++)
                {
                    var newPosition = position + new Vector3Int(x, 0, z);
                    placementGrid[newPosition.x, newPosition.z] = buildingContainer.CellType1;

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

        internal void PlaceTemporaryStructure(Vector3Int position, BuildingContainer buildingContainer, RoadBuildingData roadBuildingData)
        {
            placementGrid[position.x, position.z] = buildingContainer.CellType1;
            StructureModel structure = CreateANewStructureModel(position, buildingContainer, roadBuildingData);
            temporaryRoadobjects.Add(position, structure);
        }

        internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
        {
            var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
            List<Vector3Int> neighbours = new List<Vector3Int>();
            foreach (var point in neighbourVertices)
            {
                neighbours.Add(new Vector3Int(point.X, 0, point.Y));
            }

            return neighbours;
        }

        private StructureModel CreateANewStructureModel(Vector3Int position, BuildingContainer buildingContainer, RoadBuildingData roadBuildingData)
        {
            GameObject structure = new GameObject(buildingContainer.CellType1.ToString());
            structure.transform.SetParent(transform);
            structure.transform.localPosition = position;
            var structureModel = structure.AddComponent<StructureModel>();
            structureModel.CreateModel(buildingContainer, roadBuildingData);
            return structureModel;
        }
        
        
        private StructureModel CreateANewStructureModel(Vector3Int position, BuildingContainer buildingContainer, int upgradeState)
        {
            GameObject structure = new GameObject(buildingContainer.CellType1.ToString());
            structure.transform.SetParent(transform);
            structure.transform.localPosition = position;
            var structureModel = structure.AddComponent<StructureModel>();
            structureModel.CreateModel(buildingContainer, upgradeState);
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

        public void ModifyStructureModel(Vector3Int position, BuildingContainer buildingContainer, RoadBuildingData roadBuildingData, Quaternion rotation)
        {
            if (temporaryRoadobjects.ContainsKey(position))
                temporaryRoadobjects[position].SwapModel(buildingContainer,roadBuildingData, rotation);
            else if (structureDictionary.ContainsKey(position))
                structureDictionary[position].SwapModel(buildingContainer,roadBuildingData, rotation);
        }
    
        private StructureModel GetStructureAt(Point point)
        {
            if (point != null)
            {
                return structureDictionary[new Vector3Int(point.X, 0, point.Y)];
            }

            return null;
        }
    
        public StructureModel GetStructureAt(Vector3Int position)
        {
            if (structureDictionary.ContainsKey(position))
            {
                return structureDictionary[position];
            }

            return null;
        }

        public CellType GetCellType(int x, int  y)
        {
            return placementGrid[x, y];
        }

        public Dictionary<Vector3Int, StructureModel> GetAllStructures()
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