using System;
using System.Collections;
using System.Collections.Generic;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.ConstantValues;
using UniStorm;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _CityBuilder.Scripts.Manager
{
    [Serializable]
    public struct SeasonData
    {
        [SerializeField] private UniStormSystem.CurrentSeasonEnum season;
        [SerializeField] private List<int> seasonIndexList;

        public UniStormSystem.CurrentSeasonEnum Season => season;
        public List<int> SeasonIndexList => seasonIndexList;
    }
    public class DayTimeManager : MonoBehaviour
    {
        [SerializeField] private UniStormSystem uniStormSystem;
        [SerializeField] private List<SeasonData> seasonDataList;
        private IEnumerator Start()
        {
            yield return new  WaitForSeconds(0.2f);
            CheckCurrentDay();
        }

        private void CheckCurrentDay()
        {
            int savedDay = SaveManager.Load(SaveKeys.Day, 1);

            if (savedDay != uniStormSystem.Day)
            {
                SaveManager.Save(SaveKeys.Day, uniStormSystem.Day);
                ChangeWeather();
            }
            else
            {
                WeatherType newWeather = uniStormSystem.AllWeatherTypes[SaveManager.Load(SaveKeys.currentWeather, 0)];
                uniStormSystem.ChangeWeather(newWeather);
            }
        }

        private void ChangeWeather()
        {
            SeasonData currentSeason = seasonDataList.Find(e => e.Season == uniStormSystem.CurrentSeason);

            currentSeason.SeasonIndexList.Remove(SaveManager.Load(SaveKeys.currentWeather, 0));
            int randomIndex = currentSeason.SeasonIndexList[Random.Range(0,currentSeason.SeasonIndexList.Count )];
            SaveManager.Save(SaveKeys.currentWeather, randomIndex);
            WeatherType newWeather = uniStormSystem.AllWeatherTypes[randomIndex];
            
            uniStormSystem.ChangeWeather(newWeather);
        }
            
    }
}
