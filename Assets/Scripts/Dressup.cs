using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Dressup : MonoBehaviour
{
    [SerializeField] private string bgChildName = "Bg1";
    [SerializeField] private bool selectFirstOnEnable = false;

    private readonly List<Button> buttons = new List<Button>();
    private readonly Dictionary<Button, GameObject> bgByButton = new Dictionary<Button, GameObject>();
    private readonly Dictionary<Button, UnityAction> handlers = new Dictionary<Button, UnityAction>();

    void Awake()
    {
        Refresh(); // gom danh sách button + map Bg1
    }

    void OnEnable()
    {
        Wire();
        if (selectFirstOnEnable && buttons.Count > 0)
            Highlight(buttons[0]);
        else
            HideAll();
    }

    void OnDisable()
    {
        Unwire();
    }

    /// <summary>Gọi hàm này nếu bạn thêm/bớt nút trong runtime.</summary>
    public void Refresh()
    {
        buttons.Clear();
        bgByButton.Clear();

        // Lấy tất cả Button con (kể cả đang inactive)
        buttons.AddRange(GetComponentsInChildren<Button>(includeInactive: true));

        foreach (var btn in buttons)
        {
            var t = btn.transform.Find(bgChildName);
            if (t != null)
            {
                bgByButton[btn] = t.gameObject;
            }
            else
            {
                // Không bắt buộc, nhưng log để biết nút nào thiếu Bg1
                // Debug.LogWarning($"[Bg1Highlighter] '{btn.name}' không có child '{bgChildName}'", btn);
            }
        }
    }

    private void Wire()
    {
        foreach (var btn in buttons)
        {
            if (handlers.ContainsKey(btn)) continue; // tránh add trùng
            var captured = btn; // tránh vấn đề capture biến vòng lặp
            UnityAction act = () => OnButtonClicked(captured);
            handlers[captured] = act;
            captured.onClick.AddListener(act);
        }
    }

    private void Unwire()
    {
        foreach (var kv in handlers)
            kv.Key.onClick.RemoveListener(kv.Value);
        handlers.Clear();
    }

    private void OnButtonClicked(Button clicked)
    {
        Highlight(clicked);
    }

    private void Highlight(Button clicked)
    {
        foreach (var kv in bgByButton)
            kv.Value.SetActive(kv.Key == clicked);
    }

    private void HideAll()
    {
        foreach (var go in bgByButton.Values)
            if (go != null) go.SetActive(false);
    }
}
