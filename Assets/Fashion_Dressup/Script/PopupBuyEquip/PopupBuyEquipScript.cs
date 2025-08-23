using GIKCore.Pool;
using GIKCore.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PopupBuyEquipScript : MonoBehaviour
{
    [SerializeField] private ItemBuySteps m_ItemSteps;
    [SerializeField] private Image m_ImageBanner;
    [SerializeField] private TextMeshProUGUI m_txtTitle;
    private ICallback.CallFunc2<ChapterGame> CallbackFuncClose;
    private ChapterGame Chapter;
    private void Start()
    {
        int currentChapter = CenterDataManager.Instance.GameData.CurrentChapter - 1;
        int CurrentSteps = CenterDataManager.Instance.GameData.CurrentSteps - 1;
        Chapter = CenterDataManager.Instance.ChapterGameData.chapters[currentChapter].lstSteps[CurrentSteps];
        m_ItemSteps.SetCellCallbackData((data) =>
        {
            CallbackFuncClose?.Invoke(Chapter);
            Close();
        });
        m_ItemSteps.SetData(Chapter);
        m_txtTitle.text = CenterDataManager.Instance.ChapterGameData.chapters[currentChapter].Title;
        m_ImageBanner.sprite = CenterDataManager.Instance.ChapterGameData.chapters[currentChapter].Banner;
    }
   
    public void SetcellCloseCallback(ICallback.CallFunc2<ChapterGame> callback)
    {
        CallbackFuncClose = callback;
    }


    public void Close()
    {
        Destroy(gameObject);
    }

    public static void Show(ICallback.CallFunc2<ChapterGame> Callback)
    {
        PopupBuyEquipScript go = IUtil.LoadPrefabWithParent("", "PopupBuyEquip", IGame.main.canvas.panelPopup).GetComponent<PopupBuyEquipScript>();
        go.SetcellCloseCallback(Callback);
    }
}
