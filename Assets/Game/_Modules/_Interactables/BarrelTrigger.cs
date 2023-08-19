using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, IInteractables
{
    [Title("References and Data")]
    [SerializeField] DialogueSO _barrelDialogue;
    [SerializeField] int currencyToAdd;
    [SerializeField] Collider2D _barrelCollider;

    public static Action<DialogueSO, Action> OnBarrelInteraction;

    public void Interact()
    {
        OnBarrelInteraction?.Invoke(_barrelDialogue, AddCurrency);
    }

    void AddCurrency()
    {
        var player = FindObjectOfType<PlayerController>();

        if(player != null)
        {
            player.AddCurrency(currencyToAdd);
            _barrelCollider.enabled = false;
        }
    }
}
