using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterView : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform root;
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public CanvasGroup group;

    [Header("Enter/Exit")]
    public bool isLeftSide = true;      // Alice = true (từ trái), Ethan = false (từ phải)
    public float enterOffset = 900f;    // bay từ ngoài màn hình
    public float enterTime = 0.35f;
    public float overshoot = 30f;

    [Header("Next Hint (optional)")]
    public RectTransform nextHint;
    public float moveAmount = 10f;
    public float speed = 2f;

    [Header("Sizer (optional)")]
    public DialogueAutoSizer sizer;

    Vector2 home;
    Vector2 nextHintBase;
    bool hasNextHint;
    Vector3 baseScale;

    void Awake()
    {
        if (!root) root = transform as RectTransform;
        if (!group) group = GetComponent<CanvasGroup>();
        home = root.anchoredPosition;
        baseScale = root.localScale;                  // <- giữ scale gốc (vd 1.5)
        hasNextHint = nextHint != null;
        if (hasNextHint) nextHintBase = nextHint.anchoredPosition;
    }

    void Update()
    {
        if (!hasNextHint) return;
        float off = Mathf.Sin(Time.time * speed) * moveAmount;
        nextHint.anchoredPosition = new Vector2(nextHintBase.x + off, nextHintBase.y);
    }

    public void Bind(string displayName, Sprite portraitSprite, string line)
    {
        if (nameText) nameText.text = displayName;
        if (portrait) portrait.sprite = portraitSprite;
        if (dialogueText) dialogueText.text = line;
    }

    public void HideInstant()
    {
        // đẩy ra ngoài rìa theo phía của nhân vật, mờ đi, giữ baseScale
        root.anchoredPosition = home + new Vector2(isLeftSide ? -enterOffset : enterOffset, 0f);
        root.localScale = baseScale * 0.98f;
        if (group) group.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Enter()
    {
        gameObject.SetActive(true);

        // bắt đầu ngoài rìa
        root.anchoredPosition = home + new Vector2(isLeftSide ? -enterOffset : enterOffset, 0f);
        root.localScale = baseScale * 0.98f;
        if (group) group.alpha = 0f;

        var target = home;
        var over = home + new Vector2(isLeftSide ? +overshoot : -overshoot, 0f);

        if (group) LeanTween.alphaCanvas(group, 1f, enterTime * 0.75f);
        LeanTween.move(root, over, enterTime * 0.75f).setEaseOutQuad();
        LeanTween.scale(root, baseScale * 1.02f, enterTime * 0.75f).setEaseOutQuad()
            .setOnComplete(() =>
            {
                LeanTween.move(root, target, enterTime * 0.25f).setEaseInOutQuad();
                LeanTween.scale(root, baseScale, enterTime * 0.25f).setEaseInOutQuad();
            });
    }

    public void Exit()
    {
        // trượt ra ngoài rìa về đúng phía của nhân vật, rồi tắt
        var offPos = home + new Vector2(isLeftSide ? -enterOffset : enterOffset, 0f);
        LeanTween.scale(root, baseScale * 0.98f, enterTime * 0.5f).setEaseInQuad();
        if (group) LeanTween.alphaCanvas(group, 0f, enterTime * 0.5f);
        LeanTween.move(root, offPos, enterTime * 0.5f).setEaseInQuad()
            .setOnComplete(() => { gameObject.SetActive(false); });
    }

    public void Dim(bool on)
    {
        if (!group) return;
        LeanTween.alphaCanvas(group, on ? 0.6f : 1f, 0.2f);
    }
}
