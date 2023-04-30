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
        }

        [SerializeField]
        protected PartInstance part;
        [field: SerializeField]
        public ItemSlotType slotType { get; protected set; }

        [field: SerializeField] public bool canClick { get; protected set; } = true;

        public bool hasItem => part != null;

        [Space]
        [Header("UI Stuff")]
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

        public PartInstance GetPart() => part;
        public void SetPart(PartInstance newItem)
        {
            part = newItem;
            ReloadUI();
        }

        public void ReloadUI()
        {
            onChange?.Invoke(this);

            itemSprite.enabled = hasItem;
            rarityBorder.enabled = hasItem;

            if (!hasItem)
                return;

            itemSprite.sprite = part.PartItem.itemSprite;
            switch (part.rarity)
            {
                default:
                case ItemRarity.Common:
                    rarityBorder.color = Color.white;
                    break;
                case ItemRarity.Uncommon:
                    rarityBorder.color = Color.green;
                    break;
                case ItemRarity.Rare:
                    rarityBorder.color = Color.blue;
                    break;
                case ItemRarity.Legendary:
                    rarityBorder.color = Color.magenta;
                    break;
            }
        }
        public bool CanAcceptPart(PartInstance p)
        {
            if (p == null)
                return true;
            if (slotType == ItemSlotType.any)
                return true;

            if (slotType == ItemSlotType.blade &&
                p.PartItem.itemType == PartItem.PartType.Blade)
                return true;

            if (slotType == ItemSlotType.handle &&
                p.PartItem.itemType == PartItem.PartType.Handle)
                return true;

            if (slotType == ItemSlotType.aux &&
                p.PartItem.itemType == PartItem.PartType.Aux)
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
        public void SetLock(bool isLocked) => canClick = !isLocked;
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