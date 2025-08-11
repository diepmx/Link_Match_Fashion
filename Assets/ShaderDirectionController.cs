using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShaderDirectionController : MonoBehaviour
{
    [SerializeField] Vector2 direction;       // set từ code khác hoặc inspector
    [SerializeField] float speed = 1f;
    [SerializeField] string dirProp = "_Direction";
    [SerializeField] string speedProp = "_Speed";
    [SerializeField] string playheadProp = "_Playhead";

    float playhead; // thời gian tuỳ chỉnh
    Renderer rend;
    MaterialPropertyBlock mpb;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat(playheadProp, 0f);
        rend.SetPropertyBlock(mpb);
    }

    void Update()
    {
        // Chỉ tăng playhead khi direction != 0
        if (direction.sqrMagnitude > 0.000001f)
            playhead += Time.deltaTime;

        // Đẩy giá trị xuống shader (per-renderer, không clone material)
        rend.GetPropertyBlock(mpb);
        mpb.SetVector(dirProp, direction);
        mpb.SetFloat(speedProp, speed);
        mpb.SetFloat(playheadProp, playhead);
        rend.SetPropertyBlock(mpb);
    }

    // Gọi hàm này từ input/logic game của bạn để đổi hướng
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    // Nếu muốn “đóng băng” ngay lập tức:
    public void FreezeNow()
    {
        // không tăng playhead nữa vì direction = 0
        direction = Vector2.zero;
    }

    // Nếu muốn reset hiệu ứng về đầu:
    public void ResetPlayhead()
    {
        playhead = 0f;
    }
}
