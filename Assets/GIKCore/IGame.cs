using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGame
{
    private static IGame instance;

    public static IGame main
    {
        get
        {
            if (instance == null) instance = new IGame();
            return instance;
        }
    }
    public List<IGameListener> listeners = new List<IGameListener>();
    public NetBehavior netBehavior { get; set; }
    public ICanvas canvas { get; set; }

}
