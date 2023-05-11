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

        public float pickupRange = 1;

        [Header("Idle Anim Stuff")]
        public float hoverDistance = 2;
        public float angleOffset;
        public float angleOffsetChange;

        [Header("Inputs")]
        public InputActionReference attack;
        public InputActionReference returnWeapon;
        public InputActionReference pickupWeapon;

        void Start()
        {
            player = GetComponentInParent<Player>();
        }


        void Update()
        {
            if (attack.action.triggered)
                Shoot();
            if (returnWeapon.action.triggered)
                Recall();
            if (pickupWeapon.action.triggered)
                TryToPickupWeapon();

            IdleAnim();
        }



        void TryToPickupWeapon()
        {
            var objs = Physics2D.OverlapCircleAll(transform.position, pickupRange);

            foreach (var o in objs)
            {
                var wep = o.GetComponent<WeaponItem>();
                if (wep != null)
                {
                    LoadWeapons(wep.weapon);
                    Destroy(wep.gameObject);
                    break;
                }
            }
        }

        [ContextMenu("Generate Weapon")]
        void GenRandomWep() => LoadWeapons(WeaponGenerator.GenerateWeapon(weaponBase));
        [ContextMenu("Reload Weapon")]
        void ReloadWeapon() => LoadWeapons(weaponInstance);

        public void LoadWeapons(WeaponInstance wep)
        {
            //clear current weapons
            if (wepProjectiles.Count > 0)
                foreach (var w in wepProjectiles)
                    Destroy(w.gameObject);
            wepProjectiles.Clear();

            weaponInstance = wep;

            //instantiate weapons
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
            int numWeps = idleWeps.Count();

            if (numWeps == 0)
                return;
            
            angleOffset += angleOffsetChange * Time.deltaTime;
            if (angleOffset > 360)
                angleOffset -= 360;

            //get evenly divided points on a circle
            for (int i = 0; i < numWeps; i++)
            {
                var angle = (360f * i / numWeps + angleOffset) * Mathf.Deg2Rad;

                Vector2 goal = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * hoverDistance;
                idleWeps.ElementAt(i).MoveTowards(transform.TransformPoint(goal));
            }
        }

        void Shoot()
        {
            var idleWeps = wepProjectiles.Where(w => w.isIdle);
            if (idleWeps.Count() == 0)
                return;

            idleWeps.ElementAt(0).ShootProjectile(player.GetMousePositionWorld());
        }
        void Recall()
        {
            if (wepProjectiles.Exists(x => x.isShot))
                wepProjectiles.ForEach((x) =>
                {
                    if (x.isShot)
                        x.Recall();
                });
        }
    }
}