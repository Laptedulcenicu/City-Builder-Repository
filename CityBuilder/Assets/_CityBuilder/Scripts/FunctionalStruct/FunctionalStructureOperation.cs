using System.Collections;
using System.Collections.Generic;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using TMPro;
using UnityEngine;

namespace _CityBuilder.Scripts.FunctionalStruct
{
    public class FunctionalStructureOperation : MonoBehaviour
    {
        [SerializeField] private TextMeshPro timerText;
        private Structure structure;
        private FunctionalConfiguration functionalConfiguration;
        private Coroutine operationCoroutine;
        private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        private List<EarnResourcesDelayData> referenceFunctionalOperationList = new List<EarnResourcesDelayData>();
        public List<EarnResourcesDelayData> ReferenceFunctionalOperationList => referenceFunctionalOperationList;
        public Coroutine FunctionalOperationCoroutine => operationCoroutine;
        private int minuteMultiplayer = 60;

        public void StartOperation(List<EarnResourcesDelayData> newOperationList, Structure currentStructure)
        {
            referenceFunctionalOperationList.Clear();
            structure = currentStructure;
            functionalConfiguration = (FunctionalConfiguration) structure.Configuration;

            foreach (EarnResourcesDelayData earnResourcesDelayData in newOperationList)
            {
                EarnResourcesDelayData newResource = new EarnResourcesDelayData();
                NecessaryResourcesData necessaryResourcesData = new NecessaryResourcesData();

                necessaryResourcesData.Initialize(earnResourcesDelayData.EarnResources.Resource,
                    earnResourcesDelayData.EarnResources.Amount);
                newResource.Initialize(earnResourcesDelayData.timerSecondsValue, necessaryResourcesData);
                referenceFunctionalOperationList.Add(newResource);
            }

            operationCoroutine = StartCoroutine(OperationCoroutine());
        }

        private IEnumerator OperationCoroutine()
        {
            while (true)
            {
                yield return waitForSeconds;

                foreach (EarnResourcesDelayData earnResourcesDelayData in functionalConfiguration
                    .EarnResourcesDelayDataList)
                {
                    earnResourcesDelayData.timerSecondsValue -= 1;

                    if (earnResourcesDelayData.timerSecondsValue < 0)
                    {
                        EarnResourcesDelayData currentElement = referenceFunctionalOperationList.Find(e =>
                            e.EarnResources.Resource == earnResourcesDelayData.EarnResources.Resource);

                        earnResourcesDelayData.timerSecondsValue = currentElement.timerSecondsValue ;
                        print(earnResourcesDelayData.timerSecondsValue);

                        GameResourcesManager.AddResourceAmount(earnResourcesDelayData.EarnResources.Resource,
                            earnResourcesDelayData.EarnResources.Amount);
                    }

                    timerText.text = earnResourcesDelayData.timerSecondsValue.ToString();
                }
                
               
            }
        }
    }
}