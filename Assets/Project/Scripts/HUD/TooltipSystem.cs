using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDev
{
    public class TooltipSystem : Singleton<TooltipSystem>
    {
        private Tooltip tooltip;

        private void Start()
        {
            tooltip = GetComponentInChildren<Tooltip>();
            tooltip.gameObject.SetActive(false);
        }
        public static void Show(string content, string header)
        {
            Instance.tooltip.SetContent(content, header);
            Instance.tooltip.gameObject.SetActive(true);
        }
        public static void Hide()
        {
            Instance.tooltip.gameObject.SetActive(false);
        }
    }
}
