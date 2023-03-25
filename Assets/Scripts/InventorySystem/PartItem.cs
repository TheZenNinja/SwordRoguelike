using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "New Item")]
    public class PartItem : ScriptableObject
    {
        public enum PartType
        {
            Blade,
            Handle,
            Aux,
            Core,
        }
        public enum ItemRarity
        {
            Common, Uncommon, Rare, Legendary
        }

        [field: SerializeField] public string itemName { get; protected set; }
        [field: SerializeField] public Sprite itemSprite { get; protected set; }
        [field: SerializeField] public PartType type { get; protected set; }
        [field: SerializeField] public ItemRarity itemRarity { get; protected set; }
        [TextArea(3, 20)]
        [SerializeField]
        protected string itemDescription;
        public string ItemDescription => itemDescription;
    }
}