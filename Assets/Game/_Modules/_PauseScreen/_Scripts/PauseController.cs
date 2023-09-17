using Doozy.Runtime.UIManager.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PauseController : MonoBehaviour
{
    [SerializeField] UIContainer _pauseContainer;
    [SerializeField, ReadOnly] bool _pauseScreenActive;

    public static Action OnGamePauseOpen;
    public static Action OnGamePauseClose;

    public static Action OnSaveGame;
    public static Action OnClearGameData;

    private void OnEnable()
    {
        PlayerInputHandler.OnPause += TogglePauseScreen;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnPause -= TogglePauseScreen;
    }

    public void TogglePauseScreen()
    {
        if (_pauseScreenActive)
        {
            OnGamePauseClose?.Invoke();
            _pauseScreenActive = false;
            _pauseContainer.Hide();
        }
        else
        {
            OnGamePauseOpen?.Invoke();
            _pauseScreenActive = true;
            _pauseContainer.Show();
        }
    }

    // Sends the event to save the game data
    public void SaveButton()
    {
        OnSaveGame?.Invoke();
    }

    // Sends the event to clear the game data
    public void ClearDataButton()
    {
        OnClearGameData?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
