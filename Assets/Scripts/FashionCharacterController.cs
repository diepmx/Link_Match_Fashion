using UnityEngine;
using Spine.Unity;
namespace Spine.Unity
{
    public class FashionCharacterController : MonoBehaviour
    {
        [Header("Character References")]
        public MatchSkins matchSkins;
        public SkeletonAnimation skeletonAnimation;

        [Header("Animation Settings")]
        public string idleAnimation = "idle";
        public string walkAnimation = "walk";
        public string danceAnimation = "dance";
        public string poseAnimation = "pose";

        private void Start()
        {
            if (matchSkins == null)
                matchSkins = GetComponent<MatchSkins>();

            if (skeletonAnimation == null)
                skeletonAnimation = GetComponent<SkeletonAnimation>();

            // Subscribe to fashion manager events
            if (FashionManager.Instance != null)
            {
                FashionManager.Instance.OnItemEquipped += OnFashionItemEquipped;
                FashionManager.Instance.OnItemUnequipped += OnFashionItemUnequipped;
            }

            // Play default animation
            PlayAnimation(idleAnimation, true);
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (FashionManager.Instance != null)
            {
                FashionManager.Instance.OnItemEquipped -= OnFashionItemEquipped;
                FashionManager.Instance.OnItemUnequipped -= OnFashionItemUnequipped;
            }
        }

        void OnFashionItemEquipped(FashionItem item)
        {
            if (matchSkins != null)
            {
                matchSkins.EquipFashionItem(item);
            }

            // Play equip animation/effect
            PlayEquipEffect();
        }

        void OnFashionItemUnequipped(FashionItem item)
        {
            if (matchSkins != null)
            {
                matchSkins.UnequipFashionItemType(item.itemType);
            }
        }

        public void PlayAnimation(string animationName, bool loop = false)
        {
            if (skeletonAnimation != null && !string.IsNullOrEmpty(animationName))
            {
                skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
            }
        }

        public void PlayEquipEffect()
        {
            // Play sparkle/shine effect when equipping
            PlayAnimation(poseAnimation, false);

            // Return to idle after pose
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
            }

            // Add particle effect here if available
            // Có thể thêm particle system để tạo hiệu ứng lung linh
        }

        public void PlayFashionShowAnimation()
        {
            // Animation for fashion show/runway walk
            PlayAnimation(walkAnimation, false);

            // Chain with pose animation
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.AddAnimation(0, poseAnimation, false, 0);
                skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
            }
        }

        public void PlayDanceAnimation()
        {
            PlayAnimation(danceAnimation, true);
        }

        public void PlayIdleAnimation()
        {
            PlayAnimation(idleAnimation, true);
        }

        public void UpdateCharacterFromEquippedItems()
        {
            if (FashionManager.Instance == null || matchSkins == null) return;

            // Clear all equipped items first
            matchSkins.clothesSkin = "";
            matchSkins.pantsSkin = "";
            matchSkins.bagSkin = "";
            matchSkins.hatSkin = "";

            // Apply all currently equipped items
            foreach (var equippedItem in FashionManager.Instance.equippedItems)
            {
                matchSkins.EquipFashionItem(equippedItem);
            }
        }

        // Method để preview item mà không cần equip thật
        public void PreviewFashionItem(FashionItem item)
        {
            if (matchSkins == null || item == null) return;

            // Lưu trạng thái hiện tại
            string currentSkin = GetCurrentSkinForItemType(item.itemType);

            // Preview item mới
            matchSkins.EquipFashionItem(item);

            // Có thể tự động revert sau vài giây hoặc khi người dùng cancel
            Invoke(nameof(CancelPreview), 3f);
        }

        string GetCurrentSkinForItemType(FashionItemType itemType)
        {
            if (matchSkins == null) return "";

            switch (itemType)
            {
                case FashionItemType.Cloth:
                    return matchSkins.clothesSkin;
                case FashionItemType.Pants:
                    return matchSkins.pantsSkin;
                case FashionItemType.Bag:
                    return matchSkins.bagSkin;
                case FashionItemType.Hat:
                    return matchSkins.hatSkin;
                default:
                    return "";
            }
        }

        public void CancelPreview()
        {
            // Quay lại outfit đã được equip
            UpdateCharacterFromEquippedItems();
        }

        // Method để random outfit từ những item đã có
        public void RandomizeOutfit()
        {
            if (FashionManager.Instance == null) return;

            var inventory = FashionManager.Instance.playerInventory;
            if (inventory.Count == 0) return;

            // Unequip tất cả
            foreach (var equippedItem in FashionManager.Instance.equippedItems.ToArray())
            {
                FashionManager.Instance.UnequipItem(equippedItem);
            }

            // Random equip cho mỗi loại item
            foreach (FashionItemType itemType in System.Enum.GetValues(typeof(FashionItemType)))
            {
                var itemsOfType = FashionManager.Instance.GetItemsByType(itemType);
                if (itemsOfType.Count > 0)
                {
                    var randomItem = itemsOfType[Random.Range(0, itemsOfType.Count)];
                    FashionManager.Instance.EquipItem(randomItem);
                }
            }
        }

        // Method để save current outfit as preset
        public void SaveOutfitPreset(string presetName)
        {
            if (FashionManager.Instance == null) return;

            FashionOutfitPreset preset = new FashionOutfitPreset();
            preset.presetName = presetName;
            preset.equippedItems = new System.Collections.Generic.List<FashionItem>(FashionManager.Instance.equippedItems);

            // Save to PlayerPrefs hoặc file
            string presetJson = JsonUtility.ToJson(preset);
            PlayerPrefs.SetString($"OutfitPreset_{presetName}", presetJson);
            PlayerPrefs.Save();

            Debug.Log($"Đã lưu outfit preset: {presetName}");
        }

        // Method để load outfit preset
        public void LoadOutfitPreset(string presetName)
        {
            string presetJson = PlayerPrefs.GetString($"OutfitPreset_{presetName}", "");
            if (string.IsNullOrEmpty(presetJson)) return;

            try
            {
                FashionOutfitPreset preset = JsonUtility.FromJson<FashionOutfitPreset>(presetJson);

                // Unequip current items
                foreach (var equippedItem in FashionManager.Instance.equippedItems.ToArray())
                {
                    FashionManager.Instance.UnequipItem(equippedItem);
                }

                // Equip preset items
                foreach (var item in preset.equippedItems)
                {
                    FashionManager.Instance.EquipItem(item);
                }

                Debug.Log($"Đã load outfit preset: {presetName}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Lỗi khi load outfit preset {presetName}: {e.Message}");
            }
        }
    }

    [System.Serializable]
    public class FashionOutfitPreset
    {
        public string presetName;
        public System.Collections.Generic.List<FashionItem> equippedItems;
    }
}
