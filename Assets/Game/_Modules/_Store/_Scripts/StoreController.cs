using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class StoreController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] StoreStockSO _storeStock;
    [SerializeField] GameObject _itemSlotPrefab;
    [SerializeField] Transform _content;

    [SerializeField] UIContainer _buyStore;
    [SerializeField] UIContainer _sellStore;
    public void OpenBuyStore()
    {
        foreach (ClothingItem c in _storeStock.StoreItems)
        {
            var itemSlotObject = GameObject.Instantiate(_itemSlotPrefab, _content);
            var itemSlot = itemSlotObject.GetComponent<ItemSlot>();

            itemSlot.SetItemSlot(c.Data);
        }

        _buyStore.Show();
    }

    public void OpenSellStore()
    {
        _sellStore.Show();
    }
}
