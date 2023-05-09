using System;
using UnityEngine;

namespace CombatSystem
{
    [Serializable]
    public class WeaponInstance
    {
        [SerializeField] WeaponData data;

        [SerializeField] float dmgMulti;
        public int Damage => Mathf.RoundToInt(data.dmg * dmgMulti);

        [SerializeField] ElementalType element;
        public ElementalType Element => element;

        [Range(-1, 1)]
        [SerializeField] float statusChanceDelta;
        public float StatusChance => Mathf.Clamp01(data.statusChance + statusChanceDelta);
        public bool TryToInflictStatus() => StatusChance >= UnityEngine.Random.Range(0f, 1f);

        [SerializeField] float speedDelta;
        public float Speed => Mathf.Clamp(data.speed + speedDelta, 2, 100);

        [Range(1, 5)]
        [SerializeField] float returnDmgMultiDelta;
        public float ReturnDmgMulti => data.returnDmgMulti + returnDmgMultiDelta;

        [SerializeField] int ammoDelta;
        public int Ammo => Mathf.Clamp(data.ammo + ammoDelta, 1, 20);

        [Range(-1, 1)]
        [SerializeField] int piercingFlag;
        public bool CanPierce()
        {
            if (piercingFlag == -1)
                return false;
            else if (piercingFlag == 1)
                return true;

            return data.piercing;
        }

        [Range(-1, 1)]
        [SerializeField] int bouncingFlag;
        public bool CanBounce()
        {
            if (bouncingFlag == -1)
                return false;
            else if (bouncingFlag == 1)
                return true;

            return data.bouncing;
        }

        public WeaponProjectile Prefab => data.prefab;

        public WeaponInstance(WeaponData weaponData, ElementalType element, float dmgDelta, float statusChanceDelta, float speedDelta, float returnDmgMultiDelta, int ammoDelta, int piercingFlag = 0, int bouncingFlag = 0)
        {
            this.data = weaponData;
            this.element = element;
            this.dmgMulti = dmgDelta;
            this.statusChanceDelta = statusChanceDelta;
            this.speedDelta = speedDelta;
            this.returnDmgMultiDelta = returnDmgMultiDelta;
            this.ammoDelta = ammoDelta;
            this.piercingFlag = piercingFlag;
            this.bouncingFlag = bouncingFlag;
        }

        public DamageContainer GetDamageContainer() =>
            new DamageContainer(Damage, element, WillInflictStatus());

        public DamageContainer GetRecallDamageContainer() =>
            new DamageContainer(Mathf.RoundToInt(Damage * ReturnDmgMulti), element, WillInflictStatus());

        private bool WillInflictStatus() => UnityEngine.Random.value >= StatusChance;
    }
}
