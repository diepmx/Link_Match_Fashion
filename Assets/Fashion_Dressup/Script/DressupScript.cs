using GIKCore.Pool;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DressupScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_txtCoin;
    [SerializeField] private TextMeshProUGUI m_txtGem;
    [SerializeField] private TextMeshProUGUI m_txtEnergy;
    [SerializeField] private TextCountDownHaveTitle m_txtCountDownHaveTitle;
    [SerializeField] private HorizontalPoolGroup m_PoolItemSelect;
    [SerializeField] private playerGirl m_PlayerGirl;
    [SerializeField] private MoveAnimation m_PanelTop;
    [SerializeField] private MoveAnimation m_PanelButton;
    [SerializeField] private MoveAnimation m_PanelLevel;
    [SerializeField] private MoveAnimation m_ScrollItem;
    private bool InitFirst = false;
    private void Start()
    {
        StartCoroutine(DelayUpdate());
        SkeletonDataAsset Asset = IUtil.GetDataAsset(CenterDataManager.Instance.GameData.CurrentChapter);
        if (Asset != null)
        {
            m_PlayerGirl.Setdata(
                Asset,
                initialSkinName: string.IsNullOrEmpty(CenterDataManager.Instance.GameData.CurrentInitialName)
                    ? "skin_poor_lina"
                    : CenterDataManager.Instance.GameData.CurrentInitialName,
                AnimationName: string.IsNullOrEmpty(CenterDataManager.Instance.GameData.CurrentAnimationName)
                    ? "eye_blink"
                    : CenterDataManager.Instance.GameData.CurrentAnimationName
            );
        }
        ShowAllPanelMain();
        m_txtCountDownHaveTitle.SetCountDownToZeroUpdate(() =>
        {
            CenterDataManager.Instance.GameData.TimeRepeatEnergy = 300;
            CenterDataManager.Instance.GameData.Energy += CenterDataManager.Instance.GameData.Energy < 10 ? 1 : 0;
            m_txtCountDownHaveTitle.SetCountDown((long)CenterDataManager.Instance.GameData.TimeRepeatEnergy);

        }).SetOffsetUpdate((Offset) =>
        {
            CenterDataManager.Instance.GameData.TimeRepeatEnergy = (float)Offset;
        }).SetCountDown((long)CenterDataManager.Instance.GameData.TimeRepeatEnergy);

        m_PoolItemSelect.SetCellDataCallback<CharacterAnim>((go, data, index) =>
        {
            itemChooseEquip script = go.GetComponent<itemChooseEquip>();
            script.SetSelected(false);
            script.SetCallbackFunc((Anim) =>
            {
                m_PoolItemSelect.ReloadDataToVisibleCell();
                script.SetSelected(true);
                CenterDataManager.Instance.GameData.CurrentInitialName = Anim.InitialSkin;
                CenterDataManager.Instance.GameData.CurrentAnimationName = Anim.AnimationName;
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

    IEnumerator DelayUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_txtCoin.text = CenterDataManager.Instance.GameData.totalCoins.ToString();
            m_txtGem.text = CenterDataManager.Instance.GameData.TotalGem.ToString();
            m_txtEnergy.text = CenterDataManager.Instance.GameData.Energy.ToString();
        }
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
