using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListener : MonoBehaviour, IGameListener, INetEventHandler
{
    public void AddOnAwake()
    {
        throw new System.NotImplementedException();
    }

    public void CheckActive()
    {
        //return Ready();
    }

    public bool ProcessNetData(NetData arg)
    {
        switch (arg.id)
        {
            case NetData.CANVAS_ADJUST:
                {
                    break;
                }
        }
        return false;
    }

    public bool Ready()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveOnDestroy()
    {
        throw new System.NotImplementedException();
    }
}
