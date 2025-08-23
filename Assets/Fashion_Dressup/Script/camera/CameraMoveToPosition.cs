using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraMoveToPosition : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float m_Duration = 1.5f;
    Vector3 OldPosition;

    private void Awake()
    {
        OldPosition = m_Camera.transform.localPosition;
    }
    public void MoveCameraToPosition(Vector3 NewPosition)
    {
        m_Camera.transform.DOLocalMove(NewPosition , m_Duration)
                          .SetEase(Ease.InOutSine);
    }
}
