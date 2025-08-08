using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
namespace Spine.Unity
{
    public class FashionUI : MonoBehaviour
    {
        [Header("UI References")]
        public Transform inventoryParent;
        public Transform equippedItemsParent;
        public GameObject fashionItemUIPrefab;

        [Header("Currency UI")]
        public Text coinsText;
        public Text gemsText;

        [Header("Character Display")]
        public MatchSkins characterDisplay;

        [Header("Category Buttons")]
        public Button allButton;
        public Button clothButton;
        public Button pantsButton;
        public Button hatButton;
        public Button bagButton;
        public Button shoesButton;
        public Button accessoryButton;

        [Header("Action Buttons")]
        public Button mergeButton;
        public Button sellButton;
        public Button upgradeButton;
        public Button equipButton;

        [Header("Merge UI")]
        public GameObject mergePanel;
        public Transform mergeSlot1;
        public Transform mergeSlot2;
        public Transform mergeResultSlot;
        public Button confirmMergeButton;
        public Button cancelMergeButton;

        private FashionItemType currentCategory = (FashionItemType)(-1); // -1 = All
        private List<FashionItemUI> inventoryUIItems = new List<FashionItemUI>();
        private List<FashionItem> selectedItems = new List<FashionItem>();
        private FashionItem mergeItem1, mergeItem2;

        private void Start()
        {
            SetupUI();
            RefreshInventoryDisplay();
            RefreshEquippedDisplay();
            UpdateCurrencyDisplay();
        }

        private void OnEnable()
        {
            if (FashionManager.Instance != null)
            {
                FashionManager.Instance.OnItemEquipped += OnItemEquipped;
                FashionManager.Instance.OnItemUnequipped += OnItemUnequipped;
                FashionManager.Instance.OnItemMerged += OnItemMerged;
                FashionManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;
                FashionManager.Instance.OnGemsChanged += UpdateGemsDisplay;
            }
        }

        private void OnDisable()
        {
            if (FashionManager.Instance != null)
            {
                FashionManager.Instance.OnItemEquipped -= OnItemEquipped;
                FashionManager.Instance.OnItemUnequipped -= OnItemUnequipped;
                FashionManager.Instance.OnItemMerged -= OnItemMerged;
                FashionManager.Instance.OnCoinsChanged -= UpdateCoinsDisplay;
                FashionManager.Instance.OnGemsChanged -= UpdateGemsDisplay;
            }
        }

