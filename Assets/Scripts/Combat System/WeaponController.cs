using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponData weaponBase;
        public WeaponInstance weaponInstance;
        Player player;
        public List<WeaponProjectile> wepProjectiles;

        [Header("Idle Anim Stuff")]
        public float hoverDistance = 2;
        public float angleOffset;
        public float angleOffsetChange;

        public InputActionReference attack;
        public InputActionReference returnWeapon;

        void Start()
        {
            player = GetComponentInParent<Player>();
            GenWep();
        }


        void Update()
        {
            if (attack.action.triggered)
                Launch();
            if (returnWeapon.action.triggered)
                Return();
            
            IdleAnim();
        }

        [ContextMenu("Generate Weapon")]
        void GenWep() => LoadWeapons(GenRandomWep());

        WeaponInstance GenRandomWep() => WeaponGenerator.GenerateWeapon(weaponBase);

        [ContextMenu("Reload Weapon")]
        void ReloadWeapon() => LoadWeapons(weaponInstance);

        void LoadWeapons(WeaponInstance wep)
        {
            weaponInstance = wep;

            if (wepProjectiles.Count > 0)
                wepProjectiles.ForEach(x => Destroy(x.gameObject));
            wepProjectiles.Clear();

            for (int i = 0; i < wep.Ammo; i++)
            {
                var w = Instantiate(wep.Prefab, transform.position, Quaternion.identity);
                w.Initialize(i + 1, transform, wep);
                wepProjectiles.Add(w);
            }
        }

        void IdleAnim()
        {
            var idleWeps = wepProjectiles.Where(w => w.isIdle);
            if (idleWeps.Count() == 0)
                return;
            angleOffset += angleOffsetChange * Time.deltaTime;
            if (angleOffset > 360)
                angleOffset -= 360;

            int numWeps = idleWeps.Count();

            for (int i = 0; i < numWeps; i++)
            {
                var angle = (360f * i / numWeps + angleOffset) * Mathf.Deg2Rad;

                Vector2 goal = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * hoverDistance;
                idleWeps.ElementAt(i).MoveTowards(transform.TransformPoint(goal));
            }
        }
        void Launch()
        {
            var idleWeps = wepProjectiles.Where(w => w.isIdle);
            if (idleWeps.Count() == 0)
                return;

            idleWeps.ElementAt(0).ShootProjectile(player.GetMousePositionWorld());
        }
        void Return()
        {
            if (wepProjectiles.Exists(x => x.isShot))
                wepProjectiles.ForEach((x) => {
                    if (x.isShot)
                        x.Recall(); });
        }
    }
}