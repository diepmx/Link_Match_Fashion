using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoOnOff : MonoBehaviour
{
    public enum ActionOnAwake { None, TurnOn, TurnOff };

    [SerializeField] private GameObject m_GoOn, m_GoOff;
    [SerializeField] private ActionOnAwake m_ActionOnAwake = ActionOnAwake.None;
    [SerializeField] private string m_Id;
    [Header("[Panel Zone]")]
    [Tooltip("Priority 1: editor event\nPriority 2: event registered in script via SetPanelOnTween() function - Only executes when panel # null\nPriority 3: panel.SetActive")]
    [SerializeField] private GameObject m_Panel;
    [SerializeField] private UnityEvent m_PanelOnTweenFocus, m_PanelOnTweenBlur;

    public bool online { get; private set; } = false;
    public object data = null;
    public string id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }
    public int idInt { get { return ParseInt(id, -1); } }

    public GameObject goOn { get { return m_GoOn; } }
    public GameObject goOff { get { return m_GoOff; } }
    public GameObject panel { get { return m_Panel; } }
    public ICallback.CallFunc3<GameObject, bool> panelOnTween = null;
    public GoOnOff SetPanel(GameObject go) { m_Panel = go; return this; }
    public GoOnOff SetPanelOnTween(ICallback.CallFunc3<GameObject, bool> func) { panelOnTween = func; return this; }
    public GoOnOff SetActive(bool active) { gameObject.SetActive(active); return this; }
    public GoOnOff TurnOn()
    {
        online = true;
        m_GoOn.SetActive(true);
        m_GoOff.SetActive(false);
        SetPanelFocus();
        return this;
    }

    public int ParseInt(string s, int defaultValue = 0)
    {
        int r = defaultValue;
        int.TryParse(s, out r);
        return r;
    }

    public GoOnOff TurnOff()
    {
        online = false;
        m_GoOn.SetActive(false);
        m_GoOff.SetActive(true);
        SetPanelBlur();
        return this;
    }
    public GoOnOff Turn(bool on)
    {
        if (on) TurnOn();
        else TurnOff();
        return this;
    }

    private void SetPanelFocus()
    {
        if (m_PanelOnTweenFocus != null && m_PanelOnTweenFocus.GetPersistentEventCount() > 0)
        {
            m_PanelOnTweenFocus.Invoke();
        }
        else if (m_Panel != null)
        {
        }
    }
    private void SetPanelBlur()
    {
        if (m_PanelOnTweenBlur != null && m_PanelOnTweenBlur.GetPersistentEventCount() > 0)
        {
            m_PanelOnTweenBlur.Invoke();
        }
        else if (m_Panel != null)
        {

        }
    }

    // System
    void Awake()
    {
        if (m_ActionOnAwake == ActionOnAwake.TurnOn) TurnOn();
        else if (m_ActionOnAwake == ActionOnAwake.TurnOff) TurnOff();
        else
        {
            online = (m_GoOn.activeSelf && !m_GoOff.activeSelf);
        }
    }
}
