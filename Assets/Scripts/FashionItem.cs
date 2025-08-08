using UnityEngine;
using System.Collections.Generic;
namespace Spine.Unity
{
    [System.Serializable]
    public enum FashionItemType
    {
        Cloth = 0,
        Pants = 1,
        Bag = 2,
        Hat = 3,
        Shoes = 4,
        Accessory = 5
    }

    [System.Serializable]
    public enum FashionRarity
    {
        Common = 1,    // Màu xám
        Uncommon = 2,  // Màu xanh lá
        Rare = 3,      // Màu xanh dương
        Epic = 4,      // Màu tím
        Legendary = 5  // Màu vàng
    }

    [System.Serializable]
    public class FashionItem
    {
        [Header("Basic Info")]
        public int id;
        public string itemName;
        public string description;
        public Sprite icon;
        public FashionItemType itemType;
        public FashionRarity rarity;

        [Header("Game Properties")]
        public int level = 1;
        public int maxLevel = 5;
        public int sellPrice = 10;
        public int upgradePrice = 50;

        [Header("Visual")]
        public string spineSkinName;  // Tên skin trong Spine
        public Color itemColor = Color.white;

        [Header("Merge Properties")]
        public bool canMerge = true;
        public int mergeCount = 2;  // Số lượng cần để merge
        public FashionItem mergeResult;  // Item sẽ tạo ra khi merge

        [Header("Unlock Conditions")]
        public bool isUnlocked = false;
        public int unlockLevel = 1;  // Level cần đạt để unlock
        public int unlockCost = 0;   // Giá để unlock

        public FashionItem()
        {
            id = 0;
            itemName = "Default Item";
            description = "A basic fashion item";
            itemType = FashionItemType.Cloth;
            rarity = FashionRarity.Common;
            level = 1;
            maxLevel = 5;
            sellPrice = 10;
            upgradePrice = 50;
            itemColor = Color.white;
            canMerge = true;
            mergeCount = 2;
            isUnlocked = false;
            unlockLevel = 1;
            unlockCost = 0;
        }

        public FashionItem(int _id, string _name, FashionItemType _type, FashionRarity _rarity)
        {
            id = _id;
            itemName = _name;
            itemType = _type;
            rarity = _rarity;
            level = 1;
            maxLevel = 5;
            sellPrice = (int)_rarity * 10;
            upgradePrice = (int)_rarity * 25;
            itemColor = GetRarityColor(_rarity);
            canMerge = true;
            mergeCount = 2;
            isUnlocked = false;
            unlockLevel = 1;
            unlockCost = (int)_rarity * 100;
        }

        public Color GetRarityColor(FashionRarity rarity)
        {
            switch (rarity)
            {
                case FashionRarity.Common:
                    return new Color(0.7f, 0.7f, 0.7f); // Xám
                case FashionRarity.Uncommon:
                    return new Color(0.2f, 0.8f, 0.2f); // Xanh lá
                case FashionRarity.Rare:
                    return new Color(0.2f, 0.5f, 0.9f); // Xanh dương
                case FashionRarity.Epic:
                    return new Color(0.7f, 0.2f, 0.9f); // Tím
                case FashionRarity.Legendary:
                    return new Color(0.9f, 0.8f, 0.2f); // Vàng
                default:
                    return Color.white;
            }
        }

        public int GetTotalValue()
        {
            return sellPrice + (level - 1) * upgradePrice;
        }

        public bool CanUpgrade()
        {
            return level < maxLevel;
        }

        public void UpgradeLevel()
        {
            if (CanUpgrade())
            {
                level++;
                sellPrice = Mathf.RoundToInt(sellPrice * 1.2f);
            }
        }

        public bool CanMergeWith(FashionItem other)
        {
            if (!canMerge || !other.canMerge)
                return false;

            return id == other.id && level == other.level;
        }
    }
}
