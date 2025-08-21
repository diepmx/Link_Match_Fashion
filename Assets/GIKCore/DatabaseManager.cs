using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DatabaseManager
{
    public const string Database = "database";
    public static TextAsset LoadDatabase2(string assetName, string path = "Default")
    {
        if (path.Equals("Default"))
            path = "Database/";
        TextAsset ret = Resources.Load<TextAsset>(path + assetName);
        return ret;
    }
    public static string LoadDatabase(string assetName, string path = "Default")
    {
        TextAsset ret = LoadDatabase2(assetName, path);
        if (ret != null) return ret.text; // Dùng .text thay vì .ToString()
        return "";
    }
    public static void ParseLevelGame()
    {
        string aJSON = LoadDatabase("GamePosition");
        JSONArray ArrayLevel = JSON.Parse(aJSON).AsArray;
        try
        {
           
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}