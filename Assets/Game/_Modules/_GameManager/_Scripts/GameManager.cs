using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Title("GameStates")]
    [SerializeField, ReadOnly] bool onDialogue = false;
    [SerializeField, ReadOnly] bool onPlayerChoice = false;

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

    public bool CanMoveOrInteract()
    {
        return !onDialogue && !onPlayerChoice;
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += delegate { onDialogue = true; };
        DialogueManager.OnDialogueFinish += delegate { onDialogue = false; };
        ShopkeeperTrigger.OnPlayerChoice += delegate { onPlayerChoice = true; };
        ShopkeeperTrigger.OnPlayerChoiceEnded += delegate { onPlayerChoice = false; };
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= delegate { onDialogue = true; };
        DialogueManager.OnDialogueFinish -= delegate { onDialogue = false; };
        ShopkeeperTrigger.OnPlayerChoice -= delegate { onPlayerChoice = true; };
        ShopkeeperTrigger.OnPlayerChoiceEnded -= delegate { onPlayerChoice = false; };
    }
}
