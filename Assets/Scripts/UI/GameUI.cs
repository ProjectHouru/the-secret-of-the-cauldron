using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class GameUI : Singleton<GameUI>
{
    private UIDocument _document;
    
    // Game Elements
    private HealthBarUI _healthBar;
    private QuestUI _questPanel;
    private BasePanel _notyPanel;
    
    // Pause
    private PausePanel _pausePanel;
    private GameButtonsPanel _gameButtonsPanel;
    private Slider _volumeSlider;
    
    // WinLose
    private WinLosePanel _winPanel;
    private WinLosePanel _losePanel;
    
    // Blocker
    private VisualElement _blockerPanel;
    
    private Button _pauseButton;
    private Button _continueButton;
    
    private string _lastNoty = "";
    private float _notyHideDelay = 0f;

    protected override void Awake()
    {
        base.Awake();
        
        _document = GetComponent<UIDocument>();
        
        // Game Elements
        _healthBar = new HealthBarUI(_document.rootVisualElement.Q<VisualElement>("HealthPanel"));
        _questPanel = new QuestUI(_document.rootVisualElement.Q<VisualElement>("QuestPanel"));
        _notyPanel = new BasePanel(_document.rootVisualElement.Q<VisualElement>("NotyPanel"));
        
        // Pause
        _pausePanel = new PausePanel(_document.rootVisualElement.Q<VisualElement>("PauseModal"));
        _gameButtonsPanel = new GameButtonsPanel(_document.rootVisualElement.Q<VisualElement>("RightButtons"));
        
        // WinLose
        _winPanel = new WinLosePanel(_document.rootVisualElement.Q<VisualElement>("WinModal"));
        _losePanel = new WinLosePanel(_document.rootVisualElement.Q<VisualElement>("LoseModal"));
        
        // Blocker
        _blockerPanel = _document.rootVisualElement.Q<VisualElement>("Blocker");
    }
    
    private void Start()
    {
        _healthBar.Display(GameLogic.Instance.PlayerHealthPoint.CurrentValue);
        _questPanel.Display(GameLogic.Instance.QuestSystem.CurrentQuest.Description);
        
        // Слайдер громкости
        _volumeSlider = _pausePanel.Panel.Q<Slider>("VolumeSlider");
        
        // Значение по умолчанию
        _volumeSlider.value = Mathf.Clamp(SoundManager.Instance.GetVolume() * 100f, 0, 100f);
        
        // Обработчик события изменения значения слайдера
        _volumeSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
        {
            SoundManager.Instance.SetVolume(Mathf.Clamp(evt.newValue / 100f, 0, 100f));
        });
    }

    private void OnEnable()
    {
        Subscribe();
    }
    
    private void OnDisable()
    {
        Unsubscribe();
    }

    public void ShowNoty(string customText, float hideAfter = 0)
    {
        if (_lastNoty != customText)
        {
            StopCoroutine(HideWithDelay());
        
            _notyPanel.Hide();

            _notyPanel.Panel.Q<Label>().text = customText;

            _notyPanel.Show();

            if (hideAfter > 0)
            {
                _notyHideDelay = hideAfter;
                StartCoroutine(HideWithDelay());
            }

            _lastNoty = customText;
        }
    }

    public void HideNoty()
    {
        _notyPanel.Hide();
        
        _lastNoty = "";
    }

    private IEnumerator HideWithDelay()
    {
        yield return new WaitForSeconds(_notyHideDelay);

        HideNoty();
    }

    private void Subscribe()
    {
        _pausePanel.OnContinueGame += ContinueGame;
        _pausePanel.OnExitToMenu += ExitToMenu;

        _gameButtonsPanel.OnPauseGame += PauseGame;

        GameLogic.Instance.PlayerHealthPoint.OnChangeValue += PlayerHealthUpdate;
        GameLogic.Instance.QuestSystem.OnUpdateQuest += QuestUpdate;

        _winPanel.OnRestartButton += RestartGame;
        _winPanel.OnExitToMenu += ExitToMenu;

        _losePanel.OnRestartButton += RestartGame;
        _losePanel.OnExitToMenu += ExitToMenu;

        GameLogic.Instance.OnGameWin += WinGame;
        //GameLogic.Instance.OnGameLose += LoseGame;
    }

    private void RestartGame()
    {
        LoadSceneBlocker(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerHealthUpdate(int value)
    {
        _healthBar.Display(value);
    }

    private void QuestUpdate(BaseQuest quest)
    {
        _questPanel.Display(quest.Description);

        StopCoroutine(QuestAnim());
        StartCoroutine(QuestAnim());
    }

    private IEnumerator QuestAnim()
    {
        var duration = 1f;
        var startMargin = 200f;
        var endMargin = 0;
        
        var elapsedTime = 0f;
        
        _questPanel.QuestVisualElement.transform.position = new Vector2(startMargin, 0);
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Используем формулу для easeOutElastic
            float easeValue = EaseOutElastic(t);
            float newMargin = Mathf.Lerp(startMargin, endMargin, easeValue);

            _questPanel.QuestVisualElement.transform.position = new Vector2(newMargin, 0);
            
            yield return null;
        }

        _questPanel.QuestVisualElement.transform.position = new Vector2(endMargin, 0);;
    }
    
    // Формула easeOutElastic для создания эластичного затухания
    private float EaseOutElastic(float t, float elasticity = 2f)
    {
        float c4 = (2 * Mathf.PI) / elasticity;

        return t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
    }
    
    private void Unsubscribe()
    {
        _pausePanel.OnContinueGame -= ContinueGame;
        _pausePanel.OnExitToMenu -= ExitToMenu;
        
        _gameButtonsPanel.OnPauseGame -= PauseGame;

        if (GameLogic.Instance)
        {
            GameLogic.Instance.OnGameWin -= WinGame;
            //GameLogic.Instance.OnGameLose -= LoseGame;
            
            if (GameLogic.Instance.PlayerHealthPoint)
            {
                GameLogic.Instance.PlayerHealthPoint.OnChangeValue -= PlayerHealthUpdate;
            }
            if (GameLogic.Instance.QuestSystem)
            {
                GameLogic.Instance.QuestSystem.OnUpdateQuest -= QuestUpdate;
            }
        }

        _winPanel.OnRestartButton -= RestartGame;
        _winPanel.OnExitToMenu -= ExitToMenu;
        
        _losePanel.OnRestartButton -= RestartGame;
        _losePanel.OnExitToMenu -= ExitToMenu;
    }

    private void PauseGame()
    {
        GameLogic.Instance.PauseGame();
    
        _pausePanel.Show();
        _gameButtonsPanel.Hide();
        
        //_pausePanel.SetText("");
    }
    
    private void WinGame()
    {
        _winPanel.Show();
        _gameButtonsPanel.Hide();
        
        //_winPanel.SetText("");
    }
    
    private void LoseGame()
    {
        _losePanel.Show();
        _gameButtonsPanel.Hide();
        
        //_losePanel.SetText("");
    }

    private void ContinueGame()
    {
        GameLogic.Instance.ContinueGame();
     
        _pausePanel.Hide();
        _gameButtonsPanel.Show();
    }

    private void ExitToMenu()
    {
        LoadSceneBlocker(0);
    }

    public void StartDieBlocker()
    {
        _blockerPanel.Q<Label>().text = "Умер?";
        _blockerPanel.style.opacity = 1.0f;
        
        _blockerPanel.RegisterCallback<TransitionEndEvent>(evt =>
        {
            _blockerPanel.style.opacity = 0;
        });
    }

    private void LoadSceneBlocker(int sceneIndex)
    {
        _blockerPanel.Q<Label>().text = "Загрузка...";
        _blockerPanel.style.opacity = 1.0f;
        
        _blockerPanel.RegisterCallback<TransitionEndEvent>(evt =>
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        });
    }
}
