using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightPaintingTrigger : MonoBehaviour, IInteractables
{
    [SerializeField] DialogueSO _nightPaintingDialogue;

    public static Action<DialogueSO, Action> OnNightPaintingInteraction;

    public void Interact()
    {
        OnNightPaintingInteraction?.Invoke(_nightPaintingDialogue, null);
    }
}
