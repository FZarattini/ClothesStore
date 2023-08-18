using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Title("References")]
    [SerializeField, HideInInspector] PlayerController player;
    [SerializeField] ClothingItemData _data;
    [SerializeField] Image _itemIcon;
    [SerializeField] GameObject _buyButton;
    [SerializeField] GameObject _sellButton;
    [SerializeField] GameObject _equipButton;
    [SerializeField] TextMeshProUGUI _buyButtonPriceText;
    [SerializeField] TextMeshProUGUI _sellButtonPriceText;

    public static Action<ClothingItemData> OnBuyItem = null;
    public static Action<ClothingItemData> OnSellItem = null;
    public static Action<ClothingItemData> OnEquipItem = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        
    }
    public void SetItemSlot(ClothingItemData itemData, bool inStock)
    {
        _data = itemData;
        _itemIcon.sprite = itemData.itemIcon;

        SetButtons(false, inStock);

        if (inStock)
            _buyButtonPriceText.text = $"- {itemData.buyPrice}";
        else
            _sellButtonPriceText.text = $"+ {itemData.sellingPrice}";

    }

    public void SetInventoryItemSlot(ClothingItemData itemData)
    {
        _data = itemData;
        _itemIcon.sprite = itemData.itemIcon;

        SetButtons(true, false);
    }

    void SetButtons(bool isInventory, bool inStock)
    {
        if (isInventory)
        {
            _equipButton.gameObject.SetActive(true);
            _buyButton.SetActive(false);
            _sellButton.SetActive(false);
        }
        else
        {
            _buyButton.SetActive(inStock);
            _sellButton.SetActive(!inStock);
            _equipButton.gameObject.SetActive(false);
        }
    }

    public void TryToBuy()
    {
        if(player != null)
        {
            if(player.Currency >= _data.buyPrice)
            {
                player.DeductCurrency(_data.buyPrice);
                OnBuyItem?.Invoke(_data);
                Destroy(gameObject);
            }
        }
    }

    public void SellItem()
    {
        if(player != null)
        {
            player.AddCurrency(_data.sellingPrice);
            OnSellItem?.Invoke(_data);
            Destroy(gameObject);
        }
    }

    public void EquipItem()
    {
        OnEquipItem?.Invoke(_data);
    }
}
