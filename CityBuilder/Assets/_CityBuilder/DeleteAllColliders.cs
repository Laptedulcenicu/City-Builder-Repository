using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _CityBuilder
{
    public class DeleteAllColliders : MonoBehaviour
    {
        [ContextMenu("Do Something")]
        void Do()
        {
            List<GameObject>  allGameObject = GameObject.FindObjectsOfType<GameObject>().ToList();

            foreach (var o in allGameObject)
            {

             Collider currentCollider = o.GetComponent<Collider>();
             if (currentCollider)
             {
                 DestroyImmediate(currentCollider);
             }
            }
            
            
        }

    }
}
