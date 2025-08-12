using System;
using UnityEngine;
using UnityEngine.UI;

public class ClothingOptionUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject premiumBadge;
    [SerializeField] private Text priceText; // show gems price if premium

    private FashionItemSO item;
    private bool isPremium;

    public Action<FashionItemSO, bool> onSelected;

    public void Setup(FashionItemSO newItem, bool premium)
    {
        item = newItem;
        isPremium = premium;
        if (icon != null) icon.sprite = item != null ? item.icon : null;
        if (premiumBadge != null) premiumBadge.SetActive(isPremium);
        if (priceText != null)
        {
            priceText.gameObject.SetActive(isPremium);
            priceText.text = isPremium && item != null ? item.gemsPrice.ToString() : string.Empty;
        }
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => onSelected?.Invoke(item, isPremium));
        }
    }
}


