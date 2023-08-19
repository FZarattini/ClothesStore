using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Title("References")]
    [SerializeField] PlayerData _playerData = null;

    [Title("GameStates")]
    [SerializeField, ReadOnly] bool onDialogue = false;
    [SerializeField, ReadOnly] bool onPlayerChoice = false;
    [SerializeField, ReadOnly] bool onStore = false;
    [SerializeField, ReadOnly] bool onInventory = false;

    [Title("Global Values")]

    [SerializeField] int playerCurrency;

    public bool OnDialogue
    {
        get => onDialogue;
        set => onDialogue = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += delegate { onDialogue = true; };
        DialogueManager.OnDialogueFinish += delegate { onDialogue = false; };
        ShopkeeperTrigger.OnPlayerChoice += delegate { onPlayerChoice = true; };
        ShopkeeperTrigger.OnPlayerChoiceEnded += delegate { onPlayerChoice = false; };
        LeaveShopTrigger.OnPlayerChoiceStart += delegate { onPlayerChoice = true; };
        LeaveShopTrigger.OnPlayerChoiceEnd += delegate { onPlayerChoice = false; };
        StoreController.OnStoreOpen += delegate { onStore = true; };
        StoreController.OnStoreClose += delegate { onStore = false; };
        InventoryManager.OnInventoryOpen += delegate { onInventory = true; };
        InventoryManager.OnInventoryClose += delegate { onInventory = false; };
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= delegate { onDialogue = true; };
        DialogueManager.OnDialogueFinish -= delegate { onDialogue = false; };
        ShopkeeperTrigger.OnPlayerChoice -= delegate { onPlayerChoice = true; };
        ShopkeeperTrigger.OnPlayerChoiceEnded -= delegate { onPlayerChoice = false; };
        LeaveShopTrigger.OnPlayerChoiceStart -= delegate { onPlayerChoice = true; };
        LeaveShopTrigger.OnPlayerChoiceEnd -= delegate { onPlayerChoice = false; };
        StoreController.OnStoreOpen -= delegate { onStore = true; };
        StoreController.OnStoreClose -= delegate { onStore = false; };
        InventoryManager.OnInventoryOpen -= delegate { onInventory = true; };
        InventoryManager.OnInventoryClose -= delegate { onInventory = false; };
    }

    public bool CanMoveOrInteract()
    {
        return !onDialogue && !onPlayerChoice && !onStore && !onInventory;
    }

}
