using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour, IInteractables
{
    [SerializeField] DialogueSO _doorDialogue;

    public static Action<DialogueSO, Action> OnDoorInteraction;

    public void Interact()
    {
        OnDoorInteraction?.Invoke(_doorDialogue, null);
    }
}
