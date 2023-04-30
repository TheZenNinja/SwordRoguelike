using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace InventorySystem
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameTxt, categoryTxt, descTxt, statsTxt;
        [SerializeField] private GameObject UIContainer;

        [SerializeField] private ItemSlot slot;

        private void Start()=>ToggleWindow(false);

        void UpdateUI()
        {
            var i = slot.GetPart();
            if (i == null)
            {
                ToggleWindow(false);
                return;
            }

            var p = i.PartItem;
            nameTxt.text = p.itemName;
            categoryTxt.text = $"{i.rarity} {p.itemType}";
            descTxt.text = p.itemDescription;
            statsTxt.text = p.GetStatusText();
        }

        private void ToggleWindow(bool active) => UIContainer.SetActive(active);

        public void HoverOverSlot(ItemSlot slot)
        {
            this.slot = slot;
            ToggleWindow(true);
            //stripping the parameter
            slot.onChange += _s => UpdateUI();
            UpdateUI();
        }
        public void HoverExitSlot()
        {
            ToggleWindow(false);
            slot.onChange -= _s => UpdateUI();
            this.slot = null;
        }
    }
}