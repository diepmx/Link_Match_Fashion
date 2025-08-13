using UnityEngine;
using TMPro;

public class DressupLevelHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private string levelPrefix = "Lv. ";

    private void OnEnable()
    {
        TrySubscribeAndRefresh();
    }

    private void OnDisable()
    {
        if (DressupLevelSystem.Instance != null)
        {
            DressupLevelSystem.Instance.OnXpChanged -= HandleXpChanged;
        }
    }

    private void Update()
    {
        // In case HUD enabled before system init, attempt late subscribe once
        if (!subscribedOnce)
        {
            TrySubscribeAndRefresh();
        }
    }

    private bool subscribedOnce;

    private void TrySubscribeAndRefresh()
    {
        var sys = DressupLevelSystem.Instance;
        if (sys == null) return;
        sys.OnXpChanged -= HandleXpChanged;
        sys.OnXpChanged += HandleXpChanged;
        HandleXpChanged(sys.CurrentLevel, sys.CurrentXp, sys.GetRequiredXpForLevel(sys.CurrentLevel));
        subscribedOnce = true;
    }

    private void HandleXpChanged(int level, int currentXp, int requiredXp)
    {
        if (levelText != null) levelText.text = levelPrefix + level.ToString();
        if (xpText != null) xpText.text = currentXp + "/" + requiredXp;
    }
}


