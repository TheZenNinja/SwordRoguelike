using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;
using Sirenix.OdinInspector;
using System;

namespace InventorySystem
{
    public class PartItem : ScriptableObject
    {
        public enum PartType
        {
            Blade,
            Handle,
            Aux,
        }

        [HorizontalGroup("Split", LabelWidth = 100)]

        [BoxGroup("Split/Info")]
        [ShowInInspector] public string itemName { get; protected set; }
        [BoxGroup("Split/Info")]
        [ShowInInspector] public PartType itemType { get; protected set; }

        [BoxGroup("Split/Info")]
        [GUIColor(.5f,1,1)]
        [InlineProperty]
        [ShowInInspector] public WeaponTypeData WeaponTypeData { get; protected set; }

        [ShowIf("@itemType", Value = PartType.Blade)]
        [BoxGroup("Split/Info")]
        [GUIColor(.5f, 1,1)]
        [ShowInInspector]
        public ElementalType elementType { get; protected set; }

        [BoxGroup("Split/Info")]
        [MultiLineProperty(5)]
        [ShowInInspector] public string itemDescription { get; protected set; }

        [BoxGroup("Split/Sprite")]
        [PreviewField(150, ObjectFieldAlignment.Center)]
        [HideLabel]
        [ShowInInspector] public Sprite itemSprite { get; protected set; }

        [PropertySpace]
        [BoxGroup("Weapon Stats")]
        [InfoBox("'X' is flat increase, 'Y is a % increase'")]

        [BoxGroup("Weapon Stats")]
        [InlineProperty]
        [ShowInInspector] public WeaponPartData WeaponData { get; protected set; }

        //[ShowInInspector] public List<WeaponStat> weaponStats { get; protected set; }

        [PropertySpace]
        [BoxGroup("Ability")]
        [ShowIf("@isCorePart")]
        [GUIColor(.5f, 1,1)]
        [ShowInInspector]
        public GameObject customBehaviour { get; protected set; }


        public string GetStatusText()
        {
            string txt = "";

            if (itemType == PartType.Blade)
                txt += $"Element: {elementType}\n";

            /*if (damage.isUsed)
                txt += $"Damage: {damage}\n";
            if (statusChance != 0)
                txt += $"Status Chance: {statusChance}\n";
            if (attackSpeed.isUsed)
                txt += $"Damage: {attackSpeed}\n";
            if (attackRange.isUsed)
                txt += $"Damage: {attackRange}\n";
            if (peircing != 0)
                txt += $"Peircing: {peircing.PrintNumWithSign()}\n";*/

            return txt.Trim();
        }

        
        private void OnValidate()
        {
            //WeaponData.hidePercentValues = itemType == PartType.Core;
                //if (weaponStats.Exists(x => x.statType == WeaponStat.StatType.damage))
                  //  weaponStats.Add(new WeaponStat(WeaponStat.StatType.damage, true, 0));
                //create missing entries
        }

    }
}