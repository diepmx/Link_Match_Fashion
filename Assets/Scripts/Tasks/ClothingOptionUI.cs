using System;
using UnityEngine;
using UnityEngine.UI;
namespace Spine.Unity
{
    public class ClothingOptionUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject premiumBadge;
        [SerializeField] private Text priceText; // hiển thị giá
        [SerializeField] private GameObject ownedTag; // hiển thị đã sở hữu

        private FashionItemSO item;
        private bool isPremium;

        public Action<FashionItemSO, bool> onSelected;

        public void Setup(FashionItemSO newItem, bool premium)
        {
            item = newItem;
            isPremium = premium;
            if (icon != null) icon.sprite = item != null ? item.icon : null;

            bool hasItem = item != null && InventoryManager.Instance != null && InventoryManager.Instance.HasItem(item.id);

            if (premiumBadge != null) premiumBadge.SetActive(isPremium && !hasItem);
            if (ownedTag != null) ownedTag.SetActive(hasItem);

            if (priceText != null)
            {
                if (item == null || hasItem)
                {
                    priceText.gameObject.SetActive(false);
                }
                else
                {
                    bool showGems = item.gemsPrice > 0;
                    bool showCoins = !showGems && item.coinsPrice > 0;
                    priceText.gameObject.SetActive(showGems || showCoins);
                    priceText.text = showGems ? item.gemsPrice.ToString() : (showCoins ? item.coinsPrice.ToString() : string.Empty);
                }
            }
            if (selectButton != null)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.interactable = item != null;
                selectButton.onClick.AddListener(() => onSelected?.Invoke(item, isPremium));
            }
        }
    }
}


