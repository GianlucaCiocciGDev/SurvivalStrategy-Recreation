using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDev
{
    public class SelectorManager : Singleton<SelectorManager>
    {
        [SerializeField] public GameObject character;
        [SerializeField] int numberOfCharacterInScene;

        [SerializeField] Transform selector = default;
        [SerializeField] Transform pointerMarker = default;
        Vector3 startSelectorPoint;
        Vector3 endSelectorPoint;
        Vector3 dragCenter;
        Vector3 dragSize;

        private bool isSelectingAction;
        public List<Character> selectedCharacters = new List<Character>();

        private bool isHoverObject;

        private void Start()
        {
            Vector3 cameraPosition = Helper.camera.transform.position;
            cameraPosition.y = 0;

            for (int i = 0; i < numberOfCharacterInScene; i++)
            {
                var characterModel = Instantiate(character, character.transform.position + (transform.right * (3 + i)), Quaternion.identity);
                characterModel.transform.LookAt(cameraPosition);
            }
        }
        void Update()
        {
            isHoverObject = Helper.IsHoverUI();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startSelectorPoint = Helper.MousePosition();
                endSelectorPoint = startSelectorPoint;
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                endSelectorPoint = Helper.MousePosition();
                float selectorAreaSize = (endSelectorPoint - startSelectorPoint).magnitude;
                if (selectorAreaSize > 1)
                {
                    selector.gameObject.SetActive(true);
                    isSelectingAction = true;
                    dragCenter = (startSelectorPoint + endSelectorPoint) / 2;
                    dragSize = (endSelectorPoint - startSelectorPoint);
                    selector.transform.position = dragCenter;
                    selector.transform.localScale = dragSize + Vector3.up;
                }
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (isSelectingAction)
                {
                    SelectCharacters();
                    isSelectingAction = false;
                    selector.gameObject.SetActive(false);
                }
                else
                {
                    SetJob();
                }
            }
        }

        #region character selection
        private void SelectCharacters()
        {
            DeselectCharacters();
            dragSize.Set(Mathf.Abs(dragSize.x / 2), 1, Mathf.Abs(dragSize.z / 2));
            RaycastHit[] hits = Physics.BoxCastAll(dragCenter, dragSize, Vector3.up, Quaternion.identity, 0, LayerMask.GetMask("Character"));
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out Character character))
                {
                    selectedCharacters.Add(character);
                    character.SelectCharacter();
                }
            }
        }
        private void DeselectCharacters()
        {
            foreach (Character character in selectedCharacters)
                character.DeselectCharacter();

            selectedCharacters.Clear();
        }
        #endregion

        #region Jobs
        private void SetJob()
        {
            if (selectedCharacters.Count == 0)
                return;

            Transform ray = Helper.CameraRay().transform;

            if (!Helper.IsHoverUI())
                SetPointerMarker(true, Helper.MousePosition());

            switch (ray?.gameObject?.layer ?? 0)
            {
                case 10:
                    return;
                case 9:
                    {
                        if (isHoverObject)
                            return;
                        foreach (Character character in selectedCharacters)
                        {
                            character.DoJob(JobType.Move, Helper.MousePosition());
                        }
                        break;
                    }
                default:
                    {
                        if (ray.TryGetComponent(out Resource resource))
                        {
                            foreach (Character character in selectedCharacters)
                            {
                                character.DoJob(JobType.TakeResources, resource.transform.position, resource);
                            }
                        }
                        SetPointerMarker(false);
                        break;
                    }
            }
        }
        #endregion

        #region utils
        private void SetPointerMarker(bool active, Vector3? position = null)
        {
            Vector3 newPosition = position ?? Vector3.zero;
            newPosition.y = 1;
            pointerMarker.transform.position = newPosition;
            pointerMarker.gameObject.SetActive(active);
        }
        #endregion
    }
}

