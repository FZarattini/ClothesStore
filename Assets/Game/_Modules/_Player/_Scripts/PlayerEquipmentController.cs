using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerEquipmentController : EquipmentController
{
    [Title("References")]
    [SerializeField, ReadOnly] GameObject _hatObj;
    [SerializeField, ReadOnly] GameObject _bodyObj;
    [SerializeField, ReadOnly] Animator _hatAnimator = null;
    [SerializeField, ReadOnly] Animator _bodyAnimator = null;

    private void OnEnable()
    {
        EquipmentManager.OnHatEquipped += SetHatAnimatorAndObject;
        EquipmentManager.OnBodyEquipped += SetBodyAnimatorAndObject;
        ItemSlot.OnSellItem += CheckSoldClothes;
    }

    private void OnDisable()
    {
        EquipmentManager.OnHatEquipped -= SetHatAnimatorAndObject;
        EquipmentManager.OnBodyEquipped -= SetBodyAnimatorAndObject;
        ItemSlot.OnSellItem -= CheckSoldClothes;
    }


    // Checks if Player is wearing the clothes that were sold
    void CheckSoldClothes(ClothingItemData itemData)
    {
        switch (itemData.clothesType)
        {
            case ClothesType.Body:

                if (_bodyObj != null)
                {
                    var bodyData = _bodyObj.GetComponent<ClothingItem>().Data;
                    if (itemData.id == bodyData.id)
                        RemoveBody();
                }

                break;
            case ClothesType.Hat:

                if(_hatObj != null)
                {
                    var hatData = _hatObj.GetComponent<ClothingItem>().Data;
                    if (itemData.id == hatData.id)
                        RemoveHat();
                }

                break;
            default:
                break;
        }
    }

    // Gets reference to Hat Equipment Animator
    void SetHatAnimatorAndObject(GameObject equipment)
    {
        _hatObj = equipment;
        var animator = equipment.GetComponent<Animator>();

        if (animator != null)
            _hatAnimator = animator;

        ForceIdleDownClothes();
    }

    // Gets reference to Body Equipment Animator
    void SetBodyAnimatorAndObject(GameObject equipment)
    {
        _bodyObj = equipment;
        var animator = equipment.GetComponent<Animator>();

        if (animator != null)
            _bodyAnimator = animator;

        ForceIdleDownClothes();
    }

    // Changes clothes animator state based on the player state
    public void ChangeClothesAnimatorState(PlayerAnimationStates animationState)
    {
        if(_hatAnimator != null)
            _hatAnimator.Play(animationState.ToString());
        if(_bodyAnimator != null)
            _bodyAnimator.Play(animationState.ToString());
    }

    // Forces Idle Down state on clothes animator
    public void ForceIdleDownClothes()
    {
        ChangeClothesAnimatorState(PlayerAnimationStates.IDLE_DOWN);
    }

    void RemoveHat()
    {
        _hatAnimator = null;
        DestroyImmediate(_hatObj);
    }

    void RemoveBody()
    {
        _bodyAnimator = null;
        DestroyImmediate(_bodyObj);
    }
}
