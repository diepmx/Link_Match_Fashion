using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GIKCore.Tween
{
    public class TweenLocalPosition : ITween
    {
        // Fields
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3 m_From = Vector3.zero;
        [SerializeField] private Vector3 m_To = Vector3.zero;
        [SerializeField] private bool m_IgnoreZ = true;
        [Space]
        [Header("Priority")]
        [SerializeField] private Transform m_TransformFrom;
        [SerializeField] private Transform m_TransformTo;

        // Methods
        public ITween SetFrom(Vector3 f) { m_From = f; return this; }
        public ITween SetTo(Vector3 t) { m_To = t; return this; }
        public ITween SetTransformFrom(Transform t) { m_TransformFrom = t; return this; }
        public ITween SetTransformTo(Transform t) { m_TransformTo = t; return this; }

        private Vector3 from
        {
            get
            {
                Vector3 v3 = (m_TransformFrom != null ? m_TransformFrom.localPosition : m_From);
                if (m_IgnoreZ) v3.z = 0;
                return v3;
            }
        }
        private Vector3 to
        {
            get
            {
                Vector3 v3 = (m_TransformTo != null ? m_TransformTo.localPosition : m_To);
                if (m_IgnoreZ) v3.z = 0;
                return v3;
            }
        }

        public void DoReset2(Vector3 v3) { m_Target.localPosition = v3; }
        public override void DoReset()
        {
            DoReset2(from);
        }

        public override void DoKill() { m_Target.DOKill(); }
        protected override void InitData()
        {
            base.InitData();
            DoReset();
        }

        protected override DG.Tweening.Tween PlayOne()
        {
            return m_Target.DOLocalMove(to, duration);
        }
        protected override DG.Tweening.Tween PlayTimes()
        {
            Sequence seq = DOTween.Sequence(m_Target);
            seq.Append(m_Target.DOLocalMove(to, duration))
               .Append(m_Target.DOLocalMove(to, reverseDuration));

            //Adds the given interval to the end of the Sequence
            if (interval > 0)
                seq.AppendInterval(interval);
            return seq;
        }

        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}