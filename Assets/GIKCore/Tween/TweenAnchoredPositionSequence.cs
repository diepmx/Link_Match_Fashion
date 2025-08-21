using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GIKCore.Utilities;
using DG.Tweening;

namespace GIKCore.Tween
{
    public class TweenAnchoredPositionSequence : ITween
    {
        // Fields
        [SerializeField] private RectTransform m_RectTarget;
        [SerializeField] private Vector2 from = Vector2.zero;
        [SerializeField]
        [Tooltip("Data for run tween each step")]
        private List<WaveStepProps<Vector2>> m_WaveStep = new List<WaveStepProps<Vector2>>();

        // Methods
        public ITween SetFrom(Vector3 f) { from = f; return this; }
        public ITween SetWaveStep(List<WaveStepProps<Vector2>> ws)
        {
            m_WaveStep.Clear();
            m_WaveStep.AddRange(ws);
            return this;
        }

        public void DoReset(Vector2 v2) { m_RectTarget.anchoredPosition = v2; }
        public override void DoKill() { m_RectTarget.DOKill(); }
        protected override void InitData()
        {
            if (m_DoKill) DoKill();
            DoReset(from);
        }
        protected override void PlayTween()
        {
            InitData();
            int num = m_WaveStep.Count;
            if (num > 0)
            {
                onTween = true;

                Sequence seq = DOTween.Sequence(m_RectTarget);
                for (int i = 0; i < num; i++)
                {
                    WaveStepProps<Vector2> step = m_WaveStep[i];
                    DG.Tweening.Tween tweenStep = m_RectTarget.DOAnchorPos(step.to, step.duration);
                    ApplyEase2(tweenStep, step.ease, step.specialProps);
                    seq.Append(tweenStep);
                }
                seq.SetLoops(m_Loop)
                   .OnComplete(() =>
                   {
                       onTween = false;
                   });  
                ApplyEase(seq);

                //Adds the given interval to the end of the Sequence
                if (interval > 0)
                    seq.AppendInterval(interval);

            }
        }

        // Start is called before the first frame update
        //void Start() { }

        // Update is called once per frame
        //void Update() { }
    }
}
