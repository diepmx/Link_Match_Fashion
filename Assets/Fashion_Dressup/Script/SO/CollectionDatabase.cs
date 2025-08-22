using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CollectionDatabase", menuName = "Data/CollectionDatabase")]
public class CollectionDatabase : ScriptableObject
{
    public List<CollectionCategory> categories;
    [Button]
    public void ResetData()
    {
        foreach (CollectionCategory category in categories)
        {
            foreach(CollectionItem item in category.items)
            {
                item.isUnlocked = false;
            }
        }
    }
}
