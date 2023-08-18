using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClothingItemData", menuName = "ScriptableObjects/Clothes/New Clothing Item Data", order = 1)]
public class ClothingItemData : ScriptableObject
{
    public ClothesType clothesType;
    public Sprite itemIcon;
    public int buyPrice;
    public int sellingPrice;
}

public enum ClothesType
{
    Hat,
    Body,
}
