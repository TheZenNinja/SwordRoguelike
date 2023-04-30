using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WeaponSystem
{
    public class WeaponManager : MonoBehaviour
    {
        //[Tooltip("Order: sword, dagger, spear, claymore, hammer")]
        public List<NewMeleeController> weapons;

        [SerializeReference]
        public WeaponInstance primary;
        [SerializeReference]
        public WeaponInstance secondary;



        // Start is called before the first frame update
        void Start()
        {
            foreach (var w in weapons)
                w.gameObject.SetActive(true);
            UnequipAll();
            weapons.Find(x => x.weaponType.weaponType == primary.weaponType).Equip(primary);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                EquipWeapon(primary);
            if (secondary != null && Input.GetKeyDown(KeyCode.Alpha2))
                EquipWeapon(secondary);
        }
        public void UnequipAll()
        {
            foreach (var w in weapons)
                w.Unequip();
        }
        public void EquipWeapon(WeaponInstance weapon)
        {
            UnequipAll();
            weapons.Find(x => x.weaponType.weaponType == weapon.weaponType).Equip(weapon);
        }
        [Button("Random Weapon")]
        public void GenerateRndWeapon()
        {
            secondary = WeaponGenerator.GenerateWeapon((WeaponType)Random.Range(0, 4), (InventorySystem.ItemRarity)Random.Range(0, 3));
        }
    }
}