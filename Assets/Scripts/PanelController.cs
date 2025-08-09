using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class PanelController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Vector3 startScale = Vector3.one *0.5f;
    [SerializeField] private Vector3 endScale = Vector3.one;

    [Header("Refs (optional)")]
    [SerializeField] private RectTransform rectTransform; // kéo thả nếu muốn

    private bool isOpen = false;

    void Awake()
    {
        // Cache sớm để tránh gọi trước Start
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("[PanelController] RectTransform is NULL", this);
        }
    }

    //void Start()
    //{
    //    // Ẩn ngay khi khởi tạo (đã có rectTransform)
    //    CloseImmediate();
    //}

    public void OpenPanel()
    {
        if (isOpen) return;
        if (rectTransform == null) { Debug.LogError("RectTransform NULL in OpenPanel", this); return; }

        isOpen = true;
        gameObject.SetActive(true);

        rectTransform.DOKill();
        rectTransform.localScale = startScale;
        rectTransform.DOScale(endScale, animationDuration).SetEase(Ease.OutQuad);
    }

    public void ClosePanel()
    {
        if (!isOpen) return;
        if (rectTransform == null) { Debug.LogError("RectTransform NULL in ClosePanel", this); return; }

        isOpen = false;

        rectTransform.DOKill();
        rectTransform.DOScale(startScale, animationDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rectTransform.localScale = startScale;
                gameObject.SetActive(false);
            });
    }

    public void OpenImmediate()
    {
        if (rectTransform == null) { Debug.LogError("RectTransform NULL in OpenImmediate", this); return; }
        isOpen = true;
        gameObject.SetActive(true);
        rectTransform.DOKill();
        rectTransform.localScale = endScale;
    }

    public void CloseImmediate()
    {
        if (rectTransform == null) return;
        isOpen = false;
        rectTransform.DOKill();
        rectTransform.localScale = startScale;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (rectTransform != null) rectTransform.DOKill();
    }
}
