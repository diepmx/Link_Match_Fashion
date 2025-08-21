using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameListener
{
    bool Ready();
    bool ProcessNetData(NetData arg);
    void AddOnAwake();
    void RemoveOnDestroy();

}
