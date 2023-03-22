using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDev
{
    public class Building : MonoBehaviour
    {
        public bool isBuild;
        public int currentBuildingState = 0;
        public int totalBuildState = 6;
        public float range;
        [SerializeField] ParticleSystem buildParticle;
        public void Tick()
        {
            transform.DOComplete();
            transform.DOShakeScale(.5f, .3f, 10, 90).SetEase(Ease.Flash).OnComplete(() =>
            {
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + 0.15f, 0f, 0.8f), Mathf.Clamp(transform.localScale.y + 0.15f, 0f, 0.8f), Mathf.Clamp(transform.localScale.z + 0.15f, 0f, 0.8f)); 
                buildParticle.transform.position = transform.position;
                buildParticle.Play();
            });
            currentBuildingState++;
            isBuild = currentBuildingState >= totalBuildState ? true : false;
        }
        public bool IsBuild()
        {
            return isBuild;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
