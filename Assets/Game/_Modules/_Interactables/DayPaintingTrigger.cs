using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayPaintingTrigger : MonoBehaviour, IInteractables
{
    [SerializeField] DialogueSO _dayPaintingDialogue;

    public static Action<DialogueSO, Action> OnDayPaintingInteraction;

    public void Interact()
    {
        OnDayPaintingInteraction?.Invoke(_dayPaintingDialogue, null);
    }
}
