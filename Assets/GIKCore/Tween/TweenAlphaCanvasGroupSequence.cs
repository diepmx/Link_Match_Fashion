using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GIKCore.Utilities;

namespace GIKCore.Tween
{
    public class TweenAlphaCanvasGroupSequence : ITween
    {
        // Fields
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField][Range(0f, 1f)] private float from = 1f;
        [SerializeField]
        [Tooltip("Data for run tween each step, range from 0 - 1")]
        private List<WaveStepProps<float>> m_WaveStep = new List<WaveStepProps<float>>();

        // Methods
        public ITween SetFrom(float f) { from = IMath.LimitAmount(f); return this; }
        public ITween SetWaveStep(List<WaveStepProps<float>> ws)
        {
            m_WaveStep.Clear();
            m_WaveStep.AddRange(ws);
            return this;
        }

        public void DoReset(float alpha) { m_CanvasGroup.alpha = IMath.LimitAmount(alpha); }
        public override void DoKill() { m_CanvasGroup.DOKill(); }
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

                Sequence seq = DOTween.Sequence(m_CanvasGroup);
                for (int i = 0; i < num; i++)
                {
                    WaveStepProps<float> step = m_WaveStep[i];
                    DG.Tweening.Tween tweenStep = m_CanvasGroup.DOFade(IMath.LimitAmount(step.to), step.duration);
                    ApplyEase2(tweenStep, step.ease, step.specialProps);

                    seq.Append(tweenStep);
                }
                seq.SetLoops(m_Loop)
                   .OnComplete(() =>
                   {
                       onTween = false;
                       //IUtil.InvokeEvent(m_OnTweenComplete);
                   });
                ApplyEase(seq);

                //Adds the given interval to the end of the Sequence
                if (interval > 0)
                    seq.AppendInterval(interval);

                //IUtil.InvokeEvent(m_OnTweenStart);
            }
        }

        // Start is called before the first frame update
        //void Start() { }

        // Update is called once per frame
        //void Update() { }
    }
}
