using GIKCore.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class ActionRepeatTimer
{
    public float mInterval = 0.5f;
    //
    private float mCountTime = 0f;
    //-1: infinite
    public float mDuration = -1;
    //
    private bool mDone = false;
    //
    private ICallback.CallFunc mActionJob, mActionJobActionFinish;

    public ActionRepeatTimer(float interval, ICallback.CallFunc job)
    {
        this.mActionJob = job;
        this.mInterval = interval;
        this.mDuration = -1;
    }

    public ActionRepeatTimer(float interval, float duration, ICallback.CallFunc job,
        ICallback.CallFunc jobActionFinish = null)
    {
        this.mActionJob = job;
        this.mInterval = interval;
        this.mDuration = duration;
        this.mActionJobActionFinish = jobActionFinish;
    }

    public void ResetCounter()
    {
        mCountTime = 0f;
        mTotalTime = 0;
        mDone = false;
    }

    public void Stop()
    {
        mDone = true;
    }

    private float mTotalTime = 0;

    public void UpdateTimer(float deltaTime)
    {
        if (mDone)
        {
            return;
        }
        this.mCountTime += deltaTime;
        this.mTotalTime += deltaTime;
        //
        if (this.mCountTime > this.mInterval)
        {
            this.mCountTime = 0;
            if (mActionJob != null)
                mActionJob();
        }
        if (this.mDuration > 0 && this.mTotalTime > this.mDuration)
        {
            if (mActionJobActionFinish != null)
                mActionJobActionFinish();
            mDone = true;
        }
    }
}

public class ActionOnceTimer
{

    public float mDelay = 0.5f;
    //
    private float mCountTime = 0f;
    //
    private ICallback.CallFunc mActionJob;
    //
    private bool mDone = false;

    public ActionOnceTimer(float delay, ICallback.CallFunc job)
    {
        this.mActionJob = job;
        this.mDelay = delay;
    }

    public void Reset()
    {
        mCountTime = 0;
        mDone = false;
    }

    public void Reset(float newDelay)
    {
        mCountTime = 0;
        mDone = false;
        mDelay = newDelay;
    }


    public void Stop()
    {
        mDone = true;
    }

    public bool IsDone()
    {
        return mDone;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (mDone)
        {
            return;
        }
        //
        this.mCountTime += deltaTime;
        if (this.mCountTime > this.mDelay)
        {
            this.mCountTime = 0;
            this.mDone = true;
            //
            if (mActionJob != null)
            {
                //Debug.Log("-------- Time out, do the job");
                mActionJob();
            }
        }
    }
}
