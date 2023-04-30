using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = "Data/Weapon Type Data")]
    public class WeaponTypeData : ScriptableObject
    {
        [ShowInInspector]
        public WeaponType weaponType;
        [ShowInInspector]
        public float damage;
        [PropertySpace]
        [ShowInInspector]
        [PropertyRange(0, 1)]
        public float statusChance;

        [PropertySpace]
        [ShowInInspector]
        public float attackSpeed;
        [ShowInInspector]
        public float attackRange;

        [PropertySpace]
        [ShowInInspector]
        public int piercing;

        [PropertySpace]
        [ShowInInspector]
        public MeleeProjectile projectile;

        [ShowInInspector]
        public float projectileLifetime;
        [ShowInInspector]
        public float projectileOffset;
        [ShowInInspector]
        public AnimationCurve projectileSpeedCurve;
        [Space]
        public RuntimeAnimatorController animController;

        public int GetDamage(WeaponInstance weaponInstance) => Mathf.RoundToInt(damage * (1 + weaponInstance.dmgModifier));
        public float GetStatus(WeaponInstance weaponInstance) => statusChance  + weaponInstance.statusModifier;
        
        public float GetAttackSpeed(WeaponInstance weaponInstance) => attackSpeed * (1 + weaponInstance.atkSpdMod);
        public float GetAttackRange(WeaponInstance weaponInstance) => attackRange * (1 + weaponInstance.atkRngMod);
        
        public float GetPiercing(WeaponInstance weaponInstance) => piercing + weaponInstance.addPiercing;
    }
}