using UnityEngine.Events;
using UnityEngine.UIElements;

public class WinLosePanel: BasePanel
{
    private UnityAction _onRestartButton;
    public event UnityAction OnRestartButton
    {
        add
        {
            _onRestartButton -= value;
            _onRestartButton += value;
        }
        remove => _onRestartButton -= value;
    }
    
    private Button _exitToMenuButton;
   
    private UnityAction _onExitToMenu;
    public event UnityAction OnExitToMenu
    {
        add
        {
            _onExitToMenu -= value;
            _onExitToMenu += value;
        }
        remove => _onExitToMenu -= value;
    }

    
    private Button _restartButton;
    private Label _text;
    
    public WinLosePanel(VisualElement panel) : base(panel)
    {
        _restartButton = _panel.Q<Button>("RestartButton");
        _restartButton.clicked += ContinueGameButtonClickHandler;
        
        _exitToMenuButton = _panel.Q<Button>("ExitToMenuButton");
        _exitToMenuButton.clicked += ExitToMenuButtonClickHandler;

        _text = _panel.Q<VisualElement>("ModalContent").Q<Label>();
    }

    private void ContinueGameButtonClickHandler()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _onRestartButton?.Invoke();
    }
    
    
    private void ExitToMenuButtonClickHandler()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
      
        _onExitToMenu?.Invoke();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}