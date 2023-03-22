using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GDev
{
    public class BuildManager : Singleton<BuildManager>
    {
        [Header("Prev Mesh Setting")]
        Mesh buildingPreviewMesh;
        [SerializeField] Material buildingPreviewMat;
        [SerializeField] Material disabledBuildingPreviewMat;

        [Header("Build Object Setting")]
        BuildingItem currentBuildItem;
        GameObject currentBuildObject;
        [SerializeField] GameObject alertNotier;

        [Header("Build Object Rotation Setting")]
        float meshRotation;
        [SerializeField] float degreeMeshRotation;

        UIBuildingManager uiBuildingManager;

        bool isHoverObject;
        private bool isPlacing = false;
        private bool canBuild = true;

        private void Start()
        {
            uiBuildingManager = FindObjectOfType<UIBuildingManager>();
        }
        private void Update()
        {
            if (isPlacing)
            {
                HandleBuild();
                isHoverObject = Helper.IsHoverUI();
            }
        }
        public void SetBuilding(BuildingItem buildingItem)
        {
            currentBuildItem = buildingItem;
            currentBuildObject = currentBuildItem.Model;
            buildingPreviewMesh = currentBuildObject.transform.GetComponent<MeshFilter>().sharedMesh;
            CameraHandler.Instance.SetCanZoom(false);
            canBuild = InventoryManager.Instance.HaveResource(currentBuildItem.resocureCost);
            isPlacing = true;
        }

        #region Job
        private void StartJob(Vector3 position)
        {
            if (!canBuild)
            {
                Alert("RISORSE INSUFFICIENTI", Color.red);
                return;
            }
            if (SelectorManager.Instance.selectedCharacters.Count == 0)
            {
                Alert("SELEZIONA PERSONAGGI", Color.yellow);
                return;
            }

            InventoryManager.Instance.TakeResource(currentBuildItem.resocureCost);
            CameraHandler.Instance.SetCanZoom(true);
            position.y = currentBuildObject.transform.position.y;
            Quaternion objectRotation = Quaternion.Euler(0, meshRotation, 0);
            GameObject building = Instantiate(currentBuildObject, position, objectRotation);
            building.transform.localScale = Vector3.zero;
            foreach (Character character in SelectorManager.Instance.selectedCharacters)
            {
                character.DoJob(JobType.BuildingItem, Helper.MousePosition(), null, building.GetComponent<Building>());
            }
            currentBuildObject = null;
            currentBuildItem = null;
        }
        private void HandleBuild()
        {
            if (currentBuildObject == null)
                return;
            Vector3 position = Helper.MousePosition();
            InitDrawMesh(position, Quaternion.Euler(0, meshRotation, 0));
            HandleRotateMeshinDegree();
            if (Mouse.current.leftButton.wasPressedThisFrame && !isHoverObject)
            {
                Transform ray = Helper.CameraRay().transform;
                if((ray?.gameObject?.layer ?? 0) == 9 )
                {
                    StartJob(position);
                    isPlacing = false;
                    CameraHandler.Instance.SetCanZoom(true);
                }
            }
        }
        #endregion
        #region Utils
        private void InitDrawMesh(Vector3 position, Quaternion rotation)
        {
            Vector3 prevPosition = position;
            Quaternion prevrRotation = rotation;
            Vector3 scale = currentBuildObject.transform.localScale;

            Matrix4x4 matrix = Matrix4x4.TRS(prevPosition, prevrRotation, scale);

            Material currentMeshMaterial = (isHoverObject || !canBuild) ? disabledBuildingPreviewMat : buildingPreviewMat;
            Graphics.DrawMesh(buildingPreviewMesh, matrix, currentMeshMaterial, 9);
        }
        private void Alert(string message, Color textColor)
        {
            TMP_Text text = alertNotier.GetComponent<TMP_Text>();
            text.text = message;
            text.color = textColor;

            GameObject alertInstance = Instantiate(alertNotier, uiBuildingManager.transform.parent);
            alertInstance.transform.DOMove(alertInstance.transform.position + new Vector3(.5f, alertInstance.transform.position.y, alertInstance.transform.position.z), 6f).OnComplete(() =>
            {
                Destroy(alertInstance);
            });
        }
        private void HandleRotateMeshinDegree()
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            float finalRotation = 0;
            if (scroll != 0)
                finalRotation = Mathf.Sign(scroll);

            meshRotation += finalRotation * degreeMeshRotation;
        }
        #endregion
    }
}
