using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Spine.Unity
{
    [CreateAssetMenu(fileName = "FashionDatabase", menuName = "Fashion System/Fashion Database")]
    public class FashionDatabase : ScriptableObject
    {
        [Header("Fashion Items Database")]
        public List<FashionItem> allFashionItems = new List<FashionItem>();

        [Header("Merge Chains")]
        public List<FashionMergeChain> mergeChains = new List<FashionMergeChain>();

        private void OnEnable()
        {
            if (allFashionItems.Count == 0)
            {
                CreateDefaultItems();
            }
        }

        void CreateDefaultItems()
        {
            // Tạo một số fashion items mặc định
            CreateClothItems();
            CreatePantsItems();
            CreateHatItems();
            CreateBagItems();
            CreateShoesItems();
            CreateAccessoryItems();

            Debug.Log($"Đã tạo {allFashionItems.Count} fashion items mặc định");
        }

        void CreateClothItems()
        {
            // Quần áo - Common
            AddFashionItem(1001, "Áo Phông Trắng", "Áo phông basic màu trắng", FashionItemType.Cloth, FashionRarity.Common, "clothes/basic-white");
            AddFashionItem(1002, "Áo Hoodie Xanh", "Hoodie thoải mái màu xanh", FashionItemType.Cloth, FashionRarity.Uncommon, "clothes/hoodie-blue");
            AddFashionItem(1003, "Áo Blazer Đen", "Blazer thanh lịch màu đen", FashionItemType.Cloth, FashionRarity.Rare, "clothes/blazer-black");
            AddFashionItem(1004, "Váy Dạ Hội", "Váy dạ hội lộng lẫy", FashionItemType.Cloth, FashionRarity.Epic, "clothes/evening-dress");
            AddFashionItem(1005, "Kimono Vàng", "Kimono truyền thống màu vàng", FashionItemType.Cloth, FashionRarity.Legendary, "clothes/kimono-gold");
        }

        void CreatePantsItems()
        {
            // Quần
            AddFashionItem(2001, "Quần Jean Xanh", "Quần jean cơ bản màu xanh", FashionItemType.Pants, FashionRarity.Common, "legs/jeans-blue");
            AddFashionItem(2002, "Quần Kaki Nâu", "Quần kaki thoải mái màu nâu", FashionItemType.Pants, FashionRarity.Uncommon, "legs/khaki-brown");
            AddFashionItem(2003, "Quần Suit Đen", "Quần suit lịch sự màu đen", FashionItemType.Pants, FashionRarity.Rare, "legs/suit-black");
            AddFashionItem(2004, "Váy Ngắn Hoa", "Váy ngắn họa tiết hoa", FashionItemType.Pants, FashionRarity.Epic, "legs/skirt-floral");
            AddFashionItem(2005, "Quần Lụa Vàng", "Quần lụa cao cấp màu vàng", FashionItemType.Pants, FashionRarity.Legendary, "legs/silk-gold");
        }

        void CreateHatItems()
        {
            // Mũ
            AddFashionItem(3001, "Mũ Lưỡi Trai", "Mũ lưỡi trai thể thao", FashionItemType.Hat, FashionRarity.Common, "accessories/cap-basic");
            AddFashionItem(3002, "Mũ Beanie", "Mũ len ấm áp", FashionItemType.Hat, FashionRarity.Uncommon, "accessories/beanie");
            AddFashionItem(3003, "Mũ Fedora", "Mũ fedora phong cách", FashionItemType.Hat, FashionRarity.Rare, "accessories/fedora");
            AddFashionItem(3004, "Vương Miện Nhỏ", "Vương miện tiểu thư", FashionItemType.Hat, FashionRarity.Epic, "accessories/crown-small");
            AddFashionItem(3005, "Vương Miện Hoàng Gia", "Vương miện hoàng gia lộng lẫy", FashionItemType.Hat, FashionRarity.Legendary, "accessories/crown-royal");
        }

        void CreateBagItems()
        {
            // Túi xách
            AddFashionItem(4001, "Túi Vải", "Túi vải đơn giản", FashionItemType.Bag, FashionRarity.Common, "accessories/bag-cloth");
            AddFashionItem(4002, "Túi Da", "Túi da thật chất lượng", FashionItemType.Bag, FashionRarity.Uncommon, "accessories/bag-leather");
            AddFashionItem(4003, "Túi Xách Tay", "Túi xách tay thanh lịch", FashionItemType.Bag, FashionRarity.Rare, "accessories/handbag");
            AddFashionItem(4004, "Túi Hàng Hiệu", "Túi hàng hiệu cao cấp", FashionItemType.Bag, FashionRarity.Epic, "accessories/designer-bag");
            AddFashionItem(4005, "Túi Kim Cương", "Túi được trang trí kim cương", FashionItemType.Bag, FashionRarity.Legendary, "accessories/diamond-bag");
        }

        void CreateShoesItems()
        {
            // Giày
            AddFashionItem(5001, "Giày Thể Thao", "Giày thể thao thoải mái", FashionItemType.Shoes, FashionRarity.Common, "shoes/sneakers");
            AddFashionItem(5002, "Giày Boot", "Boots da thời trang", FashionItemType.Shoes, FashionRarity.Uncommon, "shoes/boots");
            AddFashionItem(5003, "Giày Cao Gót", "Giày cao gót thanh lịch", FashionItemType.Shoes, FashionRarity.Rare, "shoes/heels");
            AddFashionItem(5004, "Giày Búp Bê", "Giày búp bê xinh xắn", FashionItemType.Shoes, FashionRarity.Epic, "shoes/ballet");
            AddFashionItem(5005, "Giày Vàng", "Giày được mạ vàng", FashionItemType.Shoes, FashionRarity.Legendary, "shoes/golden");
        }

        void CreateAccessoryItems()
        {
            // Phụ kiện
            AddFashionItem(6001, "Dây Chuyền Bạc", "Dây chuyền bạc đơn giản", FashionItemType.Accessory, FashionRarity.Common, "accessories/necklace-silver");
            AddFashionItem(6002, "Hoa Tai Ngọc Trai", "Hoa tai ngọc trai thanh lịch", FashionItemType.Accessory, FashionRarity.Uncommon, "accessories/earrings-pearl");
            AddFashionItem(6003, "Vòng Tay Vàng", "Vòng tay vàng cao cấp", FashionItemType.Accessory, FashionRarity.Rare, "accessories/bracelet-gold");
            AddFashionItem(6004, "Nhẫn Kim Cương", "Nhẫn kim cương lấp lánh", FashionItemType.Accessory, FashionRarity.Epic, "accessories/ring-diamond");
            AddFashionItem(6005, "Bộ Trang Sức Hoàng Gia", "Bộ trang sức hoàng gia đầy đủ", FashionItemType.Accessory, FashionRarity.Legendary, "accessories/royal-jewelry");
        }

        void AddFashionItem(int id, string name, string description, FashionItemType type, FashionRarity rarity, string spineSkinName)
        {
            var item = new FashionItem(id, name, type, rarity);
            item.description = description;
            item.spineSkinName = spineSkinName;
            item.itemColor = item.GetRarityColor(rarity);

            // Set up merge chains
            SetupMergeChain(item);

            allFashionItems.Add(item);
        }

        void SetupMergeChain(FashionItem item)
        {
            // Thiết lập chuỗi merge - item có thể merge với cùng loại để tạo ra item rare hơn
            switch (item.rarity)
            {
                case FashionRarity.Common:
                    // Common merge thành Uncommon
                    var uncommonItem = allFashionItems.FirstOrDefault(x =>
                        x.itemType == item.itemType && x.rarity == FashionRarity.Uncommon);
                    if (uncommonItem != null)
                    {
                        item.mergeResult = uncommonItem;
                    }
                    break;

                case FashionRarity.Uncommon:
                    // Uncommon merge thành Rare
                    var rareItem = allFashionItems.FirstOrDefault(x =>
                        x.itemType == item.itemType && x.rarity == FashionRarity.Rare);
                    if (rareItem != null)
                    {
                        item.mergeResult = rareItem;
                    }
                    break;

                case FashionRarity.Rare:
                    // Rare merge thành Epic
                    var epicItem = allFashionItems.FirstOrDefault(x =>
                        x.itemType == item.itemType && x.rarity == FashionRarity.Epic);
                    if (epicItem != null)
                    {
                        item.mergeResult = epicItem;
                    }
                    break;

                case FashionRarity.Epic:
                    // Epic merge thành Legendary
                    var legendaryItem = allFashionItems.FirstOrDefault(x =>
                        x.itemType == item.itemType && x.rarity == FashionRarity.Legendary);
                    if (legendaryItem != null)
                    {
                        item.mergeResult = legendaryItem;
                    }
                    break;

                case FashionRarity.Legendary:
                    // Legendary không thể merge nữa
                    item.canMerge = false;
                    break;
            }
        }

        public FashionItem GetItemById(int id)
        {
            return allFashionItems.FirstOrDefault(x => x.id == id);
        }

        public List<FashionItem> GetItemsByType(FashionItemType itemType)
        {
            return allFashionItems.Where(x => x.itemType == itemType).ToList();
        }

        public List<FashionItem> GetItemsByRarity(FashionRarity rarity)
        {
            return allFashionItems.Where(x => x.rarity == rarity).ToList();
        }

        public List<FashionItem> GetStarterItems()
        {
            // Trả về các item starter cho người chơi mới
            var starterItems = new List<FashionItem>();

            // Mỗi loại một item Common
            foreach (FashionItemType itemType in System.Enum.GetValues(typeof(FashionItemType)))
            {
                var commonItem = allFashionItems.FirstOrDefault(x =>
                    x.itemType == itemType && x.rarity == FashionRarity.Common);
                if (commonItem != null)
                {
                    var newItem = CreateItemCopy(commonItem);
                    newItem.isUnlocked = true;
                    starterItems.Add(newItem);
                }
            }

            return starterItems;
        }

        public FashionItem CreateItemCopy(FashionItem original)
        {
            return new FashionItem
            {
                id = original.id,
                itemName = original.itemName,
                description = original.description,
                icon = original.icon,
                itemType = original.itemType,
                rarity = original.rarity,
                level = original.level,
                maxLevel = original.maxLevel,
                sellPrice = original.sellPrice,
                upgradePrice = original.upgradePrice,
                spineSkinName = original.spineSkinName,
                itemColor = original.itemColor,
                canMerge = original.canMerge,
                mergeCount = original.mergeCount,
                mergeResult = original.mergeResult,
                isUnlocked = original.isUnlocked,
                unlockLevel = original.unlockLevel,
                unlockCost = original.unlockCost
            };
        }

        public List<FashionItem> GetRandomItems(int count, FashionRarity maxRarity = FashionRarity.Epic)
        {
            var availableItems = allFashionItems.Where(x => x.rarity <= maxRarity).ToList();
            var randomItems = new List<FashionItem>();

            for (int i = 0; i < count; i++)
            {
                if (availableItems.Count > 0)
                {
                    var randomItem = availableItems[Random.Range(0, availableItems.Count)];
                    randomItems.Add(CreateItemCopy(randomItem));
                }
            }

            return randomItems;
        }
    }

    [System.Serializable]
    public class FashionMergeChain
    {
        public string chainName;
        public FashionItemType itemType;
        public List<FashionItem> mergeSequence;
    }
}
