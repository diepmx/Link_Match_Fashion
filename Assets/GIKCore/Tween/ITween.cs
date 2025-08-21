using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using GIKCore.Utilities;
using DG.Tweening;

namespace GIKCore.Tween
{
    [System.Serializable]
    public class EaseSpecialProps
    {
        [Header("[Priority order from 0 - 1]")]
        public bool order0;
        [Tooltip("Eventual overshoot to use with Back or Flash ease (default is 1.70158 - 1 for Flash)")]
        public float overshoot;
        [Space]
        public bool order1;
        [Tooltip("Eventual amplitude to use with Elastic easeType or overshoot to use with Flash easeType (default is 1.70158 - 1 for Flash).")]
        public float amplitude;
        [Tooltip("Eventual period to use with Elastic or Flash easeType (default is 0)")]
        public float period;
    }
    [System.Serializable]
    public class WaveStepProps<T>
    {
        public T to;
        [Min(0)]
        public float duration = 0f;
        public Ease ease = Ease.Unset;
        public EaseSpecialProps specialProps = new EaseSpecialProps();
    }
    public abstract class ITween : MonoBehaviour
    {
        // Fields
        [SerializeField] protected MonoBehaviour m_Behaviour;
        [SerializeField][Min(0)] protected float duration = 0.3f;
        [SerializeField][Min(0)] protected float reverseDuration = 0f;
        [SerializeField][Min(0)] protected float delayBeforePlay = 0f;
        [SerializeField][Min(0)] protected float interval = 0f;
        [SerializeField]
        protected int m_Loop = 0;
        [SerializeField] protected Ease m_Ease = Ease.Linear;
        [SerializeField] protected EaseSpecialProps m_EaseSpecialProps = new EaseSpecialProps();

        [SerializeField] protected bool m_AutoplayOnEnable = true;

        [SerializeField]
        [Tooltip("TRUE => we will kill tween from the running tween list before run tween")]
        protected bool m_DoKill = true;

        [SerializeField]
        [Tooltip("Execute when GameObject become enable")]
        protected UnityEvent m_OnEnable;

        [SerializeField]
        [Tooltip("Execute when call Play function and before call PlayTween function")]
        protected UnityEvent m_OnPlay;

        [SerializeField]
        [Tooltip("Execute when call PlayTween function, right after tween start")]
        protected UnityEvent m_OnTweenStart;

        [SerializeField]
        [Tooltip("Execute right after tween complete")]
        protected UnityEvent m_OnTweenComplete;

        // Values
        public bool onTween { get; protected set; } = false;
        public bool activeSelf { get { return gameObject.activeSelf; } }
        public ITween SetActive(bool on) { gameObject.SetActive(on); return this; }

        protected MonoBehaviour behaviour { get { return m_Behaviour != null ? m_Behaviour : this; } }

        // Methods
        /// <summary>
        /// Number of cycles to play: -1 = play infinity; 0 and 1 = play one time;
        /// </summary>    
        public ITween SetLoop(int loop) { m_Loop = Mathf.Max(loop, -1); return this; }
        public ITween SetDuration(float d) { duration = Mathf.Max(d, 0); return this; }
        public ITween SetReverseDuration(float rd) { reverseDuration = Mathf.Max(rd, 0); return this; }
        public ITween SetInterval(float i) { interval = Mathf.Max(i, 0); return this; }

        /// <summary>Execute when gameobject become enable</summary>    
        public ITween AddOnEnableEvent(UnityAction ua)
        {
            if (m_OnEnable == null)
                m_OnEnable = new UnityEvent();
            m_OnEnable.AddListener(ua);
            return this;
        }
        /// <summary>Execute when Play function call and before PlayTween function call</summary> 
        public ITween AddOnPlayEvent(UnityAction ua)
        {
            if (m_OnPlay == null)
                m_OnPlay = new UnityEvent();
            m_OnPlay.AddListener(ua);
            return this;
        }
        /// <summary>Execute when PlayTween function call, right after tween start</summary>
        public ITween AddOnTweenStartEvent(UnityAction ua)
        {
            if (m_OnTweenStart == null)
                m_OnTweenStart = new UnityEvent();
            m_OnTweenStart.AddListener(ua);
            return this;
        }
        /// <summary>Execute right after tween complete</summary>
        public ITween AddOnTweenCompleteEvent(UnityAction ua)
        {
            if (m_OnTweenComplete == null)
                m_OnTweenComplete = new UnityEvent();
            m_OnTweenComplete.AddListener(ua);
            return this;
        }

        public virtual void Play(float seconds = 0f)
        {

            StopAllCoroutines();
        }

        public virtual void DoReset() { }
        public virtual void DoKill() { }

        protected virtual void PlayTween()
        {
            InitData();

            DG.Tweening.Tween t = (m_Loop < 0 || m_Loop > 1) ? PlayTimes() : PlayOne();
            if (t != null)
            {
                t.SetLoops(m_Loop)
                 .OnComplete(() =>
                 {
                     onTween = false;
                 });
                ApplyEase(t);
            }
        }
        protected virtual void InitData()
        {
            if (m_DoKill) DoKill();
            onTween = true;
        }

        protected virtual DG.Tweening.Tween PlayOne() { return null; }
        protected virtual DG.Tweening.Tween PlayTimes() { return null; }

        protected void ApplyEase(DG.Tweening.Tween t)
        {
            ApplyEase2(t, m_Ease, m_EaseSpecialProps);
        }
        protected void ApplyEase2(DG.Tweening.Tween t, Ease ease, EaseSpecialProps specialProps)
        {
            if (ease != Ease.Unset)
            {
                if (specialProps.order0)
                {
                    t.SetEase(ease, specialProps.overshoot);
                }
                else if (specialProps.order1)
                {
                    t.SetEase(ease, specialProps.amplitude, specialProps.period);
                }
                else
                {
                    t.SetEase(ease);
                }
            }
        }

        // Use this for initialization
        //void Start() { }

        // Update is called once per frame
        //void Update() { }

        void OnEnable()
        {
            if (m_AutoplayOnEnable) Play(delayBeforePlay);
        }

        void OnDestroy()
        {
            DoKill();
        }

        public static Tweener CreateSimpleTween(float duration, float from = 0f, float to = 1f, ICallback.CallFunc2<float> onUpdate = null, ICallback.CallFunc2<float> onComplete = null, Tweener tweenExist = null)
        {
            if (tweenExist != null)
                tweenExist.Kill();

            float amount = from;
            Tweener t = DOTween.To(() => amount, x => amount = x, to, duration)
                   .OnUpdate(() => { onUpdate?.Invoke(amount); })
                   .OnComplete(() => { onComplete?.Invoke(amount); });
            return t;
        }
    }
}
