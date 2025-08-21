using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class NetBlockTimeout
{
    public float max = 20f;//default
    public float count = 0f;
    public bool autoResetWhenHide = false;
}

public class NetBlock : MonoBehaviour
{
    [SerializeField] private GameObject m_GoBlockAction;

    private float TIMEOUT_DEFAULT = 20f;//seconds

    private NetBlockTimeout timeoutAction = new NetBlockTimeout();
    private Coroutine coroutineCircle;


    public NetBlock SetTimeout(float x, bool autoResetWhenHide = false)
    {
        SetTimeoutFor(timeoutAction, x, autoResetWhenHide);
        return this;
    }
    public NetBlock ResetTimeout() { ResetTimeoutFor(timeoutAction); return this; }
    public NetBlock HideBlock()
    {
        HideBlockFor(m_GoBlockAction, timeoutAction);
        return this;
    }
    public NetBlock ShowBlock(float delayShowCircle = 2f, bool stopCoroutineCircle = false)
    {
        if (stopCoroutineCircle)
            StopCoroutineCircle();

        gameObject.SetActive(true);
        if (delayShowCircle <= 0)
        {
            HideBlock();
            m_GoBlockAction.SetActive(true);
        }
        else
        {
            ShowBlockFor(m_GoBlockAction, timeoutAction, delayShowCircle);
        }

        return this;
    }
    public NetBlock ShowBlockWithCircle() { ShowBlock(0f); return this; }
    public NetBlock ShowBlockWithTimeAlive(float timeAlive = 0.2f, float delayShowCircle = 2f, bool stopCoroutineCircle = false)
    {
        SetTimeout(timeAlive, true);
        ShowBlock(delayShowCircle, stopCoroutineCircle);
        return this;
    }

    private void SetTimeoutFor(NetBlockTimeout timeout, float f, bool autoResetWhenHide)
    {
        timeout.max = f < 0 ? TIMEOUT_DEFAULT : f;
        timeout.autoResetWhenHide = autoResetWhenHide;
    }

    private void ResetTimeoutFor(NetBlockTimeout timeout) { timeout.max = TIMEOUT_DEFAULT; }

    private void UpdateTimeoutFor(NetBlockTimeout timeout, float deltaTime, ICallback.CallFunc calback)
    {
        if (timeout.max > 0)
        {
            timeout.count += deltaTime;
            if (timeout.count > timeout.max)
            {
                calback?.Invoke();
            }
        }
    }
    private void CheckHideAll()
    {
        if (!m_GoBlockAction.activeSelf)
        {
            StopCoroutineCircle();
            gameObject.SetActive(false);
        }
    }
    private void StopCoroutineCircle()
    {
        if (coroutineCircle != null)
            StopCoroutine(coroutineCircle);
    }


    private void HideBlockFor(GameObject targetBlock, NetBlockTimeout timeout)
    {
        targetBlock.SetActive(false);
        timeout.count = 0;
        if (timeout.autoResetWhenHide)
        {
            timeout.autoResetWhenHide = false;
            ResetTimeoutFor(timeout);
        }

        CheckHideAll();
    }

    private void ShowBlockFor(GameObject targetBlock, NetBlockTimeout timeout, float delay)
    {
        targetBlock.SetActive(true);

        timeout.count = 0;//reset time
       
    }
    void Update()
    {
        UpdateTimeoutFor(timeoutAction, Time.deltaTime, () => { HideBlock(); });
    }
}
