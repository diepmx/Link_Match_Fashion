using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GIKCore.Tween
{
    [Serializable]
    public class TweenShakeRotationProps
    {
        public float duration = 0.3f;
        [Tooltip("default: (90, 90, 90); The shake strength")]
        public Vector3 strength = new Vector3(90f, 90f, 90f);
        [Tooltip("default: 10; How much will the shake vibrate.")]
        public int vibrato = 10;
        [Tooltip("default: 90; How much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). Setting it to 0 will shake along a single direction.\nNOTE: if you're shaking a single axis via the Vector3 strength parameter, randomness should be left to at least 90.")]
        public float randomness = 90;
        [Tooltip("default: true; If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.")]
        public bool fadeOut = true;
        [Tooltip("default: Full; The type of randomness to apply, Full (fully random) or Harmonic (more balanced and visually more pleasant).")]
        public ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;
    }
    public class TweenShakeRotation : ITween
    {
        //Fields
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3 m_From = Vector3.zero;
        [SerializeField]
        private List<TweenShakeRotationProps> m_ListProps = new List<TweenShakeRotationProps>()
        {
            new TweenShakeRotationProps() { duration = 0.3f, strength = new Vector3(90f, 90f, 90f), vibrato = 10, randomness = 90, fadeOut = true, randomnessMode = ShakeRandomnessMode.Full }
        };

        // Methods
        public void DoReset(Vector3 euler)
        {
            m_Target.rotation = Quaternion.Euler(euler);
        }
        public override void DoReset()
        {
            DoReset(m_From);
        }

        public override void DoKill() { m_Target.DOKill(); }
        protected override void InitData()
        {
            base.InitData();

            DoReset();

            Sequence seq = DOTween.Sequence(m_Target);
            for (int i = 0; i < m_ListProps.Count; i++)
            {
                TweenShakeRotationProps props = m_ListProps[i];
                seq.Append(m_Target.DOShakeRotation(props.duration, props.strength, props.vibrato, props.randomness, props.fadeOut, props.randomnessMode));
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
