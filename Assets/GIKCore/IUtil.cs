using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IUtil
{
    #region GetAsset
    public static GameObject LoadPrefabResources(string assetName, string path = "Default")
    {
        if (path.Equals("Default"))
            path = "Popups/";
        string fullPath = path + assetName;
        GameObject prefab = Resources.Load<GameObject>(fullPath);
        if (prefab == null)
        {
            Debug.LogError($"Không tìm thấy prefab ở đường dẫn: Resources/{fullPath}");
            return null;
        }
        return prefab;
    }

    public static GameObject LoadPrefabWithParent(string path, string assetName, Transform parent, bool setAsLasSibling = true)
    {
        GameObject template = LoadPrefabResources(path + assetName);
        if (template != null)
        {
            GameObject go = Object.Instantiate(template, parent);
            if (setAsLasSibling) go.transform.SetAsLastSibling();
            return go;
        }
        return null;
    }

    #endregion


    #region saveFunc

    public static void SaveData(ScriptableObject data, string textName)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + textName + ".txt", json);
    }
    public static void LoadDataObject(ScriptableObject data, string textName)
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + textName + ".txt"))
        {
            var json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + textName + ".txt");
            JsonUtility.FromJsonOverwrite(json, data);
        }
        else
        {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + textName + ".txt", json);
        }
    }

    #endregion

}
