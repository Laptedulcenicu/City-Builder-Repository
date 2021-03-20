using _CityBuilder.Scripts.GlobalManager;
using UnityEngine;

namespace _CityBuilder.Scripts.TestScript
{
   public class ButtonMoney : MonoBehaviour
   {
      [SerializeField] private ResourcesType resourcesType;
      [SerializeField] private int amountValue;

      public void AddMoney()
      {
         GameResourcesManager.AddResourceAmount(resourcesType,amountValue);
      }
   }
}
