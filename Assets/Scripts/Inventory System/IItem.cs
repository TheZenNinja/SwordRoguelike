using UnityEngine;

namespace InventorySystem
{
    public interface IItem
    {
        public string GetName();
        public Sprite GetSprite();
    }
}
