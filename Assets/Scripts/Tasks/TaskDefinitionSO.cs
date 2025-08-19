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
    [Tooltip("Chi phí sao để hoàn thành nhiệm vụ (thay cho coin). Nếu > 0 sẽ dùng sao, bỏ qua coinCost.")]
    public int starsCost = 0;
    public MatchSkins.ItemType slot; // Which slot this task equips

    [Header("Options (size = 3)")]
    public List<FashionItemSO> options = new List<FashionItemSO>();
    [Tooltip("Index [0..2] that requires Gems")] public int premiumIndex = 2;

    [Header("Story / UX")]
    [Tooltip("Nếu nhiệm vụ chỉ có 1 vật phẩm, tự động trang bị sau khi hoàn thành.")]
    public bool autoUseIfSingle = true;
    [TextArea]
    public string narrativeText;
}


