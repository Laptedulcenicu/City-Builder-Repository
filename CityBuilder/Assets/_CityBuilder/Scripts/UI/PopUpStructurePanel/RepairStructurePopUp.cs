using System.Collections.Generic;
using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.StructureModel;
using _CityBuilder.Scripts.Test_Script;

namespace _CityBuilder.Scripts.UI.PopUpStructurePanel
{
    public class RepairStructurePopUp : InfoStructurePopUp
    {
        private List<NecessaryResourcesData> repairResourceData = new List<NecessaryResourcesData>();

        protected override void ConfirmButton()
        {
            foreach (NecessaryResourcesData necessaryResourcesData in repairResourceData)
            {
                GameResourcesManager.AddResourceAmount(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
            }

            LifeStatusData lifeStatusData = infoBuildingPanel.CurrentStructure.Configuration.StatusData;
            lifeStatusData.Initialize(lifeStatusData.MAXHealth, lifeStatusData.MAXHealth);

            CancelButton();
            infoBuildingPanel.CancelButton();
        }

        public override void Show()
        {
            base.Show();


            int currentLifePercent =
                (int) (infoBuildingPanel.CurrentStructure.Configuration.StatusData.MAXHealth *
                       infoBuildingPanel.PercentOffsetRepairPrice) / infoBuildingPanel.CurrentStructure.Configuration
                    .StatusData.CurrentHealth;

            repairResourceData.Clear();

            foreach (NecessaryResourcesData necessaryResourcesData in infoBuildingPanel.CurrentShopItem
                .NecessaryResourcesDataList)
            {
                NecessaryResourcesData repairResourcesData = new NecessaryResourcesData();
                repairResourcesData.Initialize(necessaryResourcesData.Resource,
                    necessaryResourcesData.Amount * currentLifePercent);
                repairResourceData.Add(repairResourcesData);
            }

            foreach (NecessaryResourcesData necessaryResourcesData in repairResourceData)
            {
                MoneyTextData moneyTextData = moneyList.Find(e => e.resourceType == necessaryResourcesData.Resource);
                print(moneyTextData.moneyAmount.transform.parent.gameObject.name);
                moneyTextData.moneyAmount.transform.parent.gameObject.SetActive(true);
                moneyTextData.moneyAmount.color = necessaryResourcesData.Amount < 0
                    ? infoStructurePopUpList.LoseMoney
                    : infoStructurePopUpList.EarnMoney;
                moneyTextData.moneyAmount.text = necessaryResourcesData.Amount.ToString();
            }
        }
    }
}