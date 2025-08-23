using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class playerGirl : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic m_SkeletonPlayerGirl;

    public void Setdata(SkeletonDataAsset dataAsset, string initialSkinName, string AnimationName = "animation")
    {
        m_SkeletonPlayerGirl.skeletonDataAsset = dataAsset;
        m_SkeletonPlayerGirl.initialSkinName = initialSkinName;
        m_SkeletonPlayerGirl.Initialize(true);
        m_SkeletonPlayerGirl.AnimationState.SetAnimation(0, AnimationName, true);
    }
}
