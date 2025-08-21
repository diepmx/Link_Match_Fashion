using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GIKCore.Utilities;

namespace GIKCore.Tween
{
    public class TweenFillAmount : ITween
    {
        // Fields
        [SerializeField] private Image m_Target;
        [SerializeField][Range(0f, 1f)] private float from = 0f;
        [SerializeField][Range(0f, 1f)] private float to = 1f;

        // Methods
        public ITween SetFrom(float f) { from = IMath.LimitAmount(f); return this; }
        public ITween SetTo(float t) { to = IMath.LimitAmount(t); return this; }
        public void DoReset(float x) { m_Target.fillAmount = IMath.LimitAmount(x); }
        public override void DoKill() { m_Target.DOKill(); }
        protected override void InitData()
        {
            base.InitData();
            DoReset(from);
        }

        protected override DG.Tweening.Tween PlayOne()
        {
            return m_Target.DOFillAmount(to, duration);
        }
        protected override DG.Tweening.Tween PlayTimes()
        {
            Sequence seq = DOTween.Sequence(m_Target);
            seq.Append(m_Target.DOFillAmount(to, duration))
               .Append(m_Target.DOFillAmount(from, reverseDuration));

            //Adds the given interval to the end of the Sequence
            if (interval > 0)
                seq.AppendInterval(interval);
            return seq;
        }

        // Start is called before the first frame update
        //void Start() { }

        // Update is called once per frame
        //void Update() { }
    }
}
