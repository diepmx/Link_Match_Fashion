using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

namespace Spine
{
    public class MatchSkinsButton : MonoBehaviour
    {

        public SkeletonDataAsset skeletonDataAsset;
        public MatchSkins skinsSystem;

        [SpineSkin(dataField: "skeletonDataAsset")] public string itemSkin;
        public MatchSkins.ItemType itemType;

        [Header("Background Settings")]
        public Image backgroundImage; // Reference tới Image Bg1 của item

        private static MatchSkinsButton currentSelectedButton; // Để track item được chọn hiện tại

        void Start()
        {
            var button = GetComponent<Button>();

            // Đảm bảo background tắt khi bắt đầu
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(false);
            }

            button.onClick.AddListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            // Gọi function equip item gốc
            skinsSystem.Equip(itemSkin, itemType);

            // Xử lý background
            if (currentSelectedButton != null && currentSelectedButton != this)
            {
                // Tắt background của item trước đó
                if (currentSelectedButton.backgroundImage != null)
                {
                    currentSelectedButton.backgroundImage.gameObject.SetActive(false);
                }
            }

            // Bật background của item hiện tại
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(true);
            }

            // Cập nhật item được chọn hiện tại
            currentSelectedButton = this;
        }

        private void OnDestroy()
        {
            // Reset current selected button nếu đây là button đang được chọn
            if (currentSelectedButton == this)
            {
                currentSelectedButton = null;
            }
        }
    }
}