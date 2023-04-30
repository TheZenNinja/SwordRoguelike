using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

        [SerializeField] private ItemSlot prevSlot;

        [SerializeField] private ItemDescription itemDescription;


        // Start is called before the first frame update
        void Start()
        {
            foreach (var s in GetComponentsInChildren<ItemSlot>())
            {
                s.onClick += OnSlotClick;
                s.onHover += OnSlotHoverEnter;
                s.onHoverExit += OnSlotHoverExit;
            }
        }

        public void OnSlotClick(ItemSlot slot)
        {
            if (prevSlot == null)
            {
                if (!slot.hasItem)
                {
                    slot.PlayAnimError();
                    return;
                }
                prevSlot = slot;
                slot.PlayAnimHighlight();
            }
            else if (slot == prevSlot)
            {
                slot.PlayAnimError();
                prevSlot = null;
            }
            else
            {
                var prevSlotItem = prevSlot.GetPart();
                var currSlotItem = slot.GetPart();

                if (!prevSlot.CanAcceptPart(currSlotItem) || !slot.CanAcceptPart(prevSlotItem))
                {
                    prevSlot.PlayAnimError();
                    slot.PlayAnimError();

                    prevSlot = null;
                    return;
                }

                prevSlot.SetPart(currSlotItem);
                slot.SetPart(prevSlotItem);

                prevSlot.PlayAnimConfirm();
                slot.PlayAnimConfirm();

                prevSlot = null;
            }

        }
        public void OnSlotHoverEnter(ItemSlot slot)
        {
            itemDescription.HoverOverSlot(slot);
        }
        public void OnSlotHoverExit()
        {
            itemDescription.HoverExitSlot();
        }



        public PartInstance GetItem(int index)
        {
            if (index > slots.Count || index < 0)
                return null;
            return slots[index].GetPart();
        }
    }
}