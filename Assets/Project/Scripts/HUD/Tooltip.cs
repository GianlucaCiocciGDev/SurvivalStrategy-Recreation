using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GDev
{
    public class Tooltip : MonoBehaviour
    {
        [Header("tooltip setting")]
        public TextMeshProUGUI headerTooltip;
        public TextMeshProUGUI bodyTooltip;
        public int characterWrapLimit;

        public RectTransform rectTransform;
        public LayoutElement layoutElement;

        private void Awake()
        {
            layoutElement = GetComponent<LayoutElement>();
            rectTransform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            Vector2 position = Mouse.current.position.ReadValue();
            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }
        public void SetContent(string content, string header)
        {

            headerTooltip.gameObject.SetActive(true);
            headerTooltip.text = header;

            bodyTooltip.text = content;

            int headerLenght = headerTooltip.text.Length;
            int contentLenght = bodyTooltip.text.Length;

            layoutElement.enabled = headerLenght > characterWrapLimit && contentLenght > characterWrapLimit;
        }
    }
}
