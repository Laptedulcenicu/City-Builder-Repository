using System;
using System.Collections.Generic;
using _CityBuilder.Scripts.UI.PopUpStructurePanel;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts.UI
{
    public enum PopUpInfoStructureType
    {
        Destroy=0,
        Upgrade=1,
        Repair=2,
    }
    [Serializable]
    public struct PopUpData
    {
        [SerializeField] private PopUpInfoStructureType popUpType;
        [SerializeField] private InfoStructurePopUp popUp;
    
        public PopUpInfoStructureType PopUpType => popUpType;

        public InfoStructurePopUp PopUp => popUp;
    }
    public class InfoStructurePopUpList : MonoBehaviour
    {
        [SerializeField] private List<PopUpData> popUpDataList;
        [SerializeField] private GameObject bgPanel;
        [SerializeField] private InfoBuildingPanel infoBuildingPanel;
        [SerializeField] private Color loseMoney;
        [SerializeField] private Color earnMoney;

        public Color LoseMoney => loseMoney;

        public Color EarnMoney => earnMoney;

        private void Awake()
        {
            foreach (PopUpData popUpData in popUpDataList)
            {
                popUpData.PopUp.infoBuildingPanel = infoBuildingPanel;
                popUpData.PopUp.infoStructurePopUpList = this;
            }

            DisableAllPopUp();
        }

        private void DisableAllPopUp()
        {
            foreach (PopUpData upData in popUpDataList)
            {
                upData.PopUp.gameObject.SetActive(false);
            }
            bgPanel.SetActive(false);
        }

        public void ShowPopUp(PopUpInfoStructureType type)
        {
            DisableAllPopUp();
            bgPanel.SetActive(true);
            popUpDataList.Find(e=>e.PopUpType==type).PopUp.Show();
        }

        public void ClosePopUp()
        {
            DisableAllPopUp();
        }
    }
}
