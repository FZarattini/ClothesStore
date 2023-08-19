using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] PlayerController _characterController;
    [SerializeField] PlayerInteractionHandler _interactionHandler;

    private PlayerInputs input = null;
    private Vector2 movementVector = Vector2.zero;

    public static Action OnNextDialog;

    private void Awake()
    {
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Interaction.performed += OnInteractionPerformed;
        input.Player.NextDialog.performed += OnNextDialoguePerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Interaction.performed -= OnInteractionPerformed;
        input.Player.NextDialog.performed -= OnNextDialoguePerformed;
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.CanMoveOrInteract())
            _characterController.MoveCharacter(movementVector, _playerData.PlayerSpeed);
    }

    // On Interaction with envinronment
    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.CanMoveOrInteract()) return;

        _interactionHandler.Interact();
    }

    // On Player Movement
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    // On continue dialogue
    private void OnNextDialoguePerformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.OnDialogue) return;

        OnNextDialog?.Invoke();
    }

    // On stop movement
    private void OnMovementCancelled(InputAction.CallbackContext context)
    {
        movementVector = Vector2.zero;
    }
}
