using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCountDownHaveTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TxtTime;

    private ActionRepeatTimer timer;
    private int _type = 0;
    public string title, titlebehind;
    private string color = "default";
    private long timeCountDown;
    private System.Action m_CountDownToZeroCb;
    private ICallback.CallFunc2<long> callbackoffset;
    private ICallback.CallFunc CallBackUpdate;
    public long CurrentTimeCountDown
    {
        get { return timeCountDown; }
    }


    public TextCountDownHaveTitle SetCountDownToZeroCallback(System.Action cb)
    {
        m_CountDownToZeroCb = cb;
        return this;
    }

    public TextCountDownHaveTitle SetCountDownToZeroUpdate(ICallback.CallFunc cb)
    {
        CallBackUpdate = cb;
        return this;
    }

    public TextCountDownHaveTitle SetOffsetUpdate(ICallback.CallFunc2<long> _callbackoffset)
    {
        callbackoffset = _callbackoffset;
        return this;
    }

    public TextCountDownHaveTitle SetCountDown(long _timeCountDown, string _title = "", string _titleback = "", string _titlecountto0 = "", string _color = "default")
    {
        timeCountDown = _timeCountDown;
        title = _title;
        titlebehind = _titleback;
        color = _color;
        if (timeCountDown > 0)
        {
            if (_color.Equals("default"))
            {
                m_TxtTime.text = _title + IUtil.FormatHourFromSec(_timeCountDown) + " " + _titleback;
            }
            else
            {
                m_TxtTime.text = _title + IUtil.StringColor(IUtil.FormatHourFromSec(_timeCountDown), color) + " " + _titleback;
            }
        }
        else
        {
            m_TxtTime.text = _titlecountto0;
        }
        return this;
    }

    public string FormatHourFromSecForBoss(long sec)
    {
        long hour = sec / 3600;
        long tmp = sec % 3600;
        long min = tmp / 60;
        long sec2 = tmp % 60;
        //
        long day = hour / 24;
        //
        if (day > 0)
        {
            hour = hour % 24;
            return day + "d " + FormatPrefixZero(value: hour) + "h " + FormatPrefixZero(min) + "m ";
        }
        else
        {
            return FormatPrefixZero(hour) + "h " + FormatPrefixZero(min) + "m ";
        }
    }

    public string FormatPrefixZero(long value)
    {
        if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value.ToString();
        }
    }

    private void Start()
    {
        timer = new ActionRepeatTimer(1f, () =>
        {
            callbackoffset?.Invoke(timeCountDown);
            if (timeCountDown > 0)
            {
                timeCountDown--;
                if (timeCountDown == 0)
                {
                    if (m_CountDownToZeroCb != null)
                    {
                        m_CountDownToZeroCb();
                    }
                }
                if (color.Equals("default"))
                {
                    m_TxtTime.text = title + ((_type == 0) ? IUtil.FormatHourFromSec(timeCountDown) : FormatHourFromSecForBoss(timeCountDown)) + " " + titlebehind;
                }
                else
                {
                    m_TxtTime.text = title + ((_type == 0) ? IUtil.StringColor(IUtil.FormatHourFromSec(timeCountDown), color) : FormatHourFromSecForBoss(timeCountDown)) + " " + titlebehind;
                }
            }
            else
            {
                CallBackUpdate?.Invoke();
            }
        });
    }

    private void Update()
    {
        timer?.UpdateTimer(Time.deltaTime);
    }
}
