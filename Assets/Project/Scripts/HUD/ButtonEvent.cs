using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDev
{
    public class ButtonEvent : ButtonEventBase
    {
        private Vector3 originalPosition;
        public override void Start()
        {
            originalPosition = transform.position;
            StartCoroutine(WaitForFrames());
        }
        IEnumerator WaitForFrames()
        {
            yield return new WaitForSecondsRealtime(.5f);
            originalPosition = transform.position;
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            transform.DOScale(1f, .2f).SetEase(Ease.InOutSine).SetUpdate(true);
            transform.DOMove(originalPosition, .2f).SetEase(Ease.InOutSine).SetUpdate(true);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            transform.DOScale(1.2f, .2f).SetEase(Ease.InOutSine).SetUpdate(true);
            transform.DOMove(originalPosition + (Vector3.right * 30), .2f).SetEase(Ease.InOutSine).SetUpdate(true);
            base.OnSelect(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            transform.DOPunchPosition(Vector3.right, .2f, 10, 1).SetUpdate(true);
            base.OnSubmit(eventData);
        }
    }
}
