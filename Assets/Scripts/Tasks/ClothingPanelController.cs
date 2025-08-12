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

        private void OnEnable()
        {
            if (task == null) return;
            SetupOptions();
        }

        private void SetupOptions()
        {
            var opts = task.options;
            option1.Setup(opts.Count > 0 ? opts[0] : null, task.premiumIndex == 0);
            option2.Setup(opts.Count > 1 ? opts[1] : null, task.premiumIndex == 1);
            option3.Setup(opts.Count > 2 ? opts[2] : null, task.premiumIndex == 2);
            option1.onSelected = OnSelected;
            option2.onSelected = OnSelected;
            option3.onSelected = OnSelected;
        }

        private void OnSelected(FashionItemSO item, bool isPremium)
        {
            if (item == null) return;
            if (isPremium)
            {
                // require gems
                if (InitScript.Gems < item.gemsPrice)
                {
                    // TODO: show gems shop
                    return;
                }
                InitScript.Instance.SpendGems(item.gemsPrice);
            }

            // purchase and equip
            InventoryManager.Instance.Purchase(item);
            InventoryManager.Instance.Equip(item);

            if (panelRoot != null) panelRoot.SetActive(false);
        }
    }
}


