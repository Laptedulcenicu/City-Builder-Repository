using System;
using System.Collections.Generic;
using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Systems.SaveSystem;
using UnityEditor;
using UnityEngine;

namespace _CityBuilder.Scripts.GlobalManager
{
    public enum ResourcesType
    {
        Gold = 0,
        Wood = 1,
        Brick = 2,
        Food = 3,
        NumberOfPopulation = 4,
        Happiness = 5,
    }

    public class ResourcesData
    {
        public ResourcesType ResourceType;
        public int ResourceAmount;
    }

    public static class GameResourcesManager
    {
        public delegate void OnGameResourcesChangeDelegate(ResourcesType resourcesType, int amount);

        public static OnGameResourcesChangeDelegate OnGameResourcesChange = delegate { };

        private static GameCurrencySettings settings;
        private static List<ResourcesData> ResourcesDataList = new List<ResourcesData>();


        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            InitializeResourcesList();
        }

        private static void InitializeResourcesList()
        {
            for (int i = 0; i < Enum.GetNames(typeof(ResourcesType)).Length; i++)
            {
                ResourcesData newResourceData = new ResourcesData()
                    {ResourceAmount = LoadResourcesAmount((ResourcesType) i), ResourceType = (ResourcesType) i};

                ResourcesDataList.Add(newResourceData);
            }
        }

        public static string GetDisplayString(ResourcesType resourcesType)
        {
            var number = GetResourceAmount(resourcesType);

            string displayString = Mathf.Abs(number) < 1f && number >= 0.1f ? $"{number:0.0}" : $"{number:0}";

            for (int i = settings.DisplaySettings.Length - 1; i >= 0; i--)
            {
                CurrencyDisplaySettings displaySettings = settings.DisplaySettings[i];

                if (number < displaySettings.Amount)
                {
                    continue;
                }

                displayString = $"{Math.Truncate(10 * number / displaySettings.Amount) / 10:0.0}" +
                                displaySettings.Suffix;

                break;
            }

            return displayString;
        }

        public static int GetResourceAmount(ResourcesType resourcesType)
        {
            ResourcesData currentResource = ResourcesDataList.Find(e => e.ResourceType == resourcesType);
            return currentResource.ResourceAmount;
        }

        public static void SetResourceAmount(ResourcesType resourcesType, int value)
        {
            ResourcesData currentResource = ResourcesDataList.Find(e => e.ResourceType == resourcesType);
            currentResource.ResourceAmount += value;
            OnGameResourcesChange(resourcesType, currentResource.ResourceAmount);
        }

        private static int LoadResourcesAmount(ResourcesType resourcesType)
        {
            return  SaveManager.Load(resourcesType.ToString() ,0);
        }
    }
}