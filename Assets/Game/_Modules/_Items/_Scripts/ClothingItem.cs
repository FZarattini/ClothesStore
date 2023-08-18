using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothingItem : MonoBehaviour
{
    [SerializeField] ClothingItemData _data;

    public ClothingItemData Data => _data;
}
