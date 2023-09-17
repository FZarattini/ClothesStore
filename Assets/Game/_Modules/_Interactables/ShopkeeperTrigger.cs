using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperTrigger : MonoBehaviour, IInteractables
{
    [Title("References")]
    [SerializeField] UIContainer _shopkeeperMenu;
    [SerializeField] StoreController _storeController;
    [SerializeField] DialogueSO _notClothedShopDialogue;
    [SerializeField] DialogueSO _clothedShopDialogue;

    public static Action OnPlayerChoice = null;
    public static Action OnPlayerChoiceEnded = null;

    public static Action<DialogueSO, Action> OnShopkeeperInteraction = null;
    
    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player.PlayerClothed)
            OnShopkeeperInteraction?.Invoke(_clothedShopDialogue, OpenShopkeeperMenu);
        else
            OnShopkeeperInteraction?.Invoke(_notClothedShopDialogue, OpenShopkeeperMenu);

    }

    // Open shop choices menu (Buy / Sell / Leave)
    public void OpenShopkeeperMenu()
    {
        OnPlayerChoice?.Invoke();
        _shopkeeperMenu.Show();
    }

    public void CloseShopkeeperMenu()
    {
        OnPlayerChoiceEnded?.Invoke();
        _shopkeeperMenu.Hide();
    }

    // Opens the UI for buying items on the store
    public void OpenBuyStore()
    {
        _storeController.OpenBuyStore();
        CloseShopkeeperMenu();
    }

    // Opens the UI for selling items on the store
    public void OpenSellStore()
    {
        _storeController.OpenSellStore();
        CloseShopkeeperMenu();
    }
}
