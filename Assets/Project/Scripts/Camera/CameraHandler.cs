using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDev
{
    public class CameraHandler : Singleton<CameraHandler>
    {
        [SerializeField] float zoomLimitUp;
        [SerializeField] float zoomLimitDown;
        [SerializeField] float cameraSpeed;
        Transform mainCamera;
        Transform cameraFollowTransform;
        Vector3 moveInput;

        Vector3 movementDirection;

        bool CanZoom = true;

        MapInput mapInput;
        protected override void Awake()
        {
            base.Awake();
            mainCamera = Helper.camera.transform;
            transform.LookAt(mainCamera);
            cameraFollowTransform = transform.GetChild(0);

            mapInput = new MapInput();

            mapInput.Player.Quit.started += OnQuitGame;
        }

        void Update()
        {
            Vector3 position = Vector3.zero;
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            if (mousePosition.x > Screen.width * 0.95f && mousePosition.x < Screen.width)
                moveInput.x = 1;
            else if (mousePosition.x < Screen.width * 0.05f && mousePosition.x > 0)
                moveInput.x = -1;
            else
                moveInput.x = 0;

            if (mousePosition.y > Screen.height * 0.95f && mousePosition.y < Screen.height)
                moveInput.z = 1;
            else if (mousePosition.y < Screen.height * 0.05f && mousePosition.y > 0)
                moveInput.z = -1;
            else
                moveInput.z = 0;

            if (Helper.IsHoverUI())
                moveInput = Vector3.zero;

            movementDirection = mainCamera.TransformDirection(moveInput);
            movementDirection.y = 0;
            transform.position += movementDirection.normalized * Time.deltaTime * cameraSpeed;

            if (CanZoom)
            {
                float mouseScrollY = Mouse.current.scroll.ReadValue().y;
                if (mouseScrollY != 0)
                {
                    cameraFollowTransform.localPosition += new Vector3(0, 0, -mouseScrollY);
                    cameraFollowTransform.localPosition = new Vector3(cameraFollowTransform.localPosition.x, cameraFollowTransform.localPosition.y, Mathf.Clamp(cameraFollowTransform.localPosition.z, zoomLimitDown, zoomLimitUp));
                }
            }
            else
                cameraFollowTransform.localPosition = Vector3.Lerp(cameraFollowTransform.localPosition, new Vector3(cameraFollowTransform.localPosition.x, cameraFollowTransform.localPosition.y, zoomLimitUp),Time.deltaTime * 5);
        }
        public void SetCanZoom(bool value) => CanZoom = value;
        private void OnQuitGame(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Application.Quit();
                    break;
            }
        }

        private void OnEnable()
        {
            mapInput.Enable();
        }
        private void OnDisable()
        {
            mapInput.Disable();
        }
    }
}
