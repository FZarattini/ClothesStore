using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Doozy.Runtime.UIManager.Containers;

public class LeaveShopTrigger : MonoBehaviour
{
    [Title("References")]
    [SerializeField] UIContainer _menuContainer;
    [SerializeField] DialogueSO leaveShopDialogue;

    public static Action<DialogueSO, Action> OnLeaveShopTriggered;
    public static Action OnPlayerChoiceStart;
    public static Action OnPlayerChoiceEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
            OnLeaveShopTriggered?.Invoke(leaveShopDialogue, OpenLeaveShopMenu);
    }

    // Open the choices menu
    void OpenLeaveShopMenu()
    {
        OnPlayerChoiceStart?.Invoke();
        _menuContainer.Show();
    }

    // Closes the choices menu
    public void CloseLeaveShopMenu()
    {
        OnPlayerChoiceEnd?.Invoke();
        _menuContainer.Hide();
    }

    // Closes the game
    public void LeaveGameOption()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        CloseLeaveShopMenu();
    }
}
