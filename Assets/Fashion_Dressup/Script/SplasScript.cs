using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplasScript : MonoBehaviour
{
    [SerializeField] private Image m_targetImage;
    [SerializeField] private float m_fillSpeed = 0.5f;

    private void Awake()
    {
        m_targetImage.fillAmount = 0f;
        Application.targetFrameRate = 60;
        StartCoroutine(FillImage(() =>
        {
            SceneManager.LoadScene("dressup");
        }));
    }

    private IEnumerator FillImage(ICallback.CallFunc callBack)
    {
        while (m_targetImage.fillAmount < 1f)
        {
            m_targetImage.fillAmount += m_fillSpeed * Time.deltaTime;
            if (m_targetImage.fillAmount >= 1)
            {
                callBack?.Invoke();
                break;
            }
            yield return null;
        }
    }
}
