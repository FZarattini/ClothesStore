using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class EquipmentManager : MonoBehaviour
{
    [Title("References")]
    [SerializeField] Dictionary<ClothingItemData, GameObject> equipmentDictionary = new Dictionary<ClothingItemData, GameObject>();
    [SerializeField] ItemListSO _allItems;
    [SerializeField] ItemPrefabListSO _itemPrefabs;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] PlayerEquipmentController _playerEquipmentController;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] Transform _playerHatAnchor;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] Transform _playerBodyAnchor;

    public static Action<GameObject> OnHatEquipped = null;
    public static Action<GameObject> OnBodyEquipped = null;

    private void OnEnable()
    {
        ItemSlot.OnEquipItem += EquipItem;
    }

    private void OnDisable()
    {
        ItemSlot.OnEquipItem -= EquipItem;
    }

    private void Start()
    {
        InitializeDictionary();
        GetPlayerAnchors();
    }

    // Gets references to player equipment anchors
    void GetPlayerAnchors()
    {
        _playerEquipmentController = FindObjectOfType<PlayerEquipmentController>();

        _playerHatAnchor = _playerEquipmentController.HatAnchor;
        _playerBodyAnchor = _playerEquipmentController.BodyAnchor;
    }

    // Initialize the dictionary relating item scriptable objects to their prefabs
    void InitializeDictionary()
    {
        for(int i = 0; i < _allItems.ItemsList.Count; i++)
        {
            equipmentDictionary.Add(_allItems.ItemsList[i], _itemPrefabs.ItemPrefabList[i]);
        }
    }

    // Equips an item on the player
    public void EquipItem(ClothingItemData equipment)
    {
        ClearCurrentEquipment(equipment.clothesType);
        var equipmentPrefab = equipmentDictionary[equipment];

        if (equipmentPrefab == null) return;

        switch (equipment.clothesType)
        {
            case ClothesType.Hat:
                var hatObj = Instantiate(equipmentPrefab, _playerHatAnchor);

                OnHatEquipped?.Invoke(hatObj);
                
                break;
            case ClothesType.Body:

                var bodyObj = Instantiate(equipmentPrefab, _playerBodyAnchor);

                OnBodyEquipped?.Invoke(bodyObj);

                break;

            default:
                break;
        }
    }

    // Clears previously equiped item
    public void ClearCurrentEquipment(ClothesType clothesType)
    {
        switch (clothesType)
        {
            case ClothesType.Hat:
                if(_playerHatAnchor.childCount != 0)
                    DestroyImmediate(_playerHatAnchor.GetChild(0).gameObject);
                break;
            case ClothesType.Body:
                if (_playerBodyAnchor.childCount != 0)
                    DestroyImmediate(_playerBodyAnchor.GetChild(0).gameObject);
                break;

            default:
                break;
        }
    }
}
