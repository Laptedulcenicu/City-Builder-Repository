using _CityBuilder.Scripts.Global_Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CityBuilder.Scripts.TestScript
{
   public class ButtonMoney : MonoBehaviour
   {
      [FormerlySerializedAs("resourcesType")] [SerializeField] private ResourceType resourceType;
      [SerializeField] private int amountValue;

      public void AddMoney()
      {
         GameResourcesManager.AddResourceAmount(resourceType,amountValue);
      }
   }
}
