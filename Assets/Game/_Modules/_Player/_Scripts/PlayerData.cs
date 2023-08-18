using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float PlayerSpeed;
    public int PlayerInitialCurrency;
}
