using UnityEngine;
using GIKCore.Utilities;
using DG.Tweening;

namespace GIKCore.Tween
{
    public class TweenScale : ITween
    {
        // Fields
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3 from = Vector3.one;
        [SerializeField] private Vector3 to = Vector3.one;

        // Methods
        public ITween SetFrom(Vector3 f) { from = f; return this; }
        public ITween SetTo(Vector3 t) { to = t; return this; }
        public void DoReset(Vector3 scale) { m_Target.transform.localScale = scale; }

        public override void DoReset()
        {
            DoReset(from);
        }
        public override void DoKill()
        {
            m_Target.DOKill();
        }
        protected override void InitData()
        {
            base.InitData();
            DoReset();
        }

        private void Start()
        {
            InitData();
            PlayOne();
        }

        protected override DG.Tweening.Tween PlayOne()
        {
            return m_Target.DOScale(to, duration);
        }
        protected override DG.Tweening.Tween PlayTimes()
        {
            Sequence seq = DOTween.Sequence(m_Target);
            seq.Append(m_Target.DOScale(to, duration))
               .Append(m_Target.DOScale(from, reverseDuration));
            if (interval > 0)
                seq.AppendInterval(interval);
            return seq;
        }
    }
}
