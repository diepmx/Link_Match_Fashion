using UnityEngine;
using TMPro;

[ExecuteAlways]
public class DialogueAutoSizer : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    [Header("Box")]
    public float boxWidth = 900f;
    public float padX = 40f;
    public float padY = 24f;
    public float minHeight = 96f;
    public bool collapseOnPlay = true;   // co nhỏ khi Play nếu chưa có chữ

    RectTransform bgRect, textRect;

    void Awake()
    {
        bgRect = GetComponent<RectTransform>();
        textRect = dialogueText ? dialogueText.rectTransform : null;

        // Anchor/pivot để nở xuống dưới
        if (bgRect) { bgRect.anchorMin = bgRect.anchorMax = new Vector2(0.5f, 1f); bgRect.pivot = new Vector2(0.5f, 1f); }
        if (dialogueText)
        {
            dialogueText.enableWordWrapping = true;
            dialogueText.overflowMode = TextOverflowModes.Overflow;
            var tr = dialogueText.rectTransform;
            tr.anchorMin = tr.anchorMax = new Vector2(0f, 1f);
            tr.pivot = new Vector2(0f, 1f);
        }
    }

    void OnEnable()
    {
        if (!Application.isPlaying) ResizeNow(dialogueText ? dialogueText.text : "");
        else if (collapseOnPlay) ResizeNow(""); // co nhỏ khi mới vào Play
    }

    /// GỌI HÀM NÀY mỗi khi đổi chữ hoặc tăng maxVisibleCharacters
    public void ResizeForVisible()
    {
        if (!dialogueText) return;
        int vis = Mathf.Clamp(dialogueText.maxVisibleCharacters, 0, dialogueText.text.Length);
        string visible = (vis == 0) ? "" : dialogueText.text.Substring(0, vis);
        ResizeNow(visible);
    }

    void ResizeNow(string content)
    {
        if (!bgRect || !dialogueText || !textRect) return;

        float contentW = Mathf.Max(1f, boxWidth - padX * 2f);

        dialogueText.ForceMeshUpdate();

        // Tính preferred dựa trên PHẦN CHỮ đang hiển thị
        Vector2 pref = dialogueText.GetPreferredValues(content, contentW, Mathf.Infinity);

        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentW);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pref.y);

        float targetH = Mathf.Max(minHeight, pref.y + padY * 2f);
        bgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, boxWidth);
        bgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetH);
    }
}
