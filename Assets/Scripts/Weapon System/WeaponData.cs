using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

namespace WeaponSystem
{
    [System.Serializable]
    public class WeaponData
    {
        public PartItem core, blade, handle, aux;

        [Space]
        [Header("Calculated Stats")]
        public float damage;
        public ElementalType element;
        public float statusChance;

        public float attackSpeed;
        public float attackRange;
        public int piercing;

        public WeaponData(PartItem core, PartItem blade, PartItem handle, PartItem aux)
        {
            this.core = core;
            this.blade = blade;
            this.handle = handle;
            this.aux = aux;
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            float baseDmg = core.WeaponData.damageFlat;
            this.element = core.elementType;
            float baseStsChnc = core.WeaponData.statusChance;

            float baseAtkSpd = core.WeaponData.attackSpeedFlat;
            float baseAtkRan = core.WeaponData.attackRangeFlat;

            int basePierc = core.WeaponData.peircing;


            var statModifier = new WeaponPartData();
            if (blade != null)
                statModifier += blade.WeaponData;
            if (handle != null)
                statModifier += handle.WeaponData;
            if (aux != null)
                statModifier += aux.WeaponData;


            this.damage = GetTotalStat(baseDmg, statModifier.damageFlat, statModifier.damagePercent);
            this.statusChance = baseStsChnc + statModifier.statusChance;

            this.attackSpeed = GetTotalStat(baseAtkSpd, statModifier.attackSpeedFlat, statModifier.attackSpeedPercent);
            this.attackRange = GetTotalStat(baseAtkRan, statModifier.attackRangeFlat, statModifier.attackRangePercent);

            this.piercing = basePierc + statModifier.peircing;

            #region zombieCode
            /*List<WeaponStat> stats = new List<WeaponStat>();
            stats.AddRange(blade.weaponStats);
            stats.AddRange(handle.weaponStats);
            stats.AddRange(aux.weaponStats);

            float baseDmg = core.weaponStats.Find(x => x.statType == WeaponStat.StatType.damage).value;
            float baseStatus = core.weaponStats.Find(x => x.statType == WeaponStat.StatType.statusChance).value;
            float baseAtkSpeed = core.weaponStats.Find(x => x.statType == WeaponStat.StatType.attackSpeed).value;
            float baseAtkRange = core.weaponStats.Find(x => x.statType == WeaponStat.StatType.attackRange).value;
            int basePiercing = core.weaponStats.Find(x => x.statType == WeaponStat.StatType.peircing).value.RoundToInt();

            if (stats.Count == 0)
                Debug.LogError("This weapon has no stats");

            //store flat in the X and percent in the Y
            var damage = Vector2.zero;
            float statusChance = 0;
            var attackSpeed = Vector2.zero;
            var attackRange = Vector2.zero;
            int piercing = 0;
            foreach (var stat in stats)
            {
                switch (stat.statType)
                {
                    case WeaponStat.StatType.damage:
                        damage += stat;
                        break;
                    case WeaponStat.StatType.statusChance:
                        statusChance += stat.value;
                        break;
                    case WeaponStat.StatType.attackSpeed:
                        attackSpeed += stat;
                        break;
                    case WeaponStat.StatType.attackRange:
                        attackRange += stat;
                        break;
                    case WeaponStat.StatType.peircing:
                        piercing += stat.value.RoundToInt();
                        break;
                    default:
                        Debug.LogError($"Unhandled stat type {stat.statType}");
                        break;
                }
            }

            //add up the values
            float totalDamage = GetTotalStat(baseDmg, damage);
            float totalStatus = baseStatus + statusChance;
            float totalAtkSpeed = GetTotalStat(baseAtkSpeed, attackSpeed);
            float totalAtkRange = GetTotalStat(baseAtkRange, attackRange);
            int totalPiercing = basePiercing + piercing;

            //set the weapons values
            this.damage = totalDamage;
            this.element = blade.elementType;
            this.attackSpeed = totalAtkSpeed;
            this.attackRange = totalAtkRange;
            this.piercing = piercing;*/
            #endregion
        }
        private static float GetTotalStat(float baseStat, float modifyStatFlat, float modifyStatPerc) => baseStat * (1 + modifyStatPerc) + modifyStatFlat;

	}
}