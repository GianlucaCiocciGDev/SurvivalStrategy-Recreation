using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDev
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string ItemName;
        public Sprite ItemIcon;
        [TextArea]
        public string Description;
    }
}
