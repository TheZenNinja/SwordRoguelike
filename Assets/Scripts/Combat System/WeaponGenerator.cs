using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public static class WeaponGenerator
    {
        [Header("Randomized Values")]
        static readonly Vector2 dmgMultiRng = new Vector2(.6f, 2);
        static readonly Vector2 stsChnRng = new Vector2(-.25f, 0.75f);

        static readonly Vector2 projSpeedRng = new Vector2(-4f, 8f);
        static readonly Vector2 returnDmgMultRng = new Vector2(-.25f, 2f);

        static readonly Vector2Int ammoBonusRng = new Vector2Int(-2, 4);


        public static WeaponInstance GenerateWeapon(WeaponData weaponData, float lootMulti = 1)
        {
            var dmg = (GetRndInRange(dmgMultiRng)).ClampToDecimalPlaces(2);

            var stsChn = GetRndInRange(stsChnRng).ClampToDecimalPlaces(2);
            var element = GetRandomElement();

            var projSpd = GetRndInRange(projSpeedRng).ClampToDecimalPlaces(2);
            var returnDmg = GetRndInRange(returnDmgMultRng).ClampToDecimalPlaces(2);

            var ammoBonus = Random.Range(ammoBonusRng.x, ammoBonusRng.y);

            var piercingFlag = GetFlagChance(0.1f, 0.2f);

            var wepInstance = new WeaponInstance(weaponData, element, dmg, stsChn, projSpd,returnDmg,ammoBonus, piercingFlag);

            return wepInstance;
        }
        static float GetRndInRange(Vector2 range) => Random.value * range.y + range.x;

        public static int GetFlagChance(float negativeChance, float positiveChance)
        { 
            // example with neg & pos chance being 0.1
            // if random val <0.1 it gets a false flag
            // if random val is > 0.9 (a 0.1 chance on the opposite end) it gets a true
            // otherwise its a neutral flag
            positiveChance = 1 - positiveChance;
            var val = Random.value;

            if (val <= negativeChance)
                return -1;
            if (val >= positiveChance)
                return 1;

            return 0;
        }

        static ElementalType GetRandomElement() => (ElementalType)Random.Range(0, 3);
    }
}
