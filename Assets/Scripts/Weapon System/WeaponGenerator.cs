using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

namespace WeaponSystem
{
    public static class WeaponGenerator
    {
        [Header("Randomized Values")]
        static readonly Vector2 dmgMultiRng = new Vector2(-.5f, 1);
        static readonly Vector2 stsChnRng = new Vector2(-.25f, 0.75f);
        static readonly Vector2 piercingRng = new Vector2(0, 4);

        static readonly Vector2 atkSpdRng = new Vector2(-.5f, 2f);
        static readonly Vector2 atkRngRng = new Vector2(-.25f, 1f);

        [Header("Rarity Multis")]
        static readonly float commonMulti = 1;
        static readonly float uncommonMulti = 1.25f;
        static readonly float rareMulti = 1.5f;
        static readonly float legendaryMulti = 2;

        //static List<WeaponGeneratorRarityValues> rarities;


        public static WeaponInstance GenerateWeapon(WeaponType weaponType, ItemRarity rarity)
        {
            //var rarityVals = rarities.Find(x => x.rarity == rarity);
            float rarityMulti = 1;
            switch (rarity)
            {
                case ItemRarity.Common:
                default:
                    rarityMulti = commonMulti;
                    break;
                case ItemRarity.Uncommon:
                    rarityMulti = uncommonMulti;
                    break;
                case ItemRarity.Rare:
                    rarityMulti = rareMulti;
                    break;
                case ItemRarity.Legendary:
                    rarityMulti = legendaryMulti;
                    break;
            }


            var dmg = (GetRndInRange(dmgMultiRng) ).ClampToDecimalPlaces(2);
            var pierc = GetRndInRange(piercingRng).RoundToInt();

            var stsChn = GetRndInRange(stsChnRng).ClampToDecimalPlaces(2);
            var element = GetRandomElement();

            var atkSpd = GetRndInRange(atkSpdRng).ClampToDecimalPlaces(2);
            var atkRng = GetRndInRange(atkRngRng).ClampToDecimalPlaces(2);

            var wepInstance = new WeaponInstance(weaponType, rarity, element, dmg, stsChn,pierc, atkSpd, atkRng);

            return wepInstance;
        }
        static float GetRndInRange(Vector2 range) => Random.value * range.y + range.x;

        static ElementalType GetRandomElement() => (ElementalType)Random.Range(0, 4);
    }
}
