using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] ItemListSO _playerInventory;

    private void OnEnable()
    {
        ItemSlot.OnBuyItem += AddToInventory;
        ItemSlot.OnSellItem += RemoveFromInventory;
    }

    private void OnDisable()
    {
        ItemSlot.OnBuyItem -= AddToInventory;
        ItemSlot.OnSellItem -= RemoveFromInventory;
    }

    void AddToInventory(ClothingItemData itemData)
    {
        _playerInventory.AddItem(itemData);
    }

    void RemoveFromInventory(ClothingItemData itemData)
    {
        _playerInventory.RemoveItem(itemData);
    }
}
