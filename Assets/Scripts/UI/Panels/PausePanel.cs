using UnityEngine.Events;
using UnityEngine.UIElements;

public class PausePanel: BasePanel
{
    private UnityAction _onContinueGame;
    public event UnityAction OnContinueGame
    {
        add
        {
            _onContinueGame -= value;
            _onContinueGame += value;
        }
        remove => _onContinueGame -= value;
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

    
    private Button _continueButton;
    private Label _pauseText;
    
    public PausePanel(VisualElement panel) : base(panel)
    {
        _continueButton = _panel.Q<Button>("ContinueButton");
        _continueButton.clicked += ContinueGameButtonClickHandler;
        
        _exitToMenuButton = _panel.Q<Button>("ExitToMenuButton");
        _exitToMenuButton.clicked += ExitToMenuButtonClickHandler;

        _pauseText = _panel.Q<VisualElement>("ModalContent").Q<Label>();
    }

    private void ContinueGameButtonClickHandler()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        
        _onContinueGame?.Invoke();
    }
    
    
    private void ExitToMenuButtonClickHandler()
    {
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
      
        _onExitToMenu?.Invoke();
    }

    public void SetText(string text)
    {
        _pauseText.text = text;
    }
}