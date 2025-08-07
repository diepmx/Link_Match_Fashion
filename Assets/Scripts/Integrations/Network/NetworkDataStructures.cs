using UnityEngine;
using System;

namespace JuiceFresh.Scripts.Integrations
{
    [Serializable]
    public class LeadboardPlayerData
    {
        public string Name;
        public string userID;
        public int position;
        public int score;
        public Sprite picture;
        public FriendData friendData;
    }

    [Serializable]
    public class FriendData
    {
        public string FacebookID;
        public string Name;
        public string userID;
        public int level;
        public Sprite picture;
    }
}