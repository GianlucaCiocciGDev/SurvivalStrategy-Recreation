using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
using System;

namespace GDev
{
    public class UIBuildingManager : MonoBehaviour
    {
        CanvasGroup buildingBlock;
        [SerializeField] CanvasGroup prevBuildingBlock;

        [SerializeField] List<BuildingItem> items = new List<BuildingItem>();
        List<Transform> activeSlots = new List<Transform>();
        [SerializeField] Transform tableBody;

        int tableBodyChild => tableBody.childCount;
        bool isPanelClosed = true;

        private void Start()
        {
            buildingBlock = GetComponent<CanvasGroup>();
            InitPanel();
            ShowBuildingPanel();
        }
        public void ShowBuildingPanel()
        {
            if (!isPanelClosed)
                return;

            prevBuildingBlock.interactable = false;
            prevBuildingBlock.DOFade(0, .15f).SetUpdate(true).OnComplete(() =>
            {
                prevBuildingBlock.gameObject.SetActive(false);
            });

            buildingBlock.gameObject.SetActive(true);
            buildingBlock.DOFade(1 , .15f).SetUpdate(true);
            buildingBlock.interactable = true;

            AddSlotToActive();
            SpawnItem();
            
        }
        public void CloseBuildingPanel()
        {
            if (isPanelClosed)
                return;

            buildingBlock.interactable = false;
            buildingBlock.DOFade(0, .15f).SetUpdate(true).OnComplete(() =>
            {
                buildingBlock.gameObject.SetActive(false);
            });

            prevBuildingBlock.gameObject.SetActive(true);
            prevBuildingBlock.DOFade(1, .15f).SetUpdate(true);
            prevBuildingBlock.interactable = true;

            ClearButton();
            isPanelClosed = true;
        }
        #region Utils
        async void SpawnItem()
        {
            //List<Task> tasks = new List<Task>();
            foreach (var item in activeSlots)
            {
                await item.transform.DOScale(1f, .5f).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
                //tasks.Add(item.transform.DOScale(1f, .5f).SetEase(Ease.OutBounce).AsyncWaitForCompletion());
            }
            //await Task.WhenAll(tasks);
            transform.DOScale(1.2f, .1f).SetEase(Ease.InOutFlash).OnComplete(() =>
            {
                transform.DOScale(1f, .1f).SetEase(Ease.OutBounce);
                isPanelClosed = false;
            });
        }
        private void ClearButton()
        {
            foreach (var item in activeSlots)
            {
                item.localScale = Vector3.zero;
            }
            activeSlots.Clear();
        }
        private void InitButton(Transform button, BuildingItem item)
        {
            if (button != null && item != null)
            {
                button.localScale = new Vector3(0, 0, 0);
                button.GetChild(0).GetComponent<TMP_Text>().text = item.ItemName;
                button.GetChild(1).GetComponent<TMP_Text>().text = item.resocureCost[0].amount.ToString();
                button.GetChild(2).GetComponent<TMP_Text>().text = item.resocureCost[1].amount.ToString();
                button.GetChild(3).GetComponent<TMP_Text>().text = item.resocureCost[2].amount.ToString();
                button.GetComponent<Button>().onClick.AddListener(() => BuildManager.Instance.SetBuilding(item));
                activeSlots.Add(button);
            }
        }
        private void InitPanel()
        {
            for (int i = 0; i < tableBodyChild; i++)
            {
                int x = i;
                Transform current = tableBody.GetChild(x);
                InitButton(current, items[x]);
            }
        }
        private void AddSlotToActive()
        {
            for (int i = 0; i < tableBodyChild; i++)
            {
                int x = i;
                activeSlots.Add(tableBody.GetChild(x));
            }
        }
        #endregion
    }
}
