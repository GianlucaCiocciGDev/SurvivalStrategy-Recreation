using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDev
{
    public class Resource : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] string resourceName;
        ResourceStats stats;
        [SerializeField] List<ResourceCost> resourceCost = new List<ResourceCost>();

        Renderer meshRenderer;
        [SerializeField] float blinkIntensity;
        private Color BaseColor;

        string tooltipHeader;
        string tooltipBody;
        private void Start()
        {
            stats = GetComponent<ResourceStats>();
            meshRenderer = GetComponent<Renderer>();
            BaseColor = meshRenderer.material.color;
            InitTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            meshRenderer.material.color = BaseColor * blinkIntensity;
            ShowTooltip();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            meshRenderer.material.color = BaseColor;
            TooltipSystem.Hide();
        }

        public async void Tick(int damage)
        {
            if(transform != null) {
                transform.DOComplete();
                await transform.DOShakeScale(.5f, .4f, 10, 90).SetEase(Ease.Flash).AsyncWaitForCompletion();
            }
            if(stats)
                stats.TakeDamage(damage);
        }
        public bool IsDied()
        {
            return stats.IsDied();
        }
        public List<ResourceCost> GetResourceCosts()
        {
            return resourceCost;
        }

        #region Utils
        private void InitTooltip()
        {
            tooltipHeader = $"{resourceName.ToUpper()}\r\n\r\n";
            foreach(var source in resourceCost)
            {
                tooltipBody += $"- {Helper.GetDisplayResourceName(source.ResourceType).ToUpper()}: {source.amount.ToString().ToUpper()} \r\n \r\n";
            }
        }
        private async void ShowTooltip()
        {
            await Helper.WaitForSecond(500);
            TooltipSystem.Show(tooltipBody, tooltipHeader);
        }
        #endregion
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }
}
