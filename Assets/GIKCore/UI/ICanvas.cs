using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent, ExecuteInEditMode]
public class ICanvas : MonoBehaviour
{
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] private RectTransform m_PanelUI, m_PanelPopup;
    [SerializeField] private bool m_ReregisterOnEnable = false;
    [SerializeField] private bool m_ManualRegister = false;

    public Canvas root { get { return m_Canvas; } }
    public Transform panelRoot { get { return m_Canvas.transform; } }
    public RectTransform panelUI { get { return m_PanelUI; } }
    public RectTransform panelPopup { get { return m_PanelPopup; } }
    public Rect rect { get { return panelUI.rect; } }

    public void Register()
    {
        IGame.main.canvas = this;
        if (m_Canvas == null) m_Canvas = GetComponent<Canvas>();
    }

    void Awake()
    {
        if (!m_ManualRegister)
            Register();
    }

}
