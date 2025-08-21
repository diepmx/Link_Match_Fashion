using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChapterGameData", menuName = "Data/ChapterGameData")]
public class ChapterGameData : ScriptableObject
{
    public List<Chapter> chapters;
}
