using UnityEngine;
using UnityEngine.UI;
namespace Spine.Unity
{
    public class FashionItemUI : MonoBehaviour
    {
        [Header("UI Components")]
        public Image itemIcon;
        public Image backgroundImage;
        public Image rarityBorder;
        public Text itemNameText;
        public Text levelText;
        public Text priceText;
        public Button selectButton;
        public GameObject selectedIndicator;
        public GameObject equippedIndicator;

        [Header("Level Stars")]
        public Transform starsParent;
        public GameObject starPrefab;

        [Header("Rarity Colors")]
        public Color commonColor = Color.gray;
        public Color uncommonColor = Color.green;
        public Color rareColor = Color.blue;
        public Color epicColor = Color.magenta;
        public Color legendaryColor = Color.yellow;

        private FashionItem currentItem;
        private bool isSelected = false;
        private bool isEquipped = false;

        public System.Action<FashionItem> OnItemSelected;
        public System.Action<FashionItem> OnItemDeselected;

        private void Start()
        {
            if (selectButton)
            {
                selectButton.onClick.AddListener(ToggleSelection);
            }

            UpdateUI();
        }

        public void Setup(FashionItem item)
        {
            currentItem = item;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (currentItem == null) return;

            // Update icon
            if (itemIcon && currentItem.icon)
            {
                itemIcon.sprite = currentItem.icon;
            }

            // Update name
            if (itemNameText)
            {
                itemNameText.text = currentItem.itemName;
            }

            // Update level
            if (levelText)
            {
                levelText.text = $"Lv.{currentItem.level}";
            }

            // Update price
            if (priceText)
            {
                priceText.text = $"{currentItem.GetTotalValue()}";
            }

            // Update rarity colors
            Color rarityColor = GetRarityColor(currentItem.rarity);

            if (backgroundImage)
            {
                backgroundImage.color = rarityColor * 0.3f; // Lighter background
            }

            if (rarityBorder)
            {
                rarityBorder.color = rarityColor;
            }

            // Update level stars
            UpdateLevelStars();

            // Update selection and equipped states
            UpdateSelectionState();
            UpdateEquippedState();
        }

        Color GetRarityColor(FashionRarity rarity)
        {
            switch (rarity)
            {
                case FashionRarity.Common:
                    return commonColor;
                case FashionRarity.Uncommon:
                    return uncommonColor;
                case FashionRarity.Rare:
                    return rareColor;
                case FashionRarity.Epic:
                    return epicColor;
                case FashionRarity.Legendary:
                    return legendaryColor;
                default:
                    return Color.white;
            }
        }

        void UpdateLevelStars()
        {
            if (starsParent == null || starPrefab == null || currentItem == null) return;

            // Clear existing stars
            foreach (Transform child in starsParent)
            {
                Destroy(child.gameObject);
            }

            // Create stars based on level
            for (int i = 0; i < currentItem.maxLevel; i++)
            {
                GameObject star = Instantiate(starPrefab, starsParent);
                Image starImage = star.GetComponent<Image>();

                if (starImage)
                {
                    // Filled star if current level >= this star index
                    starImage.color = (i < currentItem.level) ? Color.yellow : Color.gray;
                }
            }
        }

        public void ToggleSelection()
        {
            if (!selectButton.interactable) return;

            SetSelected(!isSelected);
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateSelectionState();

            if (currentItem != null)
            {
                if (isSelected)
                    OnItemSelected?.Invoke(currentItem);
                else
                    OnItemDeselected?.Invoke(currentItem);
            }
        }

        void UpdateSelectionState()
        {
            if (selectedIndicator)
            {
                selectedIndicator.SetActive(isSelected);
            }

            // Update button appearance based on selection
            if (selectButton)
            {
                ColorBlock colors = selectButton.colors;
                colors.normalColor = isSelected ? Color.cyan : Color.white;
                selectButton.colors = colors;
            }
        }

        public void SetEquippedState(bool equipped)
        {
            isEquipped = equipped;
            UpdateEquippedState();
        }

        void UpdateEquippedState()
        {
            if (equippedIndicator)
            {
                equippedIndicator.SetActive(isEquipped);
            }
        }

        public void SetInteractable(bool interactable)
        {
            if (selectButton)
            {
                selectButton.interactable = interactable;
            }

            // Dim the item if not interactable
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = interactable ? 1f : 0.6f;
        }

        public FashionItem GetItem()
        {
            return currentItem;
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public bool IsEquipped()
        {
            return isEquipped;
        }

        // Animation methods for visual feedback
        public void PlaySelectAnimation()
        {
            // Simple scale animation
            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.1f)
                     .setOnComplete(() =>
                     {
                         LeanTween.scale(gameObject, Vector3.one, 0.1f);
                     });
        }

        public void PlayMergeAnimation()
        {
            // Merge effect animation
            LeanTween.rotateZ(gameObject, 360f, 0.5f);
            LeanTween.scale(gameObject, Vector3.zero, 0.5f)
                     .setOnComplete(() =>
                     {
                         Destroy(gameObject);
                     });
        }

        public void PlayEquipAnimation()
        {
            // Equip effect animation
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + 20f, 0.3f)
                     .setLoopPingPong(1);
        }
    }
}
