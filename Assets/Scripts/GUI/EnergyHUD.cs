using UnityEngine;
using TMPro;

public class EnergyHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private string prefix = "";
    [SerializeField] private bool showMax = true;
    [SerializeField] private string separator = "/";

    private void OnEnable()
    {
        InitScript.OnEnergyChanged += HandleEnergyChanged;
        UpdateText(InitScript.Energy);
    }

    private void OnDisable()
    {
        InitScript.OnEnergyChanged -= HandleEnergyChanged;
    }

    private void HandleEnergyChanged(int current)
    {
        UpdateText(current);
    }

    private void UpdateText(int current)
    {
        if (energyText == null) return;
        if (showMax)
        {
            energyText.text = string.IsNullOrEmpty(prefix)
                ? $"{current}{separator}{InitScript.Instance.EnergyMax}"
                : $"{prefix}{current}{separator}{InitScript.Instance.EnergyMax}";
        }
        else
        {
            energyText.text = string.IsNullOrEmpty(prefix) ? current.ToString() : prefix + current.ToString();
        }
    }
}


