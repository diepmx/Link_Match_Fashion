using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(menuName = "Fashion/Fashion Item", fileName = "FashionItem")]
public class FashionItemSO : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string displayName;

    [Header("Spine")]
    public SkeletonDataAsset skeletonDataAsset;  // <- thêm trường này

    [Header("Equip")]
    public MatchSkins.ItemType slot;

    // nói cho [SpineSkin] biết dùng skeletonDataAsset ở trên
    [SpineSkin(dataField: nameof(skeletonDataAsset))]
    public string skinKey;

    [Header("Economy")]
    public int coinsPrice;
    public int gemsPrice;

    [Header("Visuals")]
    public Sprite icon;
}
