using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDataManager : Singleton<CenterDataManager>
{
    [SerializeField] private GameData m_Gamedata;
    [SerializeField] private ChapterGameData m_ChapterGamedata;
    public GameData GameData => m_Gamedata;
    public ChapterGameData ChapterGameData => m_ChapterGamedata;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SaveGameData()
    {
        IUtil.SaveData(m_Gamedata, "GameData");
    }

    [Button]
    public void SaveData()
    {
        IUtil.SaveData(m_Gamedata, "GameData");
        IUtil.SaveData(m_ChapterGamedata, "ChapterGameData");
    }

    [Button]
    public void ResetData()
    {
        m_Gamedata.resetData();
        SaveData();
    }
}
