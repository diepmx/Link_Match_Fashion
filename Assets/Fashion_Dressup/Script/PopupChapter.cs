using GIKCore.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupChapter : MonoBehaviour
{
    [SerializeField] private VerticalPoolGroup m_Pool;


    private void Awake()
    {
        m_Pool.SetCellDataCallback<Chapter>((go, data, index) =>
        {
            ItemChapter script = go.GetComponent<ItemChapter>();
            script.SetVisitCallBack(() =>
            {
                ClosePopup();
            }).SetData(data);
        });
        m_Pool.SetAdapter(CenterDataManager.Instance.ChapterGameData.chapters);
    }

    public void ClosePopup()
    {
        Destroy(gameObject);
    }

    public static void show()
    {
        GameObject go = IUtil.LoadPrefabWithParent("", "PopupChapter", IGame.main.canvas.panelPopup);
    }
}
