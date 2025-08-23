using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private Vector2 distance = new Vector3(0, 0);
    [SerializeField] private float duration = 1f;
    [SerializeField] private float delay = 0f;
    private Vector2 originalPosition;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }
    public void Move()
    {
        rectTransform.DOAnchorPos(originalPosition + distance, duration).From(originalPosition)
            .SetDelay(delay)
            .SetEase(ease);
    }
    public void MoveBack()
    {
        rectTransform.DOAnchorPos(originalPosition, duration).From(originalPosition + distance)
            .SetDelay(delay)
            .SetEase(ease);
    }
}
