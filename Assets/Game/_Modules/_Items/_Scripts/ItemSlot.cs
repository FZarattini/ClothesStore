using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image _itemIcon;
    [SerializeField] TextMeshProUGUI _priceText;

    public void SetItemSlot(ClothingItemData itemData)
    {
        _itemIcon.sprite = itemData.itemIcon;
        _priceText.text = itemData.buyPrice.ToString();
    }
}
