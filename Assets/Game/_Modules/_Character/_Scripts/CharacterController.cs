using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Title("Object References")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] Vector2 facingDirection;

    [SerializeField] AnimationStates defaultState;
    [SerializeField, ReadOnly] AnimationStates currentState;


    public Vector2 FacingDirection => facingDirection;


    private enum AnimationStates
    {
        IDLE_UP,
        IDLE_HORIZONTAL,
        IDLE_DOWN,
        WALK_UP,
        WALK_HORIZONTAL,
        WALK_DOWN,
    }

    private void Start()
    {
        ChangeAnimatorState(defaultState);
    }

    // Moves the character based on the movement vector and speed
    public void MoveCharacter(Vector2 moveVector, float speed)
    {
        _rigidBody.velocity = moveVector * speed;

        if (_rigidBody.velocity == Vector2.zero || GameManager.Instance.OnDialogue)
        {
            if(!IsIdleState())
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
            case AnimationStates.IDLE_HORIZONTAL:
            case AnimationStates.IDLE_DOWN:
            case AnimationStates.IDLE_UP:
                    return;

            case AnimationStates.WALK_HORIZONTAL:
                ChangeAnimatorState(AnimationStates.IDLE_HORIZONTAL);
                break;

            case AnimationStates.WALK_UP:
                ChangeAnimatorState(AnimationStates.IDLE_UP);
                break;

            case AnimationStates.WALK_DOWN:
                ChangeAnimatorState(AnimationStates.IDLE_DOWN);
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
            case AnimationStates.WALK_HORIZONTAL:

                if(_rigidBody.velocity.x == 0 && _rigidBody.velocity.y != 0)
                {
                    if (_rigidBody.velocity.y > 0)
                        ChangeAnimatorState(AnimationStates.WALK_UP);
                    else
                        ChangeAnimatorState(AnimationStates.WALK_DOWN);
                }

                break;

            case AnimationStates.WALK_UP:

                if (_rigidBody.velocity.y == 0 && _rigidBody.velocity.x != 0)
                {
                    ChangeAnimatorState(AnimationStates.WALK_HORIZONTAL);
                }else if(_rigidBody.velocity.y < 0)
                {
                    ChangeAnimatorState(AnimationStates.WALK_DOWN);
                }

                break;

            case AnimationStates.WALK_DOWN:

                if(_rigidBody.velocity.y == 0 && _rigidBody.velocity.x != 0)
                {
                    ChangeAnimatorState(AnimationStates.WALK_HORIZONTAL);
                }
                else if (_rigidBody.velocity.y > 0)
                {
                    ChangeAnimatorState(AnimationStates.WALK_UP);
                }

                break;

            case AnimationStates.IDLE_HORIZONTAL:
            case AnimationStates.IDLE_DOWN:
            case AnimationStates.IDLE_UP:

                ChangeAnimatorState(AnimationStates.WALK_HORIZONTAL);

                break;

            default:
                break;
        }
    }


    // Changes the character animation based on the new state
    void ChangeAnimatorState(AnimationStates newState)
    {
        if (currentState == newState) return;

        _animator.Play(newState.ToString());

        currentState = newState;
    }
    
    // Flips the sprite of the Character using the absolute values of the scale instead of 1f in case scale values need to be changed in the future
    void SetDirection()
    {
        if (_rigidBody.velocity.x < 0)
            _spriteRenderer.transform.localScale = new Vector3(- Mathf.Abs(_spriteRenderer.transform.localScale.x), _spriteRenderer.transform.localScale.y, _spriteRenderer.transform.localScale.z);
        else
            _spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(_spriteRenderer.transform.localScale.x), _spriteRenderer.transform.localScale.y, _spriteRenderer.transform.localScale.z);
    }

    // Checks is player is in any of the Idle States
    bool IsIdleState()
    {
        return currentState == AnimationStates.IDLE_HORIZONTAL || currentState == AnimationStates.IDLE_UP || currentState == AnimationStates.IDLE_DOWN;
    }
}
