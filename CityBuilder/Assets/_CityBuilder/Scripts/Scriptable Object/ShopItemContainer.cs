using UnityEngine;
using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.Global_Manager;

namespace _CityBuilder.Scripts.Scriptable_Object
{
    public enum BuildingType
    {
        Common = 0,
        Decoration = 1,
        Money = 2,
        Happiness = 3,
        Social = 4,
        Attack = 5,
        Population = 6
    }

    [Serializable]
    public struct NecessaryResourcesData
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount;

        public ResourceType Resource => resourceType;

        public int Amount => amount;
    }

    [CreateAssetMenu(fileName = "ShopItem", menuName = "GameData/ShopItem")]
    public class ShopItemContainer : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField][TextArea] private string itemDescription;
        [SerializeField] private int unlockDay;
        [SerializeField] private BuildingType buildingType;
    
        [SerializeField] private Sprite sprite;
        [SerializeField] private BuildingContainer buildingContainer;

        [SerializeField] private List<NecessaryResourcesData> necessaryResourcesDataList;

        public List<NecessaryResourcesData> NecessaryResourcesDataList => necessaryResourcesDataList;
        public BuildingType BuildingType1 => buildingType;
        public int UnlockLevel => unlockDay;
        public Sprite Sprite1 => sprite; 

        public BuildingContainer Container => buildingContainer;

        public string ItemDescription => itemDescription;

        public string ItemName => itemName;

    
    }
}