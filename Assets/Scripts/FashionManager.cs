using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Spine.Unity
{
    public class FashionManager : MonoBehaviour
    {
        public static FashionManager Instance;

        [Header("Fashion Database")]
        public FashionDatabase fashionDatabase;

        [Header("Player Fashion Inventory")]
        public List<FashionItem> playerInventory = new List<FashionItem>();
        public List<FashionItem> equippedItems = new List<FashionItem>();

        [Header("Currency")]
        public int coins = 1000;
        public int gems = 50;

        [Header("Events")]
        public System.Action<FashionItem> OnItemEquipped;
        public System.Action<FashionItem> OnItemUnequipped;
        public System.Action<FashionItem> OnItemMerged;
        public System.Action<int> OnCoinsChanged;
        public System.Action<int> OnGemsChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeInventory();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadPlayerData();
        }

        void InitializeInventory()
        {
            // Khởi tạo với một số item cơ bản
            if (playerInventory.Count == 0)
            {
                AddStarterItems();
            }
        }

        void AddStarterItems()
        {
            // Thêm một số item starter cho người chơi
            if (fashionDatabase != null)
            {
                var starterItems = fashionDatabase.GetStarterItems();
                foreach (var item in starterItems)
                {
                    AddItemToInventory(item);
                }
            }
        }

        public void AddItemToInventory(FashionItem item)
        {
            if (item != null)
            {
                playerInventory.Add(item);
                Debug.Log($"Đã thêm {item.itemName} vào inventory");
            }
        }

        public void RemoveItemFromInventory(FashionItem item)
        {
            if (playerInventory.Contains(item))
            {
                playerInventory.Remove(item);
                Debug.Log($"Đã xóa {item.itemName} khỏi inventory");
            }
        }

        public bool EquipItem(FashionItem item)
        {
            if (!playerInventory.Contains(item))
            {
                Debug.Log("Item không có trong inventory!");
                return false;
            }

            // Unequip item cùng loại nếu có
            UnequipItemType(item.itemType);

            // Equip item mới
            equippedItems.Add(item);
            OnItemEquipped?.Invoke(item);

            Debug.Log($"Đã trang bị {item.itemName}");
            return true;
        }

        public void UnequipItem(FashionItem item)
        {
            if (equippedItems.Contains(item))
            {
                equippedItems.Remove(item);
                OnItemUnequipped?.Invoke(item);
                Debug.Log($"Đã tháo {item.itemName}");
            }
        }

        public void UnequipItemType(FashionItemType itemType)
        {
            var itemToUnequip = equippedItems.FirstOrDefault(x => x.itemType == itemType);
            if (itemToUnequip != null)
            {
                UnequipItem(itemToUnequip);
            }
        }

        public FashionItem GetEquippedItem(FashionItemType itemType)
        {
            return equippedItems.FirstOrDefault(x => x.itemType == itemType);
        }

        public bool TryMergeItems(FashionItem item1, FashionItem item2)
        {
            if (!item1.CanMergeWith(item2))
            {
                Debug.Log("Không thể merge hai item này!");
                return false;
            }

            if (item1.mergeResult == null)
            {
                Debug.Log("Item này không có kết quả merge!");
                return false;
            }

            // Xóa 2 item cũ
            RemoveItemFromInventory(item1);
            RemoveItemFromInventory(item2);

            // Thêm item mới
            var newItem = CreateNewItem(item1.mergeResult);
            AddItemToInventory(newItem);

            OnItemMerged?.Invoke(newItem);
            Debug.Log($"Đã merge thành công tạo ra {newItem.itemName}!");

            return true;
        }

        public FashionItem CreateNewItem(FashionItem template)
        {
            return new FashionItem
            {
                id = template.id,
                itemName = template.itemName,
                description = template.description,
                icon = template.icon,
                itemType = template.itemType,
                rarity = template.rarity,
                level = template.level,
                maxLevel = template.maxLevel,
                sellPrice = template.sellPrice,
                upgradePrice = template.upgradePrice,
                spineSkinName = template.spineSkinName,
                itemColor = template.itemColor,
                canMerge = template.canMerge,
                mergeCount = template.mergeCount,
                mergeResult = template.mergeResult,
                isUnlocked = template.isUnlocked,
                unlockLevel = template.unlockLevel,
                unlockCost = template.unlockCost
            };
        }

        public bool SellItem(FashionItem item)
        {
            if (!playerInventory.Contains(item))
            {
                Debug.Log("Item không có trong inventory!");
                return false;
            }

            // Unequip nếu đang được trang bị
            if (equippedItems.Contains(item))
            {
                UnequipItem(item);
            }

            // Xóa khỏi inventory và thêm tiền
            RemoveItemFromInventory(item);
            AddCoins(item.GetTotalValue());

            Debug.Log($"Đã bán {item.itemName} với giá {item.GetTotalValue()} coins");
            return true;
        }

        public bool UpgradeItem(FashionItem item)
        {
            if (!item.CanUpgrade())
            {
                Debug.Log("Item đã đạt level tối đa!");
                return false;
            }

            if (coins < item.upgradePrice)
            {
                Debug.Log("Không đủ coins để upgrade!");
                return false;
            }

            SpendCoins(item.upgradePrice);
            item.UpgradeLevel();

            Debug.Log($"Đã upgrade {item.itemName} lên level {item.level}");
            return true;
        }

        public void AddCoins(int amount)
        {
            coins += amount;
            OnCoinsChanged?.Invoke(coins);
        }

        public bool SpendCoins(int amount)
        {
            if (coins >= amount)
            {
                coins -= amount;
                OnCoinsChanged?.Invoke(coins);
                return true;
            }
            return false;
        }

        public void AddGems(int amount)
        {
            gems += amount;
            OnGemsChanged?.Invoke(gems);
        }

        public bool SpendGems(int amount)
        {
            if (gems >= amount)
            {
                gems -= amount;
                OnGemsChanged?.Invoke(gems);
                return true;
            }
            return false;
        }

        public List<FashionItem> GetItemsByType(FashionItemType itemType)
        {
            return playerInventory.Where(x => x.itemType == itemType).ToList();
        }

        public List<FashionItem> GetMergeableItems(FashionItem targetItem)
        {
            return playerInventory.Where(x => x != targetItem && targetItem.CanMergeWith(x)).ToList();
        }

        public void SavePlayerData()
        {
            // Lưu dữ liệu người chơi vào PlayerPrefs hoặc file
            PlayerPrefs.SetInt("PlayerCoins", coins);
            PlayerPrefs.SetInt("PlayerGems", gems);

            // Lưu inventory (có thể dùng JSON)
            string inventoryJson = JsonUtility.ToJson(new SerializableList<FashionItem>(playerInventory));
            PlayerPrefs.SetString("PlayerInventory", inventoryJson);

            string equippedJson = JsonUtility.ToJson(new SerializableList<FashionItem>(equippedItems));
            PlayerPrefs.SetString("PlayerEquipped", equippedJson);

            PlayerPrefs.Save();
        }

        public void LoadPlayerData()
        {
            coins = PlayerPrefs.GetInt("PlayerCoins", 1000);
            gems = PlayerPrefs.GetInt("PlayerGems", 50);

            OnCoinsChanged?.Invoke(coins);
            OnGemsChanged?.Invoke(gems);

            // Load inventory
            string inventoryJson = PlayerPrefs.GetString("PlayerInventory", "");
            if (!string.IsNullOrEmpty(inventoryJson))
            {
                try
                {
                    var loadedInventory = JsonUtility.FromJson<SerializableList<FashionItem>>(inventoryJson);
                    playerInventory = loadedInventory.items;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Lỗi khi load inventory: " + e.Message);
                }
            }

            // Load equipped items
            string equippedJson = PlayerPrefs.GetString("PlayerEquipped", "");
            if (!string.IsNullOrEmpty(equippedJson))
            {
                try
                {
                    var loadedEquipped = JsonUtility.FromJson<SerializableList<FashionItem>>(equippedJson);
                    equippedItems = loadedEquipped.items;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Lỗi khi load equipped items: " + e.Message);
                }
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SavePlayerData();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SavePlayerData();
            }
        }

        private void OnDestroy()
        {
            SavePlayerData();
        }
    }

    [System.Serializable]
    public class SerializableList<T>
    {
        public List<T> items;

        public SerializableList(List<T> list)
        {
            items = list;
        }
    }
}
