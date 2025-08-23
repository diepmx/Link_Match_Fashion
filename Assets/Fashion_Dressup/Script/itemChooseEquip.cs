using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class itemChooseEquip : MonoBehaviour
{
    [SerializeField] private Image m_ImageOn, m_ImageOff;
    [SerializeField] private GoOnOff m_OnOffSelect;
    private CharacterAnim _Character;
    private ICallback.CallFunc2<CharacterAnim> CallBackFunc;

    public itemChooseEquip SetCallbackFunc(ICallback.CallFunc2<CharacterAnim> callbackF)
    {
        CallBackFunc = callbackF;
        return this;
    }

    public itemChooseEquip SetData(CharacterAnim data)
    {
        _Character = data;
        m_ImageOn.sprite = m_ImageOff.sprite = data.IconImage;
        m_OnOffSelect.TurnOff();
        return this;
    }
    public itemChooseEquip SetSelected(bool isSelect)
    {
        m_OnOffSelect.Turn(isSelect);
        return this;
    }

    public void ClickItem()
    {
        CallBackFunc?.Invoke(_Character);
    }
}
