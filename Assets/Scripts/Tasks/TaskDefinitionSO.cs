using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(menuName = "Fashion/Task Definition", fileName = "TaskDefinition")]
public class TaskDefinitionSO : ScriptableObject
{
    [Header("Task Config")]
    public string taskId;
    public string title;
    public int coinCost = 10;
    public MatchSkins.ItemType slot; // Which slot this task equips

    [Header("Options (size = 3)")]
    public List<FashionItemSO> options = new List<FashionItemSO>();
    [Tooltip("Index [0..2] that requires Gems")] public int premiumIndex = 2;
}


