using System.Collections;
using UnityEngine;

namespace InventorySystem
{
    public class WeaponCrafter : MonoBehaviour
    {
        public ItemSlot core, blade, handle, aux;

        void Start()
        {
            core.onChange += UpdateData;
            blade.onChange += UpdateData;
            handle.onChange += UpdateData;
            aux.onChange += UpdateData;
        }

        public void UpdateData(ItemSlot changedSlot)
        { 
        
        }
    }
}