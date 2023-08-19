using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.PlayerLoop;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Title("References")]
    [SerializeField, Sirenix.OdinInspector.ReadOnly] DialogueSO currentDialogue;
    [SerializeField] UIContainer _dialogueContainer;
    [SerializeField] TextMeshProUGUI _dialogueText;

    [Title("Control")]
    [SerializeField, HideInInspector] int dialogueIndex = 0;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] bool writingLine;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] bool choicesEnabled;

    Action currentCallback = null;

    public static Action OnDialogueStart = null;
    public static Action OnDialogueFinish = null;

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
        ShopkeeperTrigger.OnShopkeeperInteraction += StartDialogueRoutine;
        LeaveShopTrigger.OnLeaveShopTriggered += StartDialogueRoutine;
        BarrelTrigger.OnBarrelInteraction += StartDialogueRoutine;
        DoorTrigger.OnDoorInteraction += StartDialogueRoutine;
        NightPaintingTrigger.OnNightPaintingInteraction += StartDialogueRoutine;
        DayPaintingTrigger.OnDayPaintingInteraction += StartDialogueRoutine;
        PlayerInputHandler.OnNextDialog += CheckNextLine;
    }

    private void OnDisable()
    {
        ShopkeeperTrigger.OnShopkeeperInteraction -= StartDialogueRoutine;
        LeaveShopTrigger.OnLeaveShopTriggered -= StartDialogueRoutine;
        BarrelTrigger.OnBarrelInteraction -= StartDialogueRoutine;
        DoorTrigger.OnDoorInteraction -= StartDialogueRoutine;
        NightPaintingTrigger.OnNightPaintingInteraction -= StartDialogueRoutine;
        DayPaintingTrigger.OnDayPaintingInteraction -= StartDialogueRoutine;
        PlayerInputHandler.OnNextDialog -= CheckNextLine;
    }


    // Starts a new dialogue, starting from the last line if dialogue has been completed before
    void StartDialogueRoutine(DialogueSO dialogue, Action callback)
    {
        OnDialogueStart?.Invoke();

        _dialogueContainer.Show();

        if (!dialogue.completed)
            dialogueIndex = 0;
        else
            dialogueIndex = dialogue.dialogueText.Count - 1;

        _dialogueText.text = string.Empty;
        currentCallback = callback;
        currentDialogue = dialogue;
        StartCoroutine(DialogueRoutine(callback));
    }

    // Writes the dialogues lines character by character
    IEnumerator DialogueRoutine(Action callback)
    {
        foreach (char c in currentDialogue.dialogueText[dialogueIndex].ToCharArray())
        {
            writingLine = true;
            _dialogueText.text += c;
            yield return new WaitForSeconds(currentDialogue.writeSpeed);
        }
        writingLine = false;

        yield return null;
    }

    // Checks if there is a next line to trigger
    void CheckNextLine()
    {
        if (!writingLine && !choicesEnabled)
            NextLine();
    }

    // Starts next line of dialogue
    void NextLine()
    {
        if (dialogueIndex < currentDialogue.dialogueText.Count - 1)
        {
            dialogueIndex++;
            _dialogueText.text = string.Empty;
            StartCoroutine(DialogueRoutine(currentCallback));
        }
        else
        {
            FinishDialogue();
        }
    }

    // Finishes the dialogue
    void FinishDialogue()
    {
        StopAllCoroutines();
        _dialogueText.text = string.Empty;
        GameManager.Instance.OnDialogue = false;
        currentDialogue.completed = true;
        currentDialogue = null;
        _dialogueContainer.Hide();

        OnDialogueFinish?.Invoke();
        currentCallback?.Invoke();
    }
}
