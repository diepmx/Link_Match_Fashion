using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(ScrollRect))]
public class ScrollHelper : MonoBehaviour
{

    // Fields        
    [SerializeField] private ScrollRect m_ScrollRect;
    [SerializeField] private bool m_BounceBackToFitChild = false;
    [SerializeField] private string m_IgnoreNote = "Ignore Note";
    [SerializeField] private List<GameObject> m_Ignores;

    // Values
    private ICallback.CallFunc2<Vector2> onScrollCompleteCB;
    private ICallback.CallFunc2<int> onBounceBackCB;

    private List<RectTransform> lstChild = new List<RectTransform>();
    private Vector2 pointFrom = Vector2.zero, pointTo = Vector2.zero;
    private float mDuration = 0f, mTime = 0f;
    public bool onTween { get; private set; } = false;
    public bool onDrag { get; private set; } = false;

    // Methods
    public ScrollRect scrollRect { get { return m_ScrollRect; } }
    public RectTransform content { get { return m_ScrollRect.content; } }
    public RectTransform viewport { get { return m_ScrollRect.viewport; } }
    public int childCount { get { return lstChild.Count; } }

    /// <summary>
    /// Only available in drag mode
    /// </summary>
    /// <param name="func"></param>
    public void SetBounceBackCallback(ICallback.CallFunc2<int> func) { onBounceBackCB = func; }

    /// <summary>
    /// Scroll the content to the percent position of ScrollRect in any direction.    
    /// <para>#Vector2 percent: A point which will be clamp between (0,0) and (1,1); 
    /// <br> ++ horizontal: percent(0, 0) -> content(0, 0), percent(1, 0) -> content(0 - MaxAbsX, 0);</br>
    /// <br> ++ vertical: percent(0, 0) -> content(0, 0 + MaxAbsY), percent(0, 1) -> content(0, 0)</br>
    /// <br> ++ view of bound: bottom-right(0, 0); top-right(0, 1); bottom-left(1, 0); top-left(1, 1)</br>
    /// </para>
    /// <para>#float duration: Scroll time in second, if you don't pass duration, the content will jump to the percent position of ScrollRect immediately.</para>
    /// <para>#Callback complete: the callback function will be execute when scroll complete</para>
    /// <para>#bool rebuildLayoutImmediate: force rebuild layout immediately if true; using this param in case of dynamic child (active, inactive, destroy, add) </para>
    /// </summary>  
    public void ScrollTo(Vector2 percent, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        if (rebuildLayoutImmediate) RebuildLayoutImmediate();

        onScrollCompleteCB = complete;
        pointFrom = content.anchoredPosition;
        pointTo = GetPointTo(scrollRect, percent, pointFrom, null);

        if (pointFrom.x != pointTo.x || pointFrom.y != pointTo.y)
        {
            if (duration > 0)
            {
                mDuration = duration;
                mTime = 0f;
                onTween = true;
            }
            else
            {
                content.anchoredPosition = pointTo;
                InvokeComplete();
            }
        }
        else InvokeComplete();
    }
    public void ScrollToFirst(float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 percent = Vector2.zero;
        if (scrollRect.vertical) percent.y = 1;
        ScrollTo(percent, duration, complete, rebuildLayoutImmediate);
    }
    public void ScrollToLast(float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 percent = Vector2.zero;
        if (scrollRect.horizontal) percent.x = 1;
        ScrollTo(percent, duration, complete, rebuildLayoutImmediate);
    }
    public void ScrollToPosition(Vector2 to, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        ScrollTo(GetPercent(scrollRect, to, () =>
        {
            if (rebuildLayoutImmediate)
                RebuildLayoutImmediate();
        }), duration, complete, false);
    }
    public void ScrollToPositionX(float x, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 to = content.anchoredPosition;
        to.x = x;
        ScrollToPosition(to, duration, complete, rebuildLayoutImmediate);
    }
    public void ScrollToPositionY(float y, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 to = content.anchoredPosition;
        to.y = y;
        ScrollToPosition(to, duration, complete, rebuildLayoutImmediate);
    }
    public void ScrollToOffset(Vector2 offset, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 to = content.anchoredPosition + offset;
        ScrollTo(GetPercent(scrollRect, to, () =>
        {
            if (rebuildLayoutImmediate)
                RebuildLayoutImmediate();
        }), duration, complete, false);
    }
    public void ScrollToOffsetX(float offsetX, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 offset = new Vector2(offsetX, 0);
        ScrollToOffset(offset, duration, complete, rebuildLayoutImmediate);
    }
    public void ScrollToOffsetY(float offsetY, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        Vector2 offset = new Vector2(0, offsetY);
        ScrollToOffset(offset, duration, complete, rebuildLayoutImmediate);
    }
    public int ScrollToChild(int index, float duration, ICallback.CallFunc2<Vector2> complete = null, bool rebuildLayoutImmediate = false)
    {
        if (rebuildLayoutImmediate || childCount <= 0)
            RebuildLayoutImmediate();

        if (childCount <= 0) return -1;

        if (index >= childCount) index = childCount - 1;
        if (index < 0) index = 0;

        Vector2 to = lstChild[index].anchoredPosition;
        to.x *= -1; to.y *= -1;

        ScrollTo(GetPercent(scrollRect, to), duration, complete, false);
        return index;
    }

    public void DoReset(bool resetPosition = true)
    {
        onScrollCompleteCB = null;
        onBounceBackCB = null;
        InvokeComplete();
        if (resetPosition) content.anchoredPosition = Vector2.zero;
    }

    private void BounceBackToFitChild()
    {
        if (m_BounceBackToFitChild && !onDrag && !onTween)
        {
            scrollRect.StopMovement();

            int index = 0;
            int numChild = childCount;

            if (scrollRect.vertical)
            {
                float offsetY = content.anchoredPosition.y; //init posY is at y = 0 (local)
                for (int i = 0; i < numChild; i++)
                {
                    RectTransform go = lstChild[i];
                    float yTop = go.anchoredPosition.y + offsetY;
                    float yBot = yTop - go.rect.height;

                    if (yBot > 0f) continue;
                    if (yTop - go.rect.height * 0.5f < 0f)
                    {
                        index = i;
                        break;
                    }
                    if (yTop - go.rect.height * 0.5f >= 0f)
                    {
                        index = i + 1;
                        break;
                    }
                }
            }
            if (scrollRect.horizontal)
            {
                float offsetX = content.anchoredPosition.x;//init posX is at x = 0 (local)                    
                for (int i = 0; i < numChild; i++)
                {
                    RectTransform go = lstChild[i];
                    float xLeft = go.anchoredPosition.x + offsetX;
                    float xRight = xLeft + go.rect.width;
                    if (xRight < 0f) continue;
                    if (xLeft + go.rect.width * 0.5f <= 0f)
                    {
                        index = i + 1;
                        break;
                    }
                    if (xLeft + go.rect.width * 0.5f > 0f)
                    {
                        index = i;
                        break;
                    }
                }
            }

            index = ScrollToChild(index, 0.2f);
            if (onBounceBackCB != null) onBounceBackCB(index);
        }
    }

    /// <summary>
    /// In case of dynamic child (active, inactive, destroy add), please call this function before you call any other function  
    /// </summary>
    public ScrollHelper RebuildLayoutImmediate()
    {
        lstChild.Clear();
        foreach (Transform child in content)
        {
            if (child.gameObject.activeSelf)
            {
                if (!IsIgnore(child.name))
                {
                    RectTransform rt = child.GetComponent<RectTransform>();
                    Normalize(rt);
                    lstChild.Add(rt);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        return this;
    }
    public Vector2 GetMaxAbs()
    {
        return GetMaxAbs(m_ScrollRect);
    }

    private bool IsIgnore(string nameCheck)
    {
        int idx = m_Ignores.FindIndex(x => x.name.Equals(nameCheck));
        return idx >= 0;
    }
    private void Normalize(RectTransform rt)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.up;
    }
    private void InvokeComplete()
    {
        onTween = false;
        mDuration = mTime = 0f;
        scrollRect.StopMovement();
        if (onScrollCompleteCB != null) onScrollCompleteCB(content.anchoredPosition);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (m_ScrollRect == null) m_ScrollRect = GetComponent<ScrollRect>();
        Normalize(content);
    }
    void Start()
    {
        RebuildLayoutImmediate();
    }

    // Update is called once per frame
    //void Update() { }

    void FixedUpdate()
    {
        if (mDuration > 0f)
        {
            content.anchoredPosition = Vector2.Lerp(pointFrom, pointTo, mTime / mDuration);
            mTime += Time.deltaTime;
            if (mTime >= mDuration)
            {
                content.anchoredPosition = pointTo;
                InvokeComplete();
            }
        }
    }

    // Event Systems        
    public void OnBeginDrag(PointerEventData eventData)
    {
        onDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onDrag = false;
        BounceBackToFitChild();
    }

    public static Vector2 GetMaxAbs(ScrollRect scrollRect)
    {
        Vector2 maxAbs = new Vector2();
        maxAbs.x = (scrollRect.content.rect.width > scrollRect.viewport.rect.width) ? (scrollRect.content.rect.width - scrollRect.viewport.rect.width) : 0f;
        maxAbs.y = (scrollRect.content.rect.height > scrollRect.viewport.rect.height) ? (scrollRect.content.rect.height - scrollRect.viewport.rect.height) : 0f;
        return maxAbs;
    }
    public static Vector2 GetPercent(ScrollRect scrollRect, Vector2 to, ICallback.CallFunc rebuild = null)
    {
        if (rebuild != null)
            rebuild();
        Vector2 maxAbs = GetMaxAbs(scrollRect);
        Vector2 percent = Vector2.zero;
        if (scrollRect.vertical)
        {
            percent.y = 1 - ((maxAbs.y != 0) ? (to.y / maxAbs.y) : 0);
        }
        if (scrollRect.horizontal)
        {
            percent.x = (maxAbs.x != 0) ? (-to.x / maxAbs.x) : 0;
        }

        if (percent.x < 0) percent.x = 0;
        if (percent.x > 1) percent.x = 1;
        if (percent.y < 0) percent.y = 0;
        if (percent.y > 1) percent.y = 1;

        return percent;
    }
    public static Vector2 GetPointTo(ScrollRect scrollRect, Vector2 percent, Vector2 pointToDefault, ICallback.CallFunc rebuild = null)
    {
        if (rebuild != null)
            rebuild();
        Vector2 maxAbs = GetMaxAbs(scrollRect);
        if (percent.x < 0) percent.x = 0;
        if (percent.x > 1) percent.x = 1;
        if (percent.y < 0) percent.y = 0;
        if (percent.y > 1) percent.y = 1;

        Vector2 pointTo = pointToDefault;
        if (scrollRect.vertical) pointTo.y = (1 - percent.y) * maxAbs.y;
        if (scrollRect.horizontal) pointTo.x = 0 - percent.x * maxAbs.x;

        return pointTo;
    }
    public static Vector2 GetPointTo2(ScrollRect scrollRect, Vector2 pointToExpect, Vector2 pointToDefault, ICallback.CallFunc rebuild = null)
    {
        if (rebuild != null)
            rebuild();
        Vector2 percent = GetPercent(scrollRect, pointToExpect, null);
        return GetPointTo(scrollRect, percent, pointToDefault, null);
    }

}
