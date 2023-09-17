using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemsList", menuName = "ScriptableObjects/Items/New Items List", order = 1)]
public class ItemListSO : ScriptableObject
{
    [SerializeField] List<ClothingItemData> _itemsList;

    [SerializeField, HideInInspector] ItemListSO _defaultStoreItems;

    public List<ClothingItemData> ItemsList
    {
        get => _itemsList;
        set => _itemsList = value;
    }

    // Adds an item to the list
    public void AddItem(ClothingItemData itemData)
    {
        _itemsList.Add(itemData);
    }

    // Removes an item from the list
    public void RemoveItem(ClothingItemData itemData)
    {
        _itemsList.Remove(itemData);
    }

    // Editor tool: Resets the Store stock to the default initial values
    [Button]
    public void SetInitialStoreStock()
    {
        _itemsList = new List<ClothingItemData>(_defaultStoreItems._itemsList);
    }

    // Editor tool: Resets the player inventory to the default initial values
    [Button]
    public void SetInitialPlayerInventory()
    {
        _itemsList.Clear();
    }
}
