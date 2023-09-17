using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] TextMeshProUGUI _currencyValue;

    private void OnEnable()
    {
        PlayerController.OnCurrencyChanged += UpdateCurrencyValue;
    }

    private void OnDisable()
    {
        PlayerController.OnCurrencyChanged -= UpdateCurrencyValue;
    }

    // Displays the current currency value on the HUD
    void UpdateCurrencyValue(int value)
    {
        _currencyValue.text = $"x {value}";
    }
}
