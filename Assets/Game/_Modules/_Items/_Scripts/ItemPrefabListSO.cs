using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Prefab List", menuName = "ScriptableObjects/Items/New Item Prefab List", order = 1)]
public class ItemPrefabListSO : ScriptableObject
{
    [SerializeField] List<GameObject> itemPrefabList;

    public List<GameObject> ItemPrefabList => itemPrefabList;
}
