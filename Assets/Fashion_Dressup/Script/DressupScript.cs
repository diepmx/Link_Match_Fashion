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
    [SerializeField] private playerGirl m_PlayerGirl;
    private void Start()
    {
        StartCoroutine(DelayUpdate());

        SkeletonDataAsset Asset = IUtil.GetDataAsset(CenterDataManager.Instance.GameData.CurrentChapter);

        if (Asset != null)
        {
            m_PlayerGirl.Setdata(Asset, initialSkinName: "skin_1", AnimationName: "animation");
        }

        m_txtCountDownHaveTitle.SetCountDownToZeroUpdate(() =>
        {
            CenterDataManager.Instance.GameData.TimeRepeatEnergy = 300;
            CenterDataManager.Instance.GameData.Energy += CenterDataManager.Instance.GameData.Energy < 10 ? 1 : 0;
            m_txtCountDownHaveTitle.SetCountDown((long)CenterDataManager.Instance.GameData.TimeRepeatEnergy);

        }).SetOffsetUpdate((Offset) =>
        {
            CenterDataManager.Instance.GameData.TimeRepeatEnergy = (float)Offset;
        }).SetCountDown((long)CenterDataManager.Instance.GameData.TimeRepeatEnergy);
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


    public void OpenPopupTask()
    {
        PopupBuyEquipScript.Show();
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
