using UnityEditor;
using UnityEngine;

namespace _CityBuilder.Scripts.Utilities.Editor
{
    public class ChangePositionUtilities : EditorWindow
    {
        [SerializeField] private Transform targetObject;

        [MenuItem("Utilities/Change Position")]
        private static void Init()
        {
            ChangePositionUtilities window =
                (ChangePositionUtilities) EditorWindow.GetWindow(typeof(ChangePositionUtilities));
            window.Show();
        }

        private void OnGUI()
        {
            targetObject =
                EditorGUILayout.ObjectField("Target Object", targetObject, typeof(Transform), true) as Transform;

            if (GUILayout.Button("Change Position"))
            {
                ChangePosition();
            }
        }


        private void ChangePosition() 
        {
            Vector3 center = targetObject.GetComponent<Renderer>().bounds.center;
            float radius = targetObject.GetComponent<Renderer>().bounds.extents.magnitude;
         //   targetObject.transform.position = new Vector3()
            Debug.Log(center);
            Debug.Log(radius);
        }
    }
}