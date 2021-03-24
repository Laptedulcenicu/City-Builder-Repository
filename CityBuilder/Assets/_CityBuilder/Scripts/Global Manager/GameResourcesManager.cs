using System;
using System.Collections.Generic;
using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Systems.OfflineSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace _CityBuilder.Scripts.Global_Manager
{
    public enum ResourceType
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
        public ResourceType ResourceType;
        public int ResourceAmount;
    }

    public static class GameResourcesManager
    {
        public delegate void OnGameResourcesChangeDelegate(ResourceType resourceType, int amount);

        public static OnGameResourcesChangeDelegate OnGameResourcesChange = delegate { };

        private static GameCurrencySettings settings;
        private static List<ResourcesData> ResourcesDataList = new List<ResourcesData>();


        [InvokeOnAppLaunch()]
        private static void Initialize()
        {
            settings = Resources.Load<GameCurrencySettings>(GameRigResourcesPaths.GameCurrencySettings);
            InitializeResourcesList();
            OfflineManager.OnGoToOffline += SaveResourcesAmount;
        }

        private static void SaveResourcesAmount()
        {
            foreach (var resourcesData in ResourcesDataList)
            {
                SaveManager.Save(resourcesData.ResourceType.ToString(), resourcesData.ResourceAmount);      
            }
        }

        private static void InitializeResourcesList()
        {
            for (int i = 0; i < Enum.GetNames(typeof(ResourceType)).Length; i++)
            {
                ResourcesData newResourceData = new ResourcesData()
                    {ResourceAmount = LoadResourcesAmount((ResourceType) i), ResourceType = (ResourceType) i};

                ResourcesDataList.Add(newResourceData);
            }
        }

        public static string GetDisplayString(ResourceType resourceType)
        {
            var number = GetResourceAmount(resourceType);

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
        
        public static string GetDisplayString(int value)
        {
            var number = value;

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
        

        public static int GetResourceAmount(ResourceType resourceType)
        {
            ResourcesData currentResource = ResourcesDataList.Find(e => e.ResourceType == resourceType);
            return currentResource.ResourceAmount;
        }

        public static void AddResourceAmount(ResourceType resourceType, int value)
        {
            ResourcesData currentResource = ResourcesDataList.Find(e => e.ResourceType == resourceType);
            currentResource.ResourceAmount += value;
            OnGameResourcesChange(resourceType, currentResource.ResourceAmount);
        }

        private static int LoadResourcesAmount(ResourceType resourceType)
        {
            return SaveManager.Load(resourceType.ToString(), 0);
        }
    }
}