using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ItemBuySteps : MonoBehaviour
{
    [SerializeField] private Image m_ImageIcon;
    [SerializeField] private TextMeshProUGUI m_txtNameSteps;
    [SerializeField] private TextMeshProUGUI m_TxtPrice;
    private ChapterGame character;
    private ICallback.CallFunc2<ChapterGame> cellCallback;

    public void SetCellCallbackData(ICallback.CallFunc2<ChapterGame> Callback)
    {
        cellCallback = Callback;
    }

    public void SetData(ChapterGame step)
    {
        character = step;
        m_ImageIcon.sprite = step.icon;
        m_txtNameSteps.text = step.name;
        m_TxtPrice.text = step.CoinBuy + "";
        // còn thiếu chỗ show rewardBuff ở đây thì thêm sau
    }

    public void BuySteps()
    {
        //if (CenterDataManager.Instance.GameData.totalCoins < character.CoinBuy) return;
        cellCallback?.Invoke(character);
    }
}
