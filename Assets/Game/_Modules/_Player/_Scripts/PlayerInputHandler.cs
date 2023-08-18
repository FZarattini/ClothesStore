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
        input.Player.NextDialog.performed += OnNextDialogPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Interaction.performed -= OnInteractionPerformed;
        input.Player.NextDialog.performed -= OnNextDialogPerformed;
    }

    private void FixedUpdate()
    {
        _characterController.MoveCharacter(movementVector, _playerData.PlayerSpeed);
    }

    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.CanMoveOrInteract()) return;

        _interactionHandler.Interact();
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.CanMoveOrInteract()) return;

        movementVector = context.ReadValue<Vector2>();
    }

    private void OnNextDialogPerformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.OnDialogue) return;

        OnNextDialog?.Invoke();
    }

    private void OnMovementCancelled(InputAction.CallbackContext context)
    {
        movementVector = Vector2.zero;
    }
}
