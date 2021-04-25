using _CityBuilder.Scripts.Global_Manager;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using _CityBuilder.Scripts.Test_Script;

namespace _CityBuilder.Scripts.UI.PopUpStructurePanel
{
    public class DestroyStructurePopUp : InfoStructurePopUp
    {
        protected override void ConfirmButton()
        {
            foreach (NecessaryResourcesData necessaryResourcesData in infoBuildingPanel.CurrentStructure.Configuration
                .DestroyEarnResourcesList)
            {
                GameResourcesManager.AddResourceAmount(necessaryResourcesData.Resource, necessaryResourcesData.Amount);
            }

            Point point = new Point((int) infoBuildingPanel.CurrentStructure.transform.position.x,
                (int) infoBuildingPanel.CurrentStructure.transform.position.z);
            infoBuildingPanel.StructureManager1.placementManager.RemoveStructureAtPosition(point,
                infoBuildingPanel.CurrentStructure);

            Destroy(infoBuildingPanel.CurrentStructure.gameObject);

            CancelButton();
            infoBuildingPanel.CancelButton();
        }

        public override void Show()
        {
            base.Show();
            foreach (NecessaryResourcesData necessaryResourcesData in infoBuildingPanel.CurrentStructure.Configuration
                .DestroyEarnResourcesList)
            {
                MoneyTextData moneyTextData =
                    moneyList.Find(e => e.resourceType == necessaryResourcesData.Resource);
                moneyTextData.moneyAmount.transform.parent.gameObject.SetActive(true);
                moneyTextData.moneyAmount.color = necessaryResourcesData.Amount < 0
                    ? infoStructurePopUpList.LoseMoney
                    : infoStructurePopUpList.EarnMoney;
                moneyTextData.moneyAmount.text = necessaryResourcesData.Amount.ToString();
            }
        }
    }
}