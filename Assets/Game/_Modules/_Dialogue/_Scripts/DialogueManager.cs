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

    [Title("Dialogues")]
    [SerializeField] DialogueSO _shopDialogue;
    [SerializeField] DialogueSO _leaveShopDialogue;
    [SerializeField] DialogueSO _changeClothesDialogue;

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
        ShopkeeperTrigger.OnShopkeeperInteraction += StartShopDialogueRoutine;
        PlayerInputHandler.OnNextDialog += CheckNextLine;
    }

    private void OnDisable()
    {
        ShopkeeperTrigger.OnShopkeeperInteraction -= StartShopDialogueRoutine;
        PlayerInputHandler.OnNextDialog -= CheckNextLine;
    }



    void StartShopDialogueRoutine(Action callback)
    {
        OnDialogueStart?.Invoke();

        _dialogueContainer.Show();

        _dialogueText.text = string.Empty;
        currentCallback = callback;
        currentDialogue = _shopDialogue;
        StartCoroutine(DialogueRoutine(callback));
    }

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

    void CheckNextLine()
    {
        if (!writingLine && !choicesEnabled)
            NextLine();
    }

    void NextLine()
    {
        if (dialogueIndex < _shopDialogue.dialogueText.Count - 1)
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

    void FinishDialogue()
    {
        StopAllCoroutines();
        _dialogueText.text = string.Empty;
        GameManager.Instance.OnDialogue = false;
        _dialogueContainer.Hide();

        OnDialogueFinish?.Invoke();
        currentCallback?.Invoke();
    }
}
