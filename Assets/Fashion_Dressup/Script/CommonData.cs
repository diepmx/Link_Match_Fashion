using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectionItem
{
    public string itemId;
    public string itemName;
    public Sprite icon;
    public bool isUnlocked;
}
[Serializable]
public class CollectionCategory
{
    public string categoryId;
    public string categoryName;
    public Sprite icon;
    public CollectionItem[] items;
}

public enum RewardBuffGame
{
    Coin,
    Gem,
    Energy,
    clotheshanger,
    XP
}

[Serializable]
public class RewardBuff
{
    public Sprite icon;
    public RewardBuffGame typeBuff;
    public int quantity;
}

[Serializable]
public class ChapterGame
{
    public string idChapter;
    public bool FinishStep = false;
    public string name;
    public Sprite icon;
    public int CoinBuy;
    public List<RewardBuff> items;
    public TypeChange Type = TypeChange.Clothes;
    public List<CharacterAnim> lstAmin;
    public Vector3 newPositionCamera;
}

[Serializable]
public class CharacterAnim
{
    public Sprite IconImage;
    public string AnimationName;
    public string InitialSkin;
}

[Serializable]
public class InforAmim
{
    public string PlayerInitialSkinName = "";
    public string AnimationName = "";
    public string BagInitialAnim = "";
    public string BagAnimationName = "";

    public void Reset()
    {
        PlayerInitialSkinName = "";
        AnimationName = "";
        BagInitialAnim = "";
        BagAnimationName = "";
    }
}

[Serializable]
public enum TypeChange
{
    Clothes,
    Bag,
    Makeup,
    lipstick
}

[Serializable]
public class Chapter
{
    public int id;
    public string Title;
    public Sprite Banner;
    public List<ChapterGame> lstSteps;
}