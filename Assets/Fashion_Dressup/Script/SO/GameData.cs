using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject
{
    public int currentLevel = 1;
    public int totalCoins = 0;
    public int Energy = 300;
    public int TotalGem = 0;
    public int CurrentChapter = 1;
    public float TimeRepeatEnergy = 300f;
    public int CurrentSteps = 0;
    [Button]
    public void resetData()
    {
        currentLevel = 1;
        totalCoins = 0;
        Energy = 300;
        TotalGem = 0;
        CurrentChapter = 1;
        CurrentSteps = 1;
    }
}
