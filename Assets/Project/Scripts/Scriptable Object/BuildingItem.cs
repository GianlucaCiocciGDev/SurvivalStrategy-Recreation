using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDev
{
    [CreateAssetMenu(fileName = "Build Prefab", menuName = "Items/Build Item")]
    public class BuildingItem : Item
    {
        public List<ResourceCost> resocureCost = new List<ResourceCost>();
        [Header("Item Model")]
        public GameObject Model;

    }

    [Serializable]
    public class ResourceCost
    {
        public ResourceType ResourceType;
        public int amount;
    }
}
