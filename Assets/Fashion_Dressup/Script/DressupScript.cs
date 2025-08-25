using GIKCore.Pool;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DressupScript : MonoBehaviour
{
    [SerializeField] private HorizontalPoolGroup m_PoolItemSelect;
    [SerializeField] private playerGirl m_PlayerGirl;
    [SerializeField] private MoveAnimation m_PanelTop;
    [SerializeField] private MoveAnimation m_PanelButton;
    [SerializeField] private MoveAnimation m_PanelLevel;
    [SerializeField] private MoveAnimation m_ScrollItem;
    private bool InitFirst = false;
    private void Start()
    {
        SkeletonDataAsset Asset = IUtil.GetDataAsset(CenterDataManager.Instance.GameData.CurrentChapter);
        if (Asset != null)
        {
            m_PlayerGirl.Setdata(
                Asset,
                initialSkinName: string.IsNullOrEmpty(CenterDataManager.Instance.GameData.AnimPlayer.PlayerInitialSkinName)
                    ? "skin_poor_lina"
                    : CenterDataManager.Instance.GameData.AnimPlayer.PlayerInitialSkinName,
                AnimationName: string.IsNullOrEmpty(CenterDataManager.Instance.GameData.AnimPlayer.AnimationName)
                    ? "eye_blink"
                    : CenterDataManager.Instance.GameData.AnimPlayer.AnimationName
            );
        }
        ShowAllPanelMain();
        m_PoolItemSelect.SetCellDataCallback<CharacterAnim>((go, data, index) =>
        {
            itemChooseEquip script = go.GetComponent<itemChooseEquip>();
            script.SetSelected(false);
            script.SetCallbackFunc((Anim) =>
            {
                m_PoolItemSelect.ReloadDataToVisibleCell();
                script.SetSelected(true);
                //CenterDataManager.Instance.GameData.CurrentInitialName = Anim.InitialSkin;
                //CenterDataManager.Instance.GameData.CurrentAnimationName = Anim.AnimationName;
                m_PlayerGirl.Setdata(Asset, initialSkinName: Anim.InitialSkin, AnimationName: Anim.AnimationName);
                CenterDataManager.Instance.SaveGameData();
            }).SetData(data);
            if(!InitFirst && index == 0)
            {
                script.SetSelected(true);
                InitFirst = true;
            }
        });
    }
    public void HideAllPanelMain()
    {
        m_PanelTop.Move();
        m_PanelButton.Move();
        m_PanelLevel.Move();
        m_ScrollItem.MoveBack();
    }

    public void ShowAllPanelMain()
    {
        m_PanelTop.MoveBack();
        m_PanelButton.MoveBack();
        m_PanelLevel.MoveBack();
        m_ScrollItem.Move();
    }


    public void OpenPopupTask()
    {
        PopupBuyEquipScript.Show((dataAdapter) =>
        {
            HideAllPanelMain();
            m_PoolItemSelect.ClearAdapter();
            m_PoolItemSelect.SetAdapter(dataAdapter.lstAmin);
        });
    }

    public void OpenPopupChapter()
    {
        PopupChapter.show();
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene("game");
    }
}
