using System.Collections.Generic;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.Test_Script;
using UnityEngine;
using UnityEngine.UI;

namespace _CityBuilder.Scripts.UI.PopUpStructurePanel
{
    public abstract class InfoStructurePopUp : MonoBehaviour
    {
        [HideInInspector] public InfoStructurePopUpList infoStructurePopUpList;
        [HideInInspector] public InfoBuildingPanel infoBuildingPanel;

        [SerializeField] protected List<MoneyTextData> moneyList;
        [SerializeField] protected Button cancelButton;
        [SerializeField] protected Button confirmButton;

        private void Awake()
        {
            cancelButton.onClick.AddListener(CancelButton);
            confirmButton.onClick.AddListener(ConfirmButton);
        }

        protected void CancelButton()
        {
            infoStructurePopUpList.ClosePopUp();
        }

        protected abstract void ConfirmButton();

        public virtual void Show()
        {
            gameObject.SetActive(true);
            foreach (MoneyTextData moneyTextData in moneyList)
            {
                moneyTextData.moneyAmount.transform.parent.gameObject.SetActive(false);
            }

           
        }
    }
}