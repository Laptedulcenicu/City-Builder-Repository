using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _CityBuilder.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public event Action<Ray> OnMouseClick, OnMouseClickDown, OnMouseClickUp;

        public event Action OnMouseUp, OnEscape;

        [SerializeField] private Camera mainCamera;

        public Camera MainCamera => mainCamera;


        void Update()
        {
            CheckClickDownEvent();
            CheckClickHoldEvent();
            CheckClickUpEvent();
            CheckEscClick();
        }

        private void CheckClickHoldEvent()
        {
            if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                OnMouseClick?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
            }
        }

        private void CheckClickUpEvent()
        {
            if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                OnMouseUp?.Invoke();
                OnMouseClickUp?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
            }
        }

        private void CheckClickDownEvent()
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                OnMouseClick?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
                OnMouseClickDown?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
            }
        }

        private void CheckEscClick()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscape.Invoke();
            }
        }

        public void ClearEvents()
        {
            OnMouseClick = null;
            OnEscape = null;
            OnMouseUp = null;
            OnMouseClickDown = null;
            OnMouseClickUp = null;
        }
    }
}