using System;
using _CityBuilder.Scripts.Scriptable_Object.Containers;
using UnityEngine;

namespace _CityBuilder.Scripts
{
    public class InputController : MonoBehaviour
    {
        public RoadManager roadManager;
        public InputManager inputManager;
        public StructureManager structureManager;
        public ObjectDetector objectDetector;

        private Vector3Int? mouseClickDownPosition;
        private Vector3Int? mouseClickUpPosition;
        private Vector3 cameraMouseDownPosition;
        private Vector3 cameraMouseUpPosition;

        void Start()
        {
            inputManager.OnEscape += HandleEscape;
        }

        private void HandleEscape()
        {
            ClearInputActions();
        }

        public void ActivateStructureSelection()
        {
            inputManager.OnMouseClickDown += OnMouseclickDown;
            inputManager.OnMouseClickUp += OnMouseClickUp;
        }

        private void OnMouseclickDown(Ray ray)
        {
            print("OnMouseclickDown");
            mouseClickDownPosition = objectDetector.RaycastGround(ray);
            cameraMouseDownPosition = inputManager.MainCamera.transform.position;
        }

        private void OnMouseClickUp(Ray ray)
        {
            print("OnMouseClickUp");
            mouseClickUpPosition = objectDetector.RaycastGround(ray);
            cameraMouseUpPosition = inputManager.MainCamera.transform.position;


            if (CheckMouseOffsetPositions()&& CheckCameraOffsetPosition())
            {
                ObjectFindHandler(ray);
            }
        }

        private bool CheckCameraOffsetPosition()
        {
            return Vector3.Distance(cameraMouseDownPosition, cameraMouseUpPosition) <= 1f;
        }

        private bool CheckMouseOffsetPositions()
        {
            if (mouseClickDownPosition == null || mouseClickUpPosition == null)
            {
                return false;
            }

            return Vector3.Distance((Vector3) mouseClickDownPosition, (Vector3) mouseClickUpPosition) <= 1f;
        }

        private void ObjectFindHandler(Ray ray)
        {
            GameObject result = objectDetector.RaycastAll(ray);
            if (!result) return;
            IClickable clickableObject = result.GetComponent<IClickable>();
            clickableObject?.OnClick();
        }

        public void GenericPlacementHandler(ShopItemContainer shopItemContainer)
        {
            ClearInputActions();

            if (shopItemContainer.Container.CellTypeStructure == CellType.Road)
            {
                print(shopItemContainer.Container.name);
                RoadPlacementHandler();
            }
            else
            {
                inputManager.OnMouseClick += (pos) =>
                {
                    ProcessInputAndCall(structureManager.PlaceGeneric, pos, shopItemContainer);
                };
                inputManager.OnEscape += HandleEscape;
            }
        }

        private void RoadPlacementHandler()
        {
            ClearInputActions();

            inputManager.OnMouseClick += (pos) => { ProcessInputAndCall(roadManager.PlaceRoad, pos); };
            inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
            inputManager.OnEscape += HandleEscape;
        }

        public void ClearInputActions()
        {
            inputManager.ClearEvents();
        }

        private void ProcessInputAndCall(Action<Vector3Int> callback, Ray ray)
        {
            Vector3Int? result = objectDetector.RaycastGround(ray);
            if (result.HasValue)
                callback.Invoke(result.Value);
        }

        private void ProcessInputAndCall(Action<Vector3Int, ShopItemContainer> callback, Ray ray,
            ShopItemContainer shopItemContainer)
        {
            Vector3Int? result = objectDetector.RaycastGround(ray);
            if (result.HasValue)
                callback.Invoke(result.Value, shopItemContainer);
        }
    }
}