using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestSystem: MonoBehaviour
{
    [SerializeField] private List<BaseQuest> _quests;
    [SerializeField] private int _currentQuestIndex = 0;
    
    private BaseQuest _currentQuest;
    
    private event UnityAction<BaseQuest> _onStartQuest;
    public event UnityAction<BaseQuest> OnStartQuest
    {
        add { _onStartQuest -= value; _onStartQuest += value; }
        remove => _onStartQuest -= value;
    }
    
    private event UnityAction<BaseQuest> _onUpdateQuest;
    public event UnityAction<BaseQuest> OnUpdateQuest
    {
        add { _onUpdateQuest -= value; _onUpdateQuest += value; }
        remove => _onUpdateQuest -= value;
    }
    
    private event UnityAction _onAllQuestCompleted;
    public event UnityAction OnAllQuestCompleted
    {
        add { _onAllQuestCompleted -= value; _onAllQuestCompleted += value; }
        remove => _onAllQuestCompleted -= value;
    }

    public void SetCurrentQuest(QuestId questId)
    {
        for (var i = 0; i < _quests.Count; i++)
        {
            if (_quests[i].Id == questId)
            {
                _currentQuestIndex = i;
            }

            // Обнуляем все квесты включая заданный
            if (i > _currentQuestIndex)
            {
                _quests[i].Init();
            }
        }
    }
    
    public BaseQuest CurrentQuest => _currentQuest;

    public void StartTempQuest(BaseQuest quest)
    {
        Debug.Log("QuestSystem # Начат времянный квест: " + quest.Id);
        
        _currentQuest.PauseQuest();
        
        quest.Init();
        
        quest.OnStart += () => _onStartQuest?.Invoke(quest);
        quest.OnUpdated += () => _onUpdateQuest?.Invoke(quest);
        quest.OnCompleted += CompleteTempQuest;
        
        quest.StartQuest();
    }
    
    private void Start()
    {
        StartQuest();
    }

    private void CompleteTempQuest()
    {
        Debug.Log("QuestSystem # Времянный квест завершен!");
        
        SoundManager.Instance.PlaySound(SoundType.Quest);
        
        StartQuest();
    }
    
    public void NextQuest()
    {
        Debug.Log("QuestSystem # Квест: " + _currentQuest.Id + " завершен!");
        
        _currentQuestIndex++;
        
        SoundManager.Instance.PlaySound(SoundType.Quest);
        
        StartQuest();
    }

    private void StartQuest()
    {
        if (_currentQuestIndex < _quests.Count)
        {
            _currentQuest = _quests.ElementAt(_currentQuestIndex);
            
            _currentQuest.Init();
            
            _currentQuest.OnStart += () => _onStartQuest?.Invoke(_currentQuest);
            _currentQuest.OnUpdated += () => _onUpdateQuest?.Invoke(_currentQuest);
            _currentQuest.OnCompleted += NextQuest;
            
            _currentQuest.StartQuest();
            
            Debug.Log("Квест: " + _currentQuest.Id + " начат!");
        }
        else
        {
            _onAllQuestCompleted?.Invoke();
            
            Debug.Log("QuestSystem # Все квесты завершены!");
        }
    }
}