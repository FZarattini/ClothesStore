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

    public static Action OnPlayerChoice = null;
    public static Action OnPlayerChoiceEnded = null;

    public static Action<Action> OnShopkeeperInteraction = null;
    
    public void Interact()
    {
        OnShopkeeperInteraction?.Invoke(OpenShopkeeperMenu);
    }

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

    public void OpenBuyStore()
    {
        _storeController.OpenBuyStore();
        CloseShopkeeperMenu();
    }

    public void OpenSellStore()
    {
        _storeController.OpenSellStore();
        CloseShopkeeperMenu();
    }
}
