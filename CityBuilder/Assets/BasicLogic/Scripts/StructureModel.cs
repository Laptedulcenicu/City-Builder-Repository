using _CityBuilder.Scripts.Scriptable_Object;
using UnityEngine;

public class StructureModel : MonoBehaviour, INeedingRoad
{
    private BuildingContainer buildingContainer;
    private BuildingConfiguration defaultBuildingConfiguration;
    private RoadBuildingData currentRoadBuildingData;
    private CellType buildingType;
    private int upgradeState;
    private bool isUpgradable;
    float yHeight = 0;
    public Vector3Int RoadPosition { get; set; }
    public BuildingContainer Container => buildingContainer;
    public int UpgradeState => upgradeState;
    public bool IsUpgradable => isUpgradable;

    public CellType Type => buildingType;

    
    public void CreateModel( BuildingContainer container, int upgradeLevel)
    {
        isUpgradable = container.IsUpgradable;
        buildingContainer = container;
        if (container.IsUpgradable)
        {
            upgradeState = upgradeLevel;
            SetUpgradeState();
        }
        else
        {
            var structure = Instantiate(container.DefaultPrefab, transform);
            yHeight = structure.transform.position.y;
        }
    }
    
    public void CreateModel( BuildingContainer container, RoadBuildingData roadBuildingData)
    {
        buildingType = container.CellType1;
        isUpgradable = container.IsUpgradable;
        currentRoadBuildingData = roadBuildingData;
        buildingContainer = container;
        
        var structure = Instantiate(currentRoadBuildingData.RoadPrefab, transform);
        yHeight = structure.transform.position.y;
  
        
    }

    private void SetUpgradeState()
    {
        FunctionalBuildingContainer functionalBuildingContainer = (FunctionalBuildingContainer) Container;
        
        
    }
    
    public void SwapModel(BuildingContainer container,RoadBuildingData roadBuildingData, Quaternion rotation)
    {
        buildingContainer = container;
        currentRoadBuildingData = roadBuildingData;
        
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        var structure = Instantiate(currentRoadBuildingData.RoadPrefab, transform);
        structure.transform.localPosition = new Vector3(0, yHeight, 0);
        structure.transform.localRotation = rotation;
    }
}
