using System;
using _CityBuilder.Scripts.GlobalManager;
using TMPro;
using UnityEngine;

namespace _CityBuilder.Scripts.TestScript
{
    public class ShowMoney : MonoBehaviour
    {
        [SerializeField] private ResourcesType resourcesType;
        private TextMeshProUGUI currentResourceValue;

        private void Awake()
        {
            currentResourceValue = GetComponent<TextMeshProUGUI>();
            GameResourcesManager.OnGameResourcesChange += OnGameResourcesChange;
            currentResourceValue.text = GameResourcesManager.GetDisplayString(resourcesType);
        }

        private void OnGameResourcesChange(ResourcesType resources, int amount)
        {
            if (resourcesType == resources)
            {
                currentResourceValue.text = GameResourcesManager.GetDisplayString(resourcesType);
            }
        }
    }
}