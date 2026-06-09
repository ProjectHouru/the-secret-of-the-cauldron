using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _menuPanel;
    private VisualElement _authorsPanel;
    private VisualElement _blockerPanel;
    private Slider _volumeSlider;
    private Button _startButton;
    private Button _authorsButton;
    private Button _exitButton;
    private Button _backToMenuButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _menuPanel = _document.rootVisualElement.Q<VisualElement>("MenuItemsContainer");
        _authorsPanel = _document.rootVisualElement.Q<VisualElement>("MenuPanel");
        _blockerPanel = _document.rootVisualElement.Q<VisualElement>("Blocker");

        _startButton = _menuPanel.Q<Button>("StartButton");
        _startButton.clicked += StartGame;
        
        _authorsButton = _menuPanel.Q<Button>("AuthorsButton");
        _authorsButton.clicked += OpenAuthorsPanel;

        _exitButton = _menuPanel.Q<Button>("ExitButton");
        _exitButton.clicked += ExitGame;

        _backToMenuButton = _authorsPanel.Q<Button>("BackToMenuButton");
        _backToMenuButton.clicked += BackToMenu;
    }

    private void Start()
    {
        // Слайдер громкости
        _volumeSlider = _menuPanel.Q<Slider>("VolumeSlider");
        
        // Значение по умолчанию
        _volumeSlider.value = Mathf.Clamp(SoundManager.Instance.GetVolume() * 100f, 0, 100f);
        
        // Обработчик события изменения значения слайдера
        _volumeSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
        {
            SoundManager.Instance.SetVolume(Mathf.Clamp(evt.newValue / 100f, 0, 100f));
        });
    }

    private void StartGame()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _blockerPanel.Q<Label>().visible = true;
        _blockerPanel.style.opacity = 1.0f;
        
        _blockerPanel.RegisterCallback<TransitionEndEvent>(evt =>
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        });
    }

    private void ExitGame()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        Application.Quit();
    }

    private void OpenAuthorsPanel()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _menuPanel.style.display = DisplayStyle.None;
        _authorsPanel.style.display = DisplayStyle.Flex;
    }

    private void BackToMenu()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _authorsPanel.style.display = DisplayStyle.None;
        _menuPanel.style.display = DisplayStyle.Flex;
    }
}

