using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClothingItem : MonoBehaviour
{
    [Title("Prefab References")]
    [SerializeField] ClothingItemData _data;
    [SerializeField] Animator _clothesAnimator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    public ClothingItemData Data => _data;

    private void Start()
    {
        _spriteRenderer.sprite = _data.itemIcon;
    }
}
