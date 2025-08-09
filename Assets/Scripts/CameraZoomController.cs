using UnityEngine;
using DG.Tweening;

public class CameraZoomController : MonoBehaviour
{
    [Header("Targets (tuỳ chọn)")]
    [Tooltip("Điểm đặt camera khi zoom Makeup (local của rig)")]
    [SerializeField] private Transform makeupTarget;

    [Header("Thời gian/Ease")]
    [SerializeField] private float duration = 0.5f;

    [Header("Tuỳ chọn FOV/Ortho khi Makeup")]
    [Tooltip("FOV khi Makeup (dùng nếu camera không orthographic)")]
    [SerializeField] private float makeupFOV = 30f;
    [Tooltip("Orthographic Size khi Makeup (dùng nếu camera orthographic)")]
    [SerializeField] private float makeupOrthoSize = 2f;

    [Header("Refs")]
    [Tooltip("Để trống thì dùng chính GameObject có script")]
    [SerializeField] private Transform camTransform;

    private Vector3 originalLocalPos;
    private Vector3 originalLocalEuler;
    private float originalFOV;
    private float originalOrthoSize;
    private Camera cam;

    void Awake()
    {
        if (camTransform == null) camTransform = transform;

        originalLocalPos = camTransform.localPosition;
        originalLocalEuler = camTransform.localEulerAngles;

        cam = camTransform.GetComponent<Camera>();
        if (cam != null)
        {
            originalFOV = cam.fieldOfView;
            originalOrthoSize = cam.orthographicSize;
        }
    }

    // Nút MAKEUP → zoom gần
    public void ZoomMakeup()
    {
        if (makeupTarget != null)
            ZoomTo(makeupTarget, makeupFOV, makeupOrthoSize);
        else
            // Nếu chưa set target, chỉ đổi FOV/Ortho để "zoom"
            ZoomTo(camTransform, makeupFOV, makeupOrthoSize, moveRotate: false);
    }

    // Nút CLOTHING → trở về nguyên trạng
    public void ZoomClothing() => ResetZoom();

    // Nút BACK → cũng trở về nguyên trạng
    public void ZoomBack() => ResetZoom();

    private void ResetZoom()
    {
        camTransform.DOKill();
        camTransform.DOLocalMove(originalLocalPos, duration).SetEase(Ease.OutQuad);
        camTransform.DOLocalRotate(originalLocalEuler, duration).SetEase(Ease.OutQuad);

        if (cam != null)
        {
            DOTween.Kill(cam);
            if (cam.orthographic)
                cam.DOOrthoSize(originalOrthoSize, duration).SetEase(Ease.OutQuad);
            else
                cam.DOFieldOfView(originalFOV, duration).SetEase(Ease.OutQuad);
        }
    }

    private void ZoomTo(Transform target, float fov, float ortho, bool moveRotate = true)
    {
        camTransform.DOKill();

        if (moveRotate)
        {
            camTransform.DOLocalMove(target.localPosition, duration).SetEase(Ease.OutQuad);
            camTransform.DOLocalRotate(target.localEulerAngles, duration).SetEase(Ease.OutQuad);
        }

        if (cam != null)
        {
            DOTween.Kill(cam);
            if (cam.orthographic)
                cam.DOOrthoSize(ortho, duration).SetEase(Ease.OutQuad);
            else
                cam.DOFieldOfView(fov, duration).SetEase(Ease.OutQuad);
        }
    }
}
