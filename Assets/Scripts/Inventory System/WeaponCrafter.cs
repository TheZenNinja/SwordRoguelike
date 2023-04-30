using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace InventorySystem
{
    public class WeaponCrafter : MonoBehaviour
    {
        /// <summary>
        /// rework weapon stat system to allow for a weapon to not have core parts
        /// have a weaponType scriptableObject for storing base stats (including unarmed)?
        /// </summary>
        public ItemSlot blade, handle, aux;

        [SerializeReference]
        public WeaponData weaponData;

        void Start()
        {
            blade.onChange += UpdateData;
            handle.onChange += UpdateData;
            aux.onChange += UpdateData;
        }

        public void UpdateData(ItemSlot changedSlot)
        {
            ReloadWeapon();
        }

        void ReloadWeapon()
        {

            //weaponData = new WeaponData(core.part, blade.part, handle.part, aux.part);
        }
    }
}