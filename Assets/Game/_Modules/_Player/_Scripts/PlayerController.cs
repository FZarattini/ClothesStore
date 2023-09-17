using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] PlayerData _playerData;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] PlayerEquipmentController _playerEquipmentController;

    [Title("Currency")]
    [SerializeField, ReadOnly] int currency;

    [SerializeField] PlayerAnimationStates defaultState;
    [SerializeField, ReadOnly] PlayerAnimationStates currentState;

    public static Action<int> OnCurrencyChanged;

    public int Currency => currency;

    public enum PlayerAnimationStates
    {
        IDLE_UP,
        IDLE_HORIZONTAL,
        IDLE_DOWN,
        WALK_UP,
        WALK_HORIZONTAL,
        WALK_DOWN,
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryOpen += ForceIdleDownState;
        StoreController.OnStoreOpen += ForceIdleUpState;
        SaveManager.OnGameLoaded += PlayerStart;
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryOpen -= ForceIdleDownState;
        StoreController.OnStoreOpen -= ForceIdleUpState;
        SaveManager.OnGameLoaded -= PlayerStart;
    }


    void PlayerStart()
    {
        if (!SaveManager.Instance.SavedCurrency)
            currency = _playerData.PlayerInitialCurrency;
        else
            currency = SaveManager.Instance.CurrentCurrency;

        OnCurrencyChanged?.Invoke(currency);

        ChangeAnimatorState(defaultState);
    }

    // Moves the character based on the movement vector and speed
    public void MoveCharacter(Vector2 moveVector, float speed)
    {
        _rigidBody.velocity = moveVector * speed;

        if (_rigidBody.velocity == Vector2.zero || GameManager.Instance.OnDialogue)
        {
            if (!IsIdleState())
                SetIdle();
        }
        else
        {
            HandleNewMovement();
            SetDirection();
        }

    }

    // Chooses which Idle animation should be played based on the players last movement
    void SetIdle()
    {
        switch (currentState)
        {
            case PlayerAnimationStates.IDLE_HORIZONTAL:
            case PlayerAnimationStates.IDLE_DOWN:
            case PlayerAnimationStates.IDLE_UP:
                return;

            case PlayerAnimationStates.WALK_HORIZONTAL:
                ChangeAnimatorState(PlayerAnimationStates.IDLE_HORIZONTAL);
                break;

            case PlayerAnimationStates.WALK_UP:
                ChangeAnimatorState(PlayerAnimationStates.IDLE_UP);
                break;

            case PlayerAnimationStates.WALK_DOWN:
                ChangeAnimatorState(PlayerAnimationStates.IDLE_DOWN);
                break;

            default:
                break;

        }
    }


    // Handles the change in movement deciding which animation should be called, prioritizing last movement animation over new movement
    void HandleNewMovement()
    {
        switch (currentState)
        {
            case PlayerAnimationStates.WALK_HORIZONTAL:

                if (_rigidBody.velocity.x == 0 && _rigidBody.velocity.y != 0)
                {
                    if (_rigidBody.velocity.y > 0)
                        ChangeAnimatorState(PlayerAnimationStates.WALK_UP);
                    else
                        ChangeAnimatorState(PlayerAnimationStates.WALK_DOWN);
                }

                break;

            case PlayerAnimationStates.WALK_UP:

                if (_rigidBody.velocity.y == 0 && _rigidBody.velocity.x != 0)
                {
                    ChangeAnimatorState(PlayerAnimationStates.WALK_HORIZONTAL);
                }
                else if (_rigidBody.velocity.y < 0)
                {
                    ChangeAnimatorState(PlayerAnimationStates.WALK_DOWN);
                }

                break;

            case PlayerAnimationStates.WALK_DOWN:

                if (_rigidBody.velocity.y == 0 && _rigidBody.velocity.x != 0)
                {
                    ChangeAnimatorState(PlayerAnimationStates.WALK_HORIZONTAL);
                }
                else if (_rigidBody.velocity.y > 0)
                {
                    ChangeAnimatorState(PlayerAnimationStates.WALK_UP);
                }

                break;

            case PlayerAnimationStates.IDLE_HORIZONTAL:
            case PlayerAnimationStates.IDLE_DOWN:
            case PlayerAnimationStates.IDLE_UP:

                ChangeAnimatorState(PlayerAnimationStates.WALK_HORIZONTAL);

                break;

            default:
                break;
        }

    }


    // Changes the character animation based on the new state
    void ChangeAnimatorState(PlayerAnimationStates newState)
    {
        if (currentState == newState) return;

        _animator.Play(newState.ToString());
        _playerEquipmentController.ChangeClothesAnimatorState(newState);

        currentState = newState;

    }

    // Flips the sprite of the Character using the absolute values of the scale instead of 1f in case scale values need to be changed in the future
    void SetDirection()
    {
        if (_rigidBody.velocity.x < 0)
            _spriteRenderer.transform.localScale = new Vector3(-Mathf.Abs(_spriteRenderer.transform.localScale.x), _spriteRenderer.transform.localScale.y, _spriteRenderer.transform.localScale.z);
        else
            _spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(_spriteRenderer.transform.localScale.x), _spriteRenderer.transform.localScale.y, _spriteRenderer.transform.localScale.z);
    }

    // Checks is player is in any of the Idle States
    bool IsIdleState()
    {
        return currentState == PlayerAnimationStates.IDLE_HORIZONTAL || currentState == PlayerAnimationStates.IDLE_UP || currentState == PlayerAnimationStates.IDLE_DOWN;
    }

    // Spends player money
    public void DeductCurrency(int value)
    {
        currency -= value;
        OnCurrencyChanged?.Invoke(currency);
    }

    // Add player money
    public void AddCurrency(int value)
    {
        currency += value;
        OnCurrencyChanged?.Invoke(currency);
    }

    // Forces player to look down to the camera
    void ForceIdleDownState()
    {
        _rigidBody.velocity = Vector2.zero;
        ChangeAnimatorState(PlayerAnimationStates.IDLE_DOWN);
    }

    // Forces player to look up with the back to the camera
    void ForceIdleUpState()
    {
        _rigidBody.velocity = Vector2.zero;
        ChangeAnimatorState(PlayerAnimationStates.IDLE_UP);
    }
}