        void SetupUI()
        {
            // Setup category buttons
            if (allButton) allButton.onClick.AddListener(() => FilterByCategory((FashionItemType)(-1)));
            if (clothButton) clothButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Cloth));
            if (pantsButton) pantsButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Pants));
            if (hatButton) hatButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Hat));
            if (bagButton) bagButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Bag));
            if (shoesButton) shoesButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Shoes));
            if (accessoryButton) accessoryButton.onClick.AddListener(() => FilterByCategory(FashionItemType.Accessory));

            // Setup action buttons
            if (mergeButton) mergeButton.onClick.AddListener(OpenMergePanel);
            if (sellButton) sellButton.onClick.AddListener(SellSelectedItems);
            if (upgradeButton) upgradeButton.onClick.AddListener(UpgradeSelectedItem);
            if (equipButton) equipButton.onClick.AddListener(EquipSelectedItem);

            // Setup merge panel
            if (confirmMergeButton) confirmMergeButton.onClick.AddListener(ConfirmMerge);
            if (cancelMergeButton) cancelMergeButton.onClick.AddListener(CloseMergePanel);

            // Initially hide merge panel
            if (mergePanel) mergePanel.SetActive(false);

            UpdateActionButtons();
        }

        public void FilterByCategory(FashionItemType category)
        {
            currentCategory = category;
            RefreshInventoryDisplay();
        }

        public void RefreshInventoryDisplay()
        {
            // Clear existing UI items
            foreach (var uiItem in inventoryUIItems)
            {
                if (uiItem != null && uiItem.gameObject != null)
                    Destroy(uiItem.gameObject);
            }
            inventoryUIItems.Clear();

            if (FashionManager.Instance == null) return;

            // Get items to display
            List<FashionItem> itemsToShow;
            if ((int)currentCategory == -1) // All items
            {
                itemsToShow = FashionManager.Instance.playerInventory;
            }
            else
            {
                itemsToShow = FashionManager.Instance.GetItemsByType(currentCategory);
            }

            // Create UI for each item
            foreach (var item in itemsToShow)
            {
                CreateItemUI(item);
            }
        }

        void CreateItemUI(FashionItem item)
        {
            if (fashionItemUIPrefab == null || inventoryParent == null) return;

            GameObject uiObj = Instantiate(fashionItemUIPrefab, inventoryParent);
            FashionItemUI itemUI = uiObj.GetComponent<FashionItemUI>();

            if (itemUI != null)
            {
                itemUI.Setup(item);
                itemUI.OnItemSelected += OnItemSelected;
                itemUI.OnItemDeselected += OnItemDeselected;
                inventoryUIItems.Add(itemUI);
            }
        }

        public void RefreshEquippedDisplay()
        {
            if (FashionManager.Instance == null || equippedItemsParent == null) return;

            // Clear existing equipped UI
            foreach (Transform child in equippedItemsParent)
            {
                Destroy(child.gameObject);
            }

            // Create UI for equipped items
            foreach (var equippedItem in FashionManager.Instance.equippedItems)
            {
                GameObject uiObj = Instantiate(fashionItemUIPrefab, equippedItemsParent);
                FashionItemUI itemUI = uiObj.GetComponent<FashionItemUI>();

                if (itemUI != null)
                {
                    itemUI.Setup(equippedItem);
                    itemUI.SetEquippedState(true);
                }
            }
        }

        void OnItemSelected(FashionItem item)
        {
            if (!selectedItems.Contains(item))
            {
                selectedItems.Add(item);
                UpdateActionButtons();
            }
        }

        void OnItemDeselected(FashionItem item)
        {
            if (selectedItems.Contains(item))
            {
                selectedItems.Remove(item);
                UpdateActionButtons();
            }
        }

        void UpdateActionButtons()
        {
            bool hasSelection = selectedItems.Count > 0;
            bool hasTwoItems = selectedItems.Count == 2;
            bool canMerge = hasTwoItems && selectedItems[0].CanMergeWith(selectedItems[1]);

            if (mergeButton) mergeButton.interactable = canMerge;
            if (sellButton) sellButton.interactable = hasSelection;
            if (upgradeButton) upgradeButton.interactable = selectedItems.Count == 1 && selectedItems[0].CanUpgrade();
            if (equipButton) equipButton.interactable = selectedItems.Count == 1;
        }

        public void OpenMergePanel()
        {
            if (selectedItems.Count != 2) return;

            mergeItem1 = selectedItems[0];
            mergeItem2 = selectedItems[1];

            if (mergePanel) mergePanel.SetActive(true);

            // Display merge items
            DisplayMergeItem(mergeItem1, mergeSlot1);
            DisplayMergeItem(mergeItem2, mergeSlot2);

            if (mergeItem1.mergeResult != null)
            {
                DisplayMergeItem(mergeItem1.mergeResult, mergeResultSlot);
            }
        }

        void DisplayMergeItem(FashionItem item, Transform slot)
        {
            if (slot == null || fashionItemUIPrefab == null) return;

            // Clear existing item in slot
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }

            // Create new item UI
            GameObject uiObj = Instantiate(fashionItemUIPrefab, slot);
            FashionItemUI itemUI = uiObj.GetComponent<FashionItemUI>();

            if (itemUI != null)
            {
                itemUI.Setup(item);
                itemUI.SetInteractable(false);
            }
        }

        public void ConfirmMerge()
        {
            if (FashionManager.Instance != null && mergeItem1 != null && mergeItem2 != null)
            {
                bool success = FashionManager.Instance.TryMergeItems(mergeItem1, mergeItem2);
                if (success)
                {
                    RefreshInventoryDisplay();
                    ClearSelection();
                }
            }

            CloseMergePanel();
        }

        public void CloseMergePanel()
        {
            if (mergePanel) mergePanel.SetActive(false);
            mergeItem1 = null;
            mergeItem2 = null;
        }

        public void SellSelectedItems()
        {
            if (FashionManager.Instance == null) return;

            foreach (var item in selectedItems.ToList())
            {
                FashionManager.Instance.SellItem(item);
            }

            RefreshInventoryDisplay();
            RefreshEquippedDisplay();
            ClearSelection();
        }

        public void UpgradeSelectedItem()
        {
            if (FashionManager.Instance == null || selectedItems.Count != 1) return;

            FashionManager.Instance.UpgradeItem(selectedItems[0]);
            RefreshInventoryDisplay();
        }

        public void EquipSelectedItem()
        {
            if (FashionManager.Instance == null || selectedItems.Count != 1) return;

            FashionManager.Instance.EquipItem(selectedItems[0]);
            RefreshEquippedDisplay();
            ClearSelection();
        }

        void ClearSelection()
        {
            foreach (var itemUI in inventoryUIItems)
            {
                if (itemUI != null)
                    itemUI.SetSelected(false);
            }
            selectedItems.Clear();
            UpdateActionButtons();
        }

        void OnItemEquipped(FashionItem item)
        {
            RefreshEquippedDisplay();
            UpdateCharacterDisplay();
        }

        void OnItemUnequipped(FashionItem item)
        {
            RefreshEquippedDisplay();
            UpdateCharacterDisplay();
        }

        void OnItemMerged(FashionItem newItem)
        {
            RefreshInventoryDisplay();
            ClearSelection();

            // Show merge success effect
            Debug.Log($"Merge thành công! Tạo ra {newItem.itemName}");
        }

        void UpdateCharacterDisplay()
        {
            if (characterDisplay == null || FashionManager.Instance == null) return;

            // Update character appearance based on equipped items
            foreach (var equippedItem in FashionManager.Instance.equippedItems)
            {
                switch (equippedItem.itemType)
                {
                    case FashionItemType.Cloth:
                        characterDisplay.clothesSkin = equippedItem.spineSkinName;
                        break;
                    case FashionItemType.Pants:
                        characterDisplay.pantsSkin = equippedItem.spineSkinName;
                        break;
                    case FashionItemType.Hat:
                        characterDisplay.hatSkin = equippedItem.spineSkinName;
                        break;
                    case FashionItemType.Bag:
                        characterDisplay.bagSkin = equippedItem.spineSkinName;
                        break;
                }
            }

            // Update the character skin
            if (characterDisplay.gameObject.activeInHierarchy)
            {
                // characterDisplay.UpdateCombinedSkin();
            }
        }

        public void UpdateCurrencyDisplay()
        {
            if (FashionManager.Instance != null)
            {
                UpdateCoinsDisplay(FashionManager.Instance.coins);
                UpdateGemsDisplay(FashionManager.Instance.gems);
            }
        }

        void UpdateCoinsDisplay(int coins)
        {
            if (coinsText) coinsText.text = coins.ToString();
        }

        void UpdateGemsDisplay(int gems)
        {
            if (gemsText) gemsText.text = gems.ToString();
        }
    }
}
