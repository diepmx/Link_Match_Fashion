using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject
{
    public int currentLevel = 0;
    public int totalCoins = 0;
    public int Energy = 300;
    public int TotalGem = 0;
    public int CurrentChapter = 0;
    public float TimeRepeatEnergy = 300f;
    public int CurrentSteps = 0;
    [Button]
    public void resetData()
    {
        currentLevel = 0;
        totalCoins = 0;
        Energy = 300;
        TotalGem = 0;
        CurrentChapter = 0;
    }
}
