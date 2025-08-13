using System;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class DressupLevelSystem : MonoBehaviour
{
    public static DressupLevelSystem Instance { get; private set; }

    [SerializeField] private int baseXpPerLevel = 100;
    [SerializeField] private float growthPerLevel = 1.25f;

    private const string XpKey = "Dressup_XP";
    private const string LevelKey = "Dressup_Level";

    public int CurrentLevel { get; private set; }
    public int CurrentXp { get; private set; }

    public event Action<int, int, int> OnXpChanged; // level, currentXp, requiredXp
    public event Action<int> OnLevelUp; // new level

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentLevel = PlayerPrefs.GetInt(LevelKey, 1);
        CurrentXp = PlayerPrefs.GetInt(XpKey, 0);
        Broadcast();
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        CurrentXp += amount;
        int required = GetRequiredXpForLevel(CurrentLevel);
        bool leveled = false;
        while (CurrentXp >= required)
        {
            CurrentXp -= required;
            CurrentLevel++;
            leveled = true;
            required = GetRequiredXpForLevel(CurrentLevel);
        }
        Save();
        if (leveled) OnLevelUp?.Invoke(CurrentLevel);
        Broadcast();
    }

    public int GetRequiredXpForLevel(int level)
    {
        if (level <= 1) return baseXpPerLevel;
        double value = baseXpPerLevel * Math.Pow(growthPerLevel, level - 1);
        return Mathf.CeilToInt((float)value);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(LevelKey, CurrentLevel);
        PlayerPrefs.SetInt(XpKey, CurrentXp);
        PlayerPrefs.Save();
    }

    private void Broadcast()
    {
        OnXpChanged?.Invoke(CurrentLevel, CurrentXp, GetRequiredXpForLevel(CurrentLevel));
    }
}


