using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StoreController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] ItemListSO _storeStock;
    [SerializeField] ItemListSO _playerInventory;
    [SerializeField] GameObject _itemSlotPrefab;
    [SerializeField] Transform _buyContent;
    [SerializeField] Transform _sellContent;
    [SerializeField] ScrollRect _buyScrollView;
    [SerializeField] ScrollRect _sellScrollView;

    [SerializeField] TextMeshProUGUI _emptyStockMessage;
    [SerializeField] TextMeshProUGUI _emptyInventoryMessage;

    [SerializeField] UIContainer _storeContainer;


    public static Action OnStoreOpen = null;
    public static Action OnStoreClose = null;

    private void OnEnable()
    {
        ItemSlot.OnBuyItem += RemoveStockItem;
        ItemSlot.OnSellItem += AddStockItem;
    }

    private void OnDisable()
    {
        ItemSlot.OnBuyItem -= RemoveStockItem;
        ItemSlot.OnSellItem -= AddStockItem;
    }

    private void Start()
    {
        // Reset store's stock
        if(!SaveManager.Instance.SavedStoreStock)
            _storeStock.SetInitialStoreStock(); 
    }

    public void OpenBuyStore()
    {
        OnStoreOpen?.Invoke();
        _buyScrollView.gameObject.SetActive(true);
        _sellScrollView.gameObject.SetActive(false);

        CheckEmptyMessage(true);

        ClearItemSlots(_buyContent);

        foreach (ClothingItemData c in _storeStock.ItemsList)
        {
            InstantiateItem(c, true);
        }

        _storeContainer.Show();
    }

    public void OpenSellStore()
    {
        OnStoreOpen?.Invoke();
        _buyScrollView.gameObject.SetActive(false);
        _sellScrollView.gameObject.SetActive(true);

        CheckEmptyMessage(false);

        ClearItemSlots(_sellContent);

        foreach (ClothingItemData c in _playerInventory.ItemsList)
        {
            InstantiateItem(c, false);
        }

        _storeContainer.Show();
    }

    public void CloseStore()
    {
        OnStoreClose?.Invoke();
        _storeContainer.Hide();
    }

    // Called whenever player sells an item
    void AddStockItem(ClothingItemData itemData)
    {
        _storeStock.AddItem(itemData);
        CheckEmptyMessage(false);
    }

    // Called whenever player buys an item
    void RemoveStockItem(ClothingItemData itemData)
    {
        _storeStock.RemoveItem(itemData);
        CheckEmptyMessage(true);
    }

    void InstantiateItem(ClothingItemData data, bool inStock)
    {
        var content = inStock ? _buyContent : _sellContent;

        var itemSlotObject = Instantiate(_itemSlotPrefab, content);
        var itemSlot = itemSlotObject.GetComponent<ItemSlot>();

        itemSlot.SetItemSlot(data, inStock);
    }

    // Checks if should display empty items lists messages
    void CheckEmptyMessage(bool isBuyShop)
    {
        if (isBuyShop)
        {
            _emptyInventoryMessage.gameObject.SetActive(false);
            _emptyStockMessage.gameObject.SetActive(_storeStock.ItemsList.Count == 0);
        }
        else
        {
            _emptyStockMessage.gameObject.SetActive(false);
            _emptyInventoryMessage.gameObject.SetActive(_playerInventory.ItemsList.Count == 0);
        }
    }


    // Destroys every Item slot
    void ClearItemSlots(Transform content)
    {
        while (content.childCount > 0)
        {
            DestroyImmediate(content.GetChild(0).gameObject);
        }
    }
}
