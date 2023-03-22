using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GDev
{
    public abstract class ButtonEventBase : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent Confirm;
        public UnityEvent Select;

        public abstract void Start();
        public abstract void OnDeselect(BaseEventData eventData);
        public virtual void OnSelect(BaseEventData eventData)
        {
            Select.Invoke();
        }
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Confirm.Invoke();
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnSubmit(eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            OnDeselect(eventData);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelect(eventData);
        }
    }
}
