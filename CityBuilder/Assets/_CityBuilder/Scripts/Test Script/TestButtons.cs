using _CityBuilder.Scripts.Global_Manager;
using UniStorm;
using UnityEngine;

namespace _CityBuilder.Scripts.Test_Script
{
    public class TestButtons : MonoBehaviour
    {
        [SerializeField] private UniStormSystem uniStormSystem;


      public void AddMoney()
      {
          GameResourcesManager.AddResourceAmount(ResourceType.Gold, 10000 );
          GameResourcesManager.AddResourceAmount(ResourceType.Brick, 10000 );
          GameResourcesManager.AddResourceAmount(ResourceType.Wood, 10000 );
          GameResourcesManager.AddResourceAmount(ResourceType.Food, 10000 );
      }

      public void ChangePrecipitation()
      {
          var randomWeather = uniStormSystem.AllWeatherTypes[Random.Range(0, uniStormSystem.AllWeatherTypes.Count-1)];
          uniStormSystem.ChangeWeather(randomWeather);
          print(randomWeather.name);
      }
    }
}
