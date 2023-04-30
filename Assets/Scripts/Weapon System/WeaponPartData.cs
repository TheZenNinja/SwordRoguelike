using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WeaponSystem
{
    [System.Serializable]
    public struct WeaponPartData
    {
        [HorizontalGroup("Split", LabelWidth = 100)]
        [BoxGroup("Split/Flat")]
        [ShowInInspector] public float damageFlat { get; private set; }
        [HideIfGroup("@hidePercentValues", Value = true)]
        [BoxGroup("Split/Percent")]
        [ShowInInspector] public float damagePercent { get; private set; }

        [BoxGroup("Split/Flat")]
        [ShowInInspector] public float statusChance { get; private set; }

        [BoxGroup("Split/Flat")]
        [ShowInInspector] public float attackSpeedFlat { get; private set; }
        [BoxGroup("Split/Percent")]
        [ShowInInspector] public float attackSpeedPercent { get; private set; }
        
        [BoxGroup("Split/Flat")]
        [ShowInInspector] public float attackRangeFlat { get; private set; }
        [BoxGroup("Split/Percent")]
        [ShowInInspector] public float attackRangePercent { get; private set; }
        
        [BoxGroup("Split/Flat")]
        [ShowInInspector] public int peircing { get; private set; }

        //[HideInInspector]
        public bool hidePercentValues;

        public WeaponPartData(float damageFlat = 0, float damagePercent = 0, float statusChance = 0, float attackSpeedFlat = 0, float attackSpeedPercent = 0, float attackRangeFlat = 0, float attackRangePercent = 0, int peircing = 0)
        {
            this.damageFlat = damageFlat;
            this.damagePercent = damagePercent;
            this.statusChance = statusChance;
            this.attackSpeedFlat = attackSpeedFlat;
            this.attackSpeedPercent = attackSpeedPercent;
            this.attackRangeFlat = attackRangeFlat;
            this.attackRangePercent = attackRangePercent;
            this.peircing = peircing;
            hidePercentValues = false;
        }

        public void RemovePercentValues()
        {
            damagePercent = 0;
            attackRangePercent = 0;
            attackRangePercent = 0;
        }

        public static WeaponPartData operator +(WeaponPartData a, WeaponPartData b)
        {
            WeaponPartData d = new WeaponPartData()
            {
                damageFlat = a.damageFlat + b.damageFlat,
                damagePercent = a.damagePercent + b.damagePercent,
                statusChance = a.statusChance + b.statusChance,
                attackSpeedFlat = a.attackSpeedFlat + b.attackSpeedFlat,
                attackSpeedPercent = a.attackSpeedPercent + b.attackSpeedPercent,
                attackRangeFlat = a.attackRangeFlat + b.attackRangeFlat,
                attackRangePercent = a.attackRangePercent + b.attackRangePercent,
                peircing = a.peircing
            };
            return d;
        }
    }
}