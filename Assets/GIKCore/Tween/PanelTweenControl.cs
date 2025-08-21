using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GIKCore.Tween
{
    [Serializable]
    public class TweenControlProps
    {
        public string key;
        public GameObject go;

        public void SetActive(bool on)
        {
            if (go != null) go.SetActive(on);
        }

        public void Validate()
        {
            if (go != null && string.IsNullOrEmpty(key))
            {
                key = go.name;
            }
        }
    }

    public class TweenControlNetData
    {
        public string keyParent;
        public string keySub;
    }
    public class PanelTweenControl : GameListener
    {
        // Fields
        [SerializeField] private string m_KeyParent = "Tween-Control";
        [SerializeField] private List<TweenControlProps> m_ListTweenFocus, m_ListTweenBlur;
        [SerializeField] private UnityEvent m_OnEnable;

        // Methods
        public string keyParent { get { return m_KeyParent; } }
        public void ShowTweenFocus(string key = "")
        {
            SetActiveTween(false, m_ListTweenFocus, key, true);
        }
        public void HideTweenFocus(string key = "")
        {
            SetActiveTween(false, m_ListTweenFocus, key, false);
        }
        public void HideAllTweenFocus()
        {
            SetActiveTween(true, m_ListTweenFocus, "", false);
        }

        public void ShowTweenBlur(string key = "")
        {
            SetActiveTween(false, m_ListTweenBlur, key, true);
        }
        public void HideTweenBlur(string key = "")
        {
            SetActiveTween(false, m_ListTweenBlur, key, false);
        }
        public void HideAllTweenBlur()
        {
            SetActiveTween(true, m_ListTweenBlur, "", false);
        }

        public void HideAll()
        {
            HideAllTweenFocus();
            HideAllTweenBlur();
        }

        public void EditorSendEventFocus(string param)
        {
            EditorParseParam(param, out string keySub, out float delay);
            SendEventFocus(keySub, delay);
        }
        public void EditorSendEventBlur(string param)
        {
            EditorParseParam(param, out string keySub, out float delay);
            SendEventBlur(keySub, delay);
        }

        private void EditorParseParam(string param, out string keySub, out float delay)
        {//delay|keySub; Ex: 1|TweenFocus
            keySub = "";//active index 0 by default
            delay = 0f;

            if (!string.IsNullOrEmpty(param))
            {
                string[] split = param.Split("|");
                for (int i = 0; i < split.Length; i++)
                {
                    string si = split[i];
                    if (i == 0) delay = float.Parse(si);
                    else if (i == 1) keySub = si;
                }
            }
        }

        public void SendEventFocus(string keySub = "", float delay = 0f)
        {
            NetData nd = new NetData()
            {
                id = NetData.TWEEN_FOCUS_ACTIVE,
                data = new TweenControlNetData() { keyParent = m_KeyParent, keySub = keySub },
            };
        }
        public void SendEventBlur(string keySub = "", float delay = 0f)
        {
            NetData nd = new NetData()
            {
                id = NetData.TWEEN_BLUR_ACTIVE,
                data = new TweenControlNetData() { keyParent = m_KeyParent, keySub = keySub },
            };
        }
        private void ProcessTween(NetData arg)
        {
            TweenControlNetData tcnd = arg.data != null ? (TweenControlNetData)arg.data : null;
            string keyP = tcnd != null ? tcnd.keyParent : "";
            string keyS = tcnd != null ? tcnd.keySub : "";

            if (keyP.Equals(m_KeyParent))
            {
                switch (arg.id)
                {
                    case NetData.TWEEN_FOCUS_ACTIVE:
                        {
                            ShowTweenFocus(keyS);
                            break;
                        }
                    case NetData.TWEEN_BLUR_ACTIVE:
                        {
                            ShowTweenBlur(keyS);
                            break;
                        }
                }
            }
        }
        private void SetActiveTween(bool all, List<TweenControlProps> target, string key, bool on)
        {
            if (target == null || target.Count <= 0) return;

            if (all)
            {
                foreach (TweenControlProps elm in target)
                {
                    elm.SetActive(on);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(key))
                {
                    target[0].SetActive(on);
                }
                else
                {
                    foreach (TweenControlProps elm in target)
                    {
                        if (elm.key.Equals(key))
                        {
                            elm.SetActive(on);
                            break;
                        }
                    }
                }
            }
        }

        void OnEnable()
        {
        }

        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}

        void OnDisable()
        {
            HideAll();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            foreach (TweenControlProps elm in m_ListTweenFocus)
            {
                elm.Validate();
            }

            foreach (TweenControlProps elm in m_ListTweenBlur)
            {
                elm.Validate();
            }
        }
#endif
    }
}
