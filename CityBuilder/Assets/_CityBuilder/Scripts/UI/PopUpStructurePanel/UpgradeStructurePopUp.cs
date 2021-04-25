using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Configurations;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.Test_Script;

namespace _CityBuilder.Scripts.UI.PopUpStructurePanel
{
    public class UpgradeStructurePopUp : InfoStructurePopUp
    {
        private int currentLevel;
        private FunctionalStructureContainer currentContainer;

        protected override void ConfirmButton()
        {
            foreach (NecessaryResourcesData necessaryResourcesData in currentContainer.UpgradeStageList[currentLevel].NecessaryResourcesDataList)
            {
                GameResourcesManager.AddResourceAmount(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
            }

            infoBuildingPanel.CurrentStructure.SetUpgradeStage(currentLevel);
            infoBuildingPanel.CurrentUpgradeLevel();
            CancelButton();
        }

        public override void Show()
        {
            base.Show();
            //TODO Show The result of the future upgrade


            FunctionalConfiguration currentFunctionalConfiguration = (FunctionalConfiguration) infoBuildingPanel.CurrentStructure.Configuration;
            currentContainer = (FunctionalStructureContainer) infoBuildingPanel.CurrentStructure.Container;
            currentLevel = currentFunctionalConfiguration.currentUpgradeLevel;
            currentLevel++;
            
            foreach (NecessaryResourcesData necessaryResourcesData in currentContainer.UpgradeStageList[currentLevel].NecessaryResourcesDataList)
            {
                MoneyTextData moneyTextData = moneyList.Find(e => e.resourceType == necessaryResourcesData.Resource);
                
                moneyTextData.moneyAmount.transform.parent.gameObject.SetActive(true);
                moneyTextData.moneyAmount.color = necessaryResourcesData.Amount < 0 ? infoStructurePopUpList.LoseMoney : infoStructurePopUpList.EarnMoney;
                moneyTextData.moneyAmount.text = necessaryResourcesData.Amount.ToString();
            }
        }
    }
}