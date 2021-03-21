using System;
using _CityBuilder.Scripts.Global_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts.TestScript
{
    public class ShowMoney : MonoBehaviour
    {
        [FormerlySerializedAs("resourcesType")] [SerializeField] private ResourceType resourceType;
        private TextMeshProUGUI currentResourceValue;

        private void Awake()
        {
            currentResourceValue = GetComponent<TextMeshProUGUI>();
            GameResourcesManager.OnGameResourcesChange += OnGameResourcesChange;
            currentResourceValue.text = GameResourcesManager.GetDisplayString(resourceType);
        }

        private void OnGameResourcesChange(ResourceType resource, int amount)
        {
            if (resourceType == resource)
            {
                currentResourceValue.text = GameResourcesManager.GetDisplayString(resourceType);
            }
        }
    }
}