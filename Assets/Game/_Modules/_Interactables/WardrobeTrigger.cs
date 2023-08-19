using Doozy.Runtime.UIManager.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeTrigger : MonoBehaviour, IInteractables
{
    public static Action OnWardrobeInteraction = null;

    // Executes interaction behaviour
    public void Interact()
    {
        OnWardrobeInteraction?.Invoke();
    }

}
