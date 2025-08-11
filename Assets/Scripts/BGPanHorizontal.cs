using UnityEngine;
using System.Collections;

public class BGPanHorizontal : MonoBehaviour
{
    public RectTransform background;
    public float aliceBGX;   // giá trị anchoredPosition.x của BG khi focus Alice
    public float ethanBGX;   // tương tự cho Ethan
    public float duration = 0.35f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    Coroutine co;

    public void FocusAlice() { PanTo(aliceBGX); }
    public void FocusEthan() { PanTo(ethanBGX); }
    public void FocusById(string id) { if (id == "Alice") FocusAlice(); else FocusEthan(); }

    void PanTo(float xTarget)
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(Pan(background.anchoredPosition.x, xTarget));
    }

    System.Collections.IEnumerator Pan(float from, float to)
    {
        var start = background.anchoredPosition;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float k = ease.Evaluate(Mathf.Clamp01(t));
            background.anchoredPosition = new Vector2(Mathf.LerpUnclamped(from, to, k), start.y);
            yield return null;
        }
        background.anchoredPosition = new Vector2(to, start.y);
    }
}
