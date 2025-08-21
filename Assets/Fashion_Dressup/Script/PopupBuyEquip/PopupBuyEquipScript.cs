using GIKCore.Pool;
using UnityEngine;
using UnityEngine.UI;
public class PopupBuyEquipScript : MonoBehaviour
{
    [SerializeField] private ItemBuySteps m_ItemSteps;
    [SerializeField] private Image m_ImageBanner;
    private ChapterGame Chapter;
    private void Start()
    {
        int currentChapter = CenterDataManager.Instance.GameData.CurrentChapter;
        int CurrentSteps = CenterDataManager.Instance.GameData.CurrentSteps;
        Chapter = CenterDataManager.Instance.ChapterGameData.chapters[currentChapter].lstSteps[CurrentSteps];
        m_ItemSteps.SetData(Chapter);
        m_ImageBanner.sprite = CenterDataManager.Instance.ChapterGameData.chapters[currentChapter].Banner;
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public static void Show()
    {
        GameObject go = IUtil.LoadPrefabWithParent("", "PopupBuyEquip", IGame.main.canvas.panelPopup);
    }
}
