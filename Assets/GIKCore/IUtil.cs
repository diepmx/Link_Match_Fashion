using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


    #region formatNumber

    public static string StringColor(string source, string colorHTMLformat)
    {
        if (string.IsNullOrEmpty(colorHTMLformat)) return source;
        return string.Format("<color={0}>{1}</color>", colorHTMLformat, source);
    }
    public static string StringSize(string source, int fontSize)
    {
        return string.Format("<size={0}>{1}</size>", fontSize, source);
    }

    public static string StringBoldface(string source)
    {
        return string.Format("<b>{0}</b>", source);
    }
    public static string StringItalics(string source)
    {
        return string.Format("<i>{0}</i>", source);
    }

    public static string StringToUpperFirstChar(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return input.First().ToString().ToUpper() + input.Substring(1);
    }
    public static string StringToUpperFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        if (input.EndsWith(' '))
            input = input.Trim(input[input.Length - 1]);
        string[] split = input.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            if (!string.IsNullOrEmpty(split[i]))
                split[i] = split[i].First().ToString().ToUpper() + split[i].Substring(1);
        }

        return string.Join(" ", split);
    }

    public static string FormatHourFromSec(long sec)
    {
        long hour = sec / 3600;
        long tmp = sec % 3600;
        long min = tmp / 60;
        long sec2 = tmp % 60;
        //
        long day = hour / 24;
        //
        if (day > 0)
        {
            hour = hour % 24;
            return day + "d " + FormatPrefixZero(hour) + "h " + FormatPrefixZero(min) + "m " + FormatPrefixZero(sec2) + "s";
        }
        else
        {
            return FormatPrefixZero(hour) + "h " + FormatPrefixZero(min) + "m " + FormatPrefixZero(sec2) + "s";
        }
    }

    private static string FormatPrefixZero(long value)
    {
        if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value.ToString();
        }
    }

    #endregion

}
