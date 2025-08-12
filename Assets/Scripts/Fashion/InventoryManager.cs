using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity { 
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private List<FashionItemSO> catalog = new List<FashionItemSO>();
    [SerializeField] public MatchSkins matchSkins;

    private const string OwnedKey = "InventoryOwned";
    private HashSet<string> ownedItemIds = new HashSet<string>();
    private readonly Dictionary<MatchSkins.ItemType, string> equippedBySlot = new Dictionary<MatchSkins.ItemType, string>();

    public event Action OnInventoryChanged;
    public event Action<MatchSkins.ItemType, string> OnEquippedChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        LoadInventory();
    }

    public bool HasItem(string id) => ownedItemIds.Contains(id);

    public bool AddItem(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        if (ownedItemIds.Add(id))
        {
            SaveInventory();
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }

    public bool Purchase(FashionItemSO item)
    {
        if (item == null) return false;
        if (HasItem(item.id)) return true;

        if (item.coinsPrice > 0)
        {
            if (!InitScript.Instance.SpendCoins(item.coinsPrice)) return false;
        }
        else if (item.gemsPrice > 0)
        {
            if (InitScript.Gems < item.gemsPrice) return false;
            InitScript.Instance.SpendGems(item.gemsPrice);
        }

        return AddItem(item.id);
    }

    public bool Equip(FashionItemSO item)
    {
        if (item == null) return false;
        if (!HasItem(item.id)) return false;

        equippedBySlot[item.slot] = item.id;
        ApplyEquipToMatchSkins(item);
        OnEquippedChanged?.Invoke(item.slot, item.id);
        return true;
    }

    private void ApplyEquipToMatchSkins(FashionItemSO item)
    {
        if (matchSkins == null) return;
        matchSkins.Equip(item.skinKey, item.slot);
    }

    public FashionItemSO GetItemById(string id)
    {
        return catalog.Find(x => x.id == id);
    }

    public IEnumerable<string> GetOwnedItems() => ownedItemIds;

    private void SaveInventory()
    {
        var list = new List<string>(ownedItemIds);
        string json = JsonUtility.ToJson(new StringListWrapper { items = list });
        PlayerPrefs.SetString(OwnedKey, json);
        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        string json = PlayerPrefs.GetString(OwnedKey, string.Empty);
        if (string.IsNullOrEmpty(json)) return;
        try
        {
            var wrapper = JsonUtility.FromJson<StringListWrapper>(json);
            if (wrapper?.items != null)
            {
                ownedItemIds = new HashSet<string>(wrapper.items);
            }
        }
        catch { /* ignore */ }
    }

    [Serializable]
    private class StringListWrapper
    {
        public List<string> items;
    }
}
}


