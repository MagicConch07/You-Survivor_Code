using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowEnum
{
    LevelUp
}

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<WindowEnum, IWindowPanel> panelDictionary;

    [SerializeField] private GameObject _death;
    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _settings;
    [SerializeField] private InputReader _inputReader;

    private bool _isGameOver = false;

    void OnEnable()
    {
        Time.timeScale = 1;
        _inputReader.OnSettingsEvent += HandleSettingsEvent;
        _death.SetActive(false);
        _clear.SetActive(false);
        _settings.SetActive(false);
    }

    void OnDisable()
    {
        _inputReader.OnSettingsEvent -= HandleSettingsEvent;
        _isGameOver = false;
    }

    private void HandleSettingsEvent(bool isRun)
    {
        if (_isGameOver) return;

        if (isRun)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _settings.SetActive(true);
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _settings.SetActive(false);
        }
    }

    public void DeadUI()
    {
        Time.timeScale = 0;
        _isGameOver = true;
        _death.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClaerUI()
    {
        Time.timeScale = 0;
        _isGameOver = true;
        _clear.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
