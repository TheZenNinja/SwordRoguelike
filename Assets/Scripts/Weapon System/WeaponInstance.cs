using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WeaponSystem
{
    [System.Serializable]
    public class WeaponInstance
    {
        public WeaponType weaponType;

        [Header("Randomized Values")]
        public InventorySystem.ItemRarity rarity;
        public ElementalType element;

        [Space]

        [Header("Percent Multiplier Values")]
        public float dmgModifier;
        public float statusModifier;
        public int addPiercing;
        [Space]
        public float atkSpdMod;
        public float atkRngMod;

        public WeaponInstance(WeaponType weaponType, ItemRarity rarity, ElementalType element, float dmgModifier, float statusModifier, int addPiercing, float atkSpdMod, float atkRngMod)
        {
            this.weaponType = weaponType;
            this.rarity = rarity;
            this.element = element;
            this.dmgModifier = dmgModifier;
            this.statusModifier = statusModifier;
            this.addPiercing = addPiercing;
            this.atkSpdMod = atkSpdMod;
            this.atkRngMod = atkRngMod;
        }
    }
}
