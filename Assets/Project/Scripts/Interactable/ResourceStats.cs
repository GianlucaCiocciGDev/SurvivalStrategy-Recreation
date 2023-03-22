using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GDev
{
    public class ResourceStats : StatsManager
    {
        Resource resource;
        [SerializeField] ParticleSystem destoryParticle;
        protected override void Start()
        {
            base.Start();
            resource = GetComponent<Resource>();
        }
        protected override void HandleDeath()
        {
            if (isDied)
                return;
            transform.localScale = transform.localScale + new Vector3(.5f, .5f, .5f);
            InventoryManager.Instance.AddResourcesToInventory(resource.GetResourceCosts());
            transform.DOScale(0, .1f).SetEase(Ease.InOutBounce).OnComplete(async () =>
            {
                await Play();
            });
        }
        private async Task<bool>Play()
        {
            destoryParticle.transform.position = transform.position;
            destoryParticle.Play();
            await Helper.WaitForSecond(Mathf.RoundToInt(destoryParticle.main.duration * 1000));
            gameObject.SetActive(false);
            return true;
        }
        public bool IsDied()
        {
            return base.isDied;
        }
    }
}
