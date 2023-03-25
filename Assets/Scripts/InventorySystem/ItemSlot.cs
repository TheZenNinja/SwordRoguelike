using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler//IPointerDownHandler, IPointerUpHandler
    {
        public enum ItemSlotType
        {
            any,
            blade,
            handle,
            aux,
            core,
        }

        [field: SerializeField]
        public PartItem Item { get; protected set; }
        [field: SerializeField]
        public ItemSlotType SlotType { get; protected set; }

        [field: SerializeField] public bool canClick { get; protected set; } = true;

        public bool hasItem => Item != null;

        [SerializeField] protected Image itemSprite;
        [SerializeField] protected Image rarityBorder;
        [SerializeField] protected Image animBorder;

        //public Action<ItemSlot> onClickStart;
        //public Action<ItemSlot> onClickEnd;
        public Action<ItemSlot> onClick;
        public Action<ItemSlot> onHover;
        public Action onHoverExit;
        public Action<ItemSlot> onChange;

        private void Start() => ReloadUI();
        private void OnValidate() => ReloadUI();

        public PartItem GetItem() => Item;
        public void SetItem(PartItem newItem)
        {
            Item = newItem;
            ReloadUI();
        }

        public void ReloadUI()
        {
            onChange?.Invoke(this);

            itemSprite.enabled = hasItem;
            rarityBorder.enabled = hasItem;

            if (!hasItem)
                return;

            itemSprite.sprite = Item.itemSprite;
            switch (Item.itemRarity)
            {
                default:
                case PartItem.ItemRarity.Common:
                    rarityBorder.color = Color.white;
                    break;
                case PartItem.ItemRarity.Uncommon:
                    rarityBorder.color = Color.green;
                    break;
                case PartItem.ItemRarity.Rare:
                    rarityBorder.color = Color.blue;
                    break;
                case PartItem.ItemRarity.Legendary:
                    rarityBorder.color = Color.magenta;
                    break;
            }
        }
        public bool CanAcceptItem(PartItem item)
        {
            if (item == null)
                return true;
            if (SlotType == ItemSlotType.any)
                return true;

            if (SlotType == ItemSlotType.blade &&
                item.type == PartItem.PartType.Blade)
                return true;

            if (SlotType == ItemSlotType.handle &&
                item.type == PartItem.PartType.Handle)
                return true;

            if (SlotType == ItemSlotType.aux &&
                item.type == PartItem.PartType.Aux)
                return true;

            if (SlotType == ItemSlotType.core &&
                item.type == PartItem.PartType.Core)
                return true;

            return false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverExit?.Invoke();
        }

        public void PlayAnimConfirm() => PlayAnim(1, Color.green, Color.white);
        public void PlayAnimHighlight() => PlayAnim(120, new Color(.3f, 1, 1), new Color(.3f, 1, 1));
        public void PlayAnimError() => PlayAnim(1, Color.red, Color.white);

        //didnt feel like setting up proper animations
        private void PlayAnim(float duration, Color startColor, Color endColor)
        {
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(duration, startColor, endColor));
        }
        private IEnumerator PlayAnimation(float duration, Color startColor, Color endColor)
        {
            float currentDur = duration;
            animBorder.enabled = true;
            Color targetColor = endColor * new Color(1, 1, 1, 0);

            while (currentDur > 0)
            {
                animBorder.color = Color.Lerp(startColor, targetColor, 1 - currentDur / duration);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                currentDur -= Time.fixedDeltaTime;
            }
            animBorder.enabled = false;
        }

        /*public void OnPointerDown(PointerEventData eventData)
        {
            onClickStart?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onClickEnd?.Invoke(this);
        }*/
    }
}