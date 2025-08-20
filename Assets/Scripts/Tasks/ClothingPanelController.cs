using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity
{
    public class ClothingPanelController : MonoBehaviour
    {
        [SerializeField] private TaskDefinitionSO task;
        [SerializeField] private ClothingOptionUI option1;
        [SerializeField] private ClothingOptionUI option2;
        [SerializeField] private ClothingOptionUI option3;
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private bool guardUnlock = true; // prevent opening unless task unlocked via stars

        private void OnEnable()
        {
            if (guardUnlock && task != null && !string.IsNullOrEmpty(task.taskId))
            {
                int unlocked = PlayerPrefs.GetInt($"TaskUnlocked:{task.taskId}", 0);
                if (unlocked == 0)
                {
                    // Block accidental open if not unlocked (e.g. via inspector onClick)
                    if (panelRoot != null) panelRoot.SetActive(false);
                    gameObject.SetActive(false);
                    return;
                }
            }
            if (task == null) return;
            SetupOptions();
        }

        private void SetupOptions()
        {
            var opts = task.options;
            var item1 = opts.Count > 0 ? opts[0] : null;
            var item2 = opts.Count > 1 ? opts[1] : null;
            var item3 = opts.Count > 2 ? opts[2] : null;

            // Xác định premium dựa trên giá Gems của từng item (fallback về premiumIndex nếu chưa cấu hình giá)
            bool isPremium1 = item1 != null ? item1.gemsPrice > 0 : task.premiumIndex == 0;
            bool isPremium2 = item2 != null ? item2.gemsPrice > 0 : task.premiumIndex == 1;
            bool isPremium3 = item3 != null ? item3.gemsPrice > 0 : task.premiumIndex == 2;

            option1.Setup(item1, isPremium1);
            option2.Setup(item2, isPremium2);
            option3.Setup(item3, isPremium3);
            option1.onSelected = OnSelected;
            option2.onSelected = OnSelected;
            option3.onSelected = OnSelected;
        }

        private void OnSelected(FashionItemSO item, bool isPremium)
        {
            if (item == null) return;
            // Only handle Gems here (premium). Coins are not used; stars were already spent in TaskPanel.
            var inv = InventoryManager.Instance;
            if (inv == null)
            {
                inv = FindObjectOfType<InventoryManager>();
            }
            if (inv == null)
            {
                Debug.LogError("InventoryManager instance not found in scene.");
                return;
            }
            bool purchasedOrOwned = inv.HasItem(item.id) || inv.Purchase(item);
            if (!purchasedOrOwned)
            {
                // TODO: feedback UI không đủ tiền/gems
                return;
            }
            inv.Equip(item);
            
            // Award dressup XP for completing selection
            var levelSys = DressupLevelSystem.Instance ? DressupLevelSystem.Instance : FindObjectOfType<DressupLevelSystem>();
            if (levelSys != null)
            {
                levelSys.AddExperience(20);
            }

            // if (panelRoot != null) panelRoot.SetActive(false);
        }
    }
}


