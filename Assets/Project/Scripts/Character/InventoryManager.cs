using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GDev
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public UnityEvent<Dictionary<ResourceType, int>> updateResourceUI;

        private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>()
        {
            {ResourceType.Wood,0 },
            {ResourceType.Iron,0 },
            {ResourceType.Gems,0 }
        };

        public void AddResourcesToInventory(List<ResourceCost> resourcesCost)
        {
            Dictionary<ResourceType, int> resourceToDictionary = resourcesCost.ToDictionary(x => x.ResourceType, x => x.amount);
            if (resourceToDictionary.TryGetValue(ResourceType.Wood, out int woodValue))
                resources[ResourceType.Wood] += woodValue;
            if (resourceToDictionary.TryGetValue(ResourceType.Iron, out int ironValue))
                resources[ResourceType.Iron] += ironValue;
            if (resourceToDictionary.TryGetValue(ResourceType.Gems, out int gemsValue))
                resources[ResourceType.Gems] += gemsValue;
            updateResourceUI?.Invoke(resources);
        }
        public void TakeResource(List<ResourceCost> resourcesCost)
        {
            Dictionary<ResourceType, int> resourceToDictionary = resourcesCost.ToDictionary(x => x.ResourceType, x => x.amount);

            if (resourceToDictionary.TryGetValue(ResourceType.Wood, out int woodValue))
                resources[ResourceType.Wood] -= woodValue;
            if (resourceToDictionary.TryGetValue(ResourceType.Iron, out int ironValue))
                resources[ResourceType.Iron] -= ironValue;
            if (resourceToDictionary.TryGetValue(ResourceType.Gems, out int gemsValue))
                resources[ResourceType.Gems] -= gemsValue;
            updateResourceUI?.Invoke(resources);
        }
        public bool HaveResource(List<ResourceCost> resourcesCost)
        {
            Dictionary<ResourceType, int> resourceToDictionary = resourcesCost.ToDictionary(x => x.ResourceType, x => x.amount);
            bool haveResource = resourceToDictionary[ResourceType.Wood] <= resources[ResourceType.Wood] &&
                                resourceToDictionary[ResourceType.Iron] <= resources[ResourceType.Iron] &&
                                resourceToDictionary[ResourceType.Gems] <= resources[ResourceType.Gems];
            return haveResource;
        }
    }
}
