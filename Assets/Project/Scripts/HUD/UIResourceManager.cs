using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GDev
{
    public class UIResourceManager : MonoBehaviour
    {
        [SerializeField] TMP_Text woodAmount;
        [SerializeField] TMP_Text ironAmount;
        [SerializeField] TMP_Text gemsAmount;

        public void UpdateUIResource(ResourceType type,int amount)
        {
            switch(type) 
            {
                case ResourceType.Wood: woodAmount.text = amount.ToString();break;
                case ResourceType.Iron: ironAmount.text = amount.ToString();break;
                case ResourceType.Gems: gemsAmount.text = amount.ToString();break;
            }    
        }
        public void UpdateResourceUI(Dictionary<ResourceType,int> resourceTypes)
        {
            woodAmount.text = resourceTypes[ResourceType.Wood].ToString();
            ironAmount.text = resourceTypes[ResourceType.Iron].ToString();
            gemsAmount.text = resourceTypes[ResourceType.Gems].ToString();
        }
    }
}
