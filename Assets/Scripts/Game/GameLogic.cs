using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLogic: Singleton<GameLogic>
{
    [Header("Quest")]
    [SerializeField] private QuestSystem _questSystem;
    [SerializeField] private DialogueSystem _dialogueSystem;
    
    [Header("Player")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private HealthPoint _playerHealth;
    
    [Header("NPC and OBJECTS")]
    [SerializeField] private EnemyController _witchController;
    [SerializeField] private WitchCauldronUsableZone _witchCauldron;

    [Header("Boss")] 
    [SerializeField] private EnemyController _boss;
    [SerializeField] private AudioSource _fightIntroAudioSource;
    [SerializeField] private int _fightBackgroundMusic = -1;
    
    [Header("Teleport")]
    [SerializeField] private Transform _witchZone;
    [SerializeField] private Transform _graveyardZone;
    [SerializeField] private Transform _swampZone;
    [SerializeField] private BaseQuest _deadGraveyard;
    [SerializeField] private BaseQuest _deadSwamp;
    [SerializeField] private BaseQuest _deadWitchHouse;
    
    public PlayerController PlayerController => _playerController;
    public EnemyController WitchController => _witchController;
    public WitchCauldronUsableZone WitchCauldron => _witchCauldron;
    public EnemyController PumpkinBoss => _boss;
    public Inventory PlayerInventory => _playerInventory;
    public HealthPoint PlayerHealthPoint => _playerHealth;
    public QuestSystem QuestSystem => _questSystem;
    public DialogueSystem DialogueSystem => _dialogueSystem;
    
    private UnityAction _onGameWin;
    private UnityAction _onGameLose;
    private UnityAction _onGameRestarted;
    
    public event UnityAction OnGameWin
    {
        add
        {
            _onGameWin -= value;
            _onGameWin += value;
        }
        remove => _onGameWin -= value;
    }
 
    public event UnityAction OnGameLose
    {
        add
        {
            _onGameLose -= value;
            _onGameLose += value;
        }
        remove => _onGameLose -= value;
    }

    public event UnityAction OnGameRestarted
    {
        add
        {
            _onGameRestarted -= value;
            _onGameRestarted += value;
        }
        remove => _onGameRestarted -= value;
    }
    
    private void Start()
    {
        if (_playerController)
        {
            _playerController.OnDead += LoseGame;
        }

        if (_questSystem)
        {
            _questSystem.OnAllQuestCompleted += WinGame;
        }
        
        Time.timeScale = 1;
    }
    
    public void StopPlayer()
    {
        _playerController.Stop();
    }

    public void StartPlayer()
    {
        GameUI.Instance.HideNoty();
        
        _playerController.Run();
    }

    private void WinGame()
    {
        StartCoroutine(WinDelayTimer());
    }
    
    private void LoseGame()
    {
        GameUI.Instance.StartDieBlocker();
        StartCoroutine(LoseDelayTimer());
    }

    public void PlayBossMusic(bool withIntro = true)
    {
        // Когда заметил босс
        if (_fightBackgroundMusic >= 0 && SoundManager.Instance.CurrentBackgroundSound != _fightBackgroundMusic && !SoundManager.Instance.IsWaitingIntro)
        {
            Debug.Log("SwitchBackgroundMusic - Boss");
                
            if (_fightIntroAudioSource != null && withIntro)
            {
                StartCoroutine(SoundManager.Instance.SwitchBackgroundWithIntro(_fightIntroAudioSource, _fightBackgroundMusic));
            }
            else
            {
                SoundManager.Instance.SwitchBackground(_fightBackgroundMusic);
            }
        }
    }
    
    public void TeleportToWitch(bool switchMusic = true)
    {
        PlayerController.transform.position = _witchZone.position;

        if (switchMusic)
        {
            SoundManager.Instance.SwitchBackground(0);
        }
    }
    
    public void TeleportToGraveYard()
    {
        PlayerController.transform.position = _graveyardZone.position;

        if (_boss.gameObject.activeSelf)
        {
            PlayBossMusic(false);
        }
        else
        {
            SoundManager.Instance.SwitchBackground(1);
        }
    }
    
    public void TeleportToSwamp()
    {
        PlayerController.transform.position = _swampZone.position;
        
        SoundManager.Instance.SwitchBackground(2);
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    private IEnumerator WinDelayTimer()
    {
        yield return null; //new WaitForSeconds(1f);
        
        // Sound
        SoundManager.Instance.PauseBackgroundMusic();
        SoundManager.Instance.PlaySound(SoundType.Win, gameObject);
        
        Time.timeScale = 0;
        
        _onGameWin?.Invoke();
    }
    
    private IEnumerator LoseDelayTimer()
    {
        yield return new WaitForSeconds(1f);
        
        if (QuestSystem.CurrentQuest.Id == QuestId.CollectPoisonBells || QuestSystem.CurrentQuest.Id == QuestId.ComebackWithBells)
        {
            QuestSystem.StartTempQuest(_deadSwamp);
            
            TeleportToWitch();
        } 
        else if (QuestSystem.CurrentQuest.Id == QuestId.CollectSkullAndTeeth || QuestSystem.CurrentQuest.Id == QuestId.ComebackWithSkullAndTeeth)
        {
            QuestSystem.StartTempQuest(_deadGraveyard);
            
            TeleportToWitch();
        }
        else if (QuestSystem.CurrentQuest.Id == QuestId.BossFight || QuestSystem.CurrentQuest.Id == QuestId.BossFightFinishCast)
        {
            QuestSystem.StartTempQuest(_deadWitchHouse);
            
            TeleportToWitch(false);
        }
    }
}