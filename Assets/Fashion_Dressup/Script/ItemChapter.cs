using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemChapter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_txtTitle;
    [SerializeField] private Image m_Banner;
    [SerializeField] private GameObject m_ButtonVisit;
    [SerializeField] private GameObject m_Finish;
    [SerializeField] private TextMeshProUGUI m_Process;
    [SerializeField] private Image m_ImageFill;
    [SerializeField] private GameObject m_ObjectProcess;
    private ICallback.CallFunc ICallBackvisit;

    public ItemChapter SetVisitCallBack(ICallback.CallFunc call)
    {
        ICallBackvisit = call;
        return this;
    }

    public void SetData(Chapter chapter)
    {
        m_txtTitle.text = chapter.Title;
        m_Banner.sprite = chapter.Banner;
        m_ButtonVisit.SetActive(chapter.id == CenterDataManager.Instance.GameData.CurrentChapter);
        m_Finish.SetActive(chapter.id < CenterDataManager.Instance.GameData.CurrentChapter);
        ChapterGameData chaptergame = CenterDataManager.Instance.ChapterGameData;
        float progress = (float)CenterDataManager.Instance.GameData.CurrentSteps /
            chaptergame.chapters[CenterDataManager.Instance.GameData.CurrentChapter].lstSteps.Count;
        m_Process.text = (progress * 100f).ToString("0") + "%";
        m_ObjectProcess.SetActive(chapter.id == CenterDataManager.Instance.GameData.CurrentChapter);
        m_ImageFill.fillAmount = progress;
    }

    public void ClickVisit()
    {
        ICallBackvisit?.Invoke();
    }
}
