using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuySteps : MonoBehaviour
{
    [SerializeField] private Image m_ImageIcon;
    [SerializeField] private TextMeshProUGUI m_txtNameSteps;
    [SerializeField] private TextMeshProUGUI m_TxtPrice;

    public void SetData(ChapterGame step)
    {
        m_ImageIcon.sprite = step.icon;
        m_txtNameSteps.text = step.name;
        m_TxtPrice.text = step.CoinBuy + "";
        // còn thiếu chỗ show rewardBuff ở đây thì thêm sau
    }

    public void BuySteps()
    {

    }
}
