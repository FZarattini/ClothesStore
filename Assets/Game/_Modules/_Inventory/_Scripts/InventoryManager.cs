using Doozy.Runtime.Common.Extensions;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Title("References")]
    [SerializeField] UIContainer _inventoryContainer;
    [SerializeField] ItemListSO _playerInventory;
    [SerializeField, ReadOnly] PlayerController _player;
    [SerializeField] GameObject _itemSlotPrefab;
    [SerializeField] Transform _content;
    [SerializeField] TextMeshProUGUI _emptyInventoryMessage;

    public static Action OnInventoryOpen = null;
    public static Action OnInventoryClose = null;

    private void OnEnable()
    {
        WardrobeTrigger.OnWardrobeInteraction += OpenInventory;
    }

    private void OnDisable()
    {
        WardrobeTrigger.OnWardrobeInteraction -= OpenInventory;
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void OpenInventory()
    {
        OnInventoryOpen?.Invoke();

        _emptyInventoryMessage.gameObject.SetActive(_playerInventory.ItemsList.Count == 0);

        InstantiateItemSlots();

        _inventoryContainer.Show();
    }

    public void CloseInventory()
    {
        OnInventoryClose?.Invoke();

        _inventoryContainer.Hide();
    }

    void InstantiateItemSlots()
    {
        ClearItemSlots();

        foreach (ClothingItemData c in _playerInventory.ItemsList)
        {
            var slotObj = Instantiate(_itemSlotPrefab, _content);
            var itemSlot = slotObj.GetComponent<ItemSlot>();

            itemSlot.SetInventoryItemSlot(c);

        }
    }

    void ClearItemSlots()
    {
        while (_content.childCount > 0)
        {
            DestroyImmediate(_content.GetChild(0).gameObject);
        }
    }
}
