using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinsHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private string prefix = "";

    private void OnEnable()
    {
        InitScript.OnCoinsChanged += HandleCoinsChanged;
        UpdateText(InitScript.Coins);
    }

    private void OnDisable()
    {
        InitScript.OnCoinsChanged -= HandleCoinsChanged;
    }

    private void HandleCoinsChanged(int coins)
    {
        UpdateText(coins);
    }

    private void UpdateText(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = string.IsNullOrEmpty(prefix) ? coins.ToString() : prefix + coins.ToString();
        }
    }
}


