using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreStock", menuName = "ScriptableObjects/Stock/New Store Stock", order = 1)]
public class StoreStockSO : ScriptableObject
{
    [SerializeField] List<ClothingItem> _storeItems;

    public List<ClothingItem> StoreItems => _storeItems;

    public void AddStockItem(ClothingItem clothes)
    {

    }

    public void RemoveStockItem(ClothingItem clothes)
    {

    }
}
