using UnityEngine;
using TMPro;

public class GemsHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private string prefix = "";

    private void OnEnable()
    {
        InitScript.OnGemsChanged += HandleGemsChanged;
        UpdateText(InitScript.Gems);
    }

    private void OnDisable()
    {
        InitScript.OnGemsChanged -= HandleGemsChanged;
    }

    private void HandleGemsChanged(int value)
    {
        UpdateText(value);
    }

    private void UpdateText(int value)
    {
        if (gemsText == null) return;
        gemsText.text = string.IsNullOrEmpty(prefix) ? value.ToString() : prefix + value.ToString();
    }
}


