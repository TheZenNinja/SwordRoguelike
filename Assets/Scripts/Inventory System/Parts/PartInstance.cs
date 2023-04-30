using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
namespace InventorySystem
{
    public class PartInstance
    {
        [ShowInInspector] public PartItem PartItem { get; private set; }

        [HideIf("@hideElement")]
        [ShowInInspector] public ElementalType element { get; private set; }
        [ShowInInspector] public ItemRarity rarity { get; private set; }
        
        private bool hideElement => PartItem == null || PartItem.itemType != PartItem.PartType.Blade;
    }
}
