using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameButtonsPanel: BasePanel
{
    private UnityAction _onPauseGame;
    public event UnityAction OnPauseGame
    {
        add
        {
            _onPauseGame -= value;
            _onPauseGame += value;
        }
        remove => _onPauseGame -= value;
    }
    
    private Button _pauseButton;
    
    public GameButtonsPanel(VisualElement panel) : base(panel)
    {
        _pauseButton = _panel.Q<Button>("PauseButton");
        _pauseButton.clicked += PauseGameButtonClickHandler;
    }

    private void PauseGameButtonClickHandler()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _onPauseGame?.Invoke();
    }
}