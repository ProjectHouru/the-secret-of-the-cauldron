using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private DialogueActor[] _actors;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private float _typingSpeed = 0.05f;

    private DialogueActor _currentActor;
    private Queue<DialogueMessage> _messages = new();
    private bool _isTyping;
    private bool _isDialogueActive;
    private VisualElement _dialogue;

    private event UnityAction _onStart;

    public event UnityAction OnStart
    {
        add
        {
            // Только один подписчик
            _onStart = null;
            _onStart += value;
        }
        remove => _onStart -= value;
    }

    private event UnityAction _onEnd;

    public event UnityAction OnEnd
    {
        add
        {
            // Только один подписчик
            _onEnd = null;
            _onEnd += value;
        }
        remove => _onEnd -= value;
    }

    //private VisualElement _testMarker; 

    protected void Awake()
    {
        var root = _uiDocument.rootVisualElement;

        _dialogue = root.Q<VisualElement>("Dialogue");

        HideDialogue();
    }

    protected void Start()
    {
        // Инициализация VisualElement
        /*var root = _uiDocument.rootVisualElement;

        _testMarker = new VisualElement
        {
            style =
            {
                width = 10,
                height = 10,
                backgroundColor = new StyleColor(Color.red),
                position = Position.Absolute  // Используем абсолютное позиционирование
            }
        };

        root.Add(_testMarker);*/
    }

    private void Update()
    {
        if (_isDialogueActive)
        {
            GameUI.Instance.ShowNoty("Нажмите E чтобы продолжить!");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Нажата E чтобы продолжить!");
                ShowNextSentence();
            }
        }
    }

    private void FixedUpdate()
    {
        // Положение на экране
        if (_currentActor && (_isTyping || _isDialogueActive))
        {
            UpdateDialoguePosition();
        }
    }

    private void UpdateDialoguePosition()
    {
        // Положение на экране
        Vector2 screenPosition = _mainCamera.WorldToScreenPoint(_currentActor.Point().position);

        // Преобразуем координаты в зависимости от текущего разрешения экрана
        float normalizedX = screenPosition.x / Screen.width;
        float normalizedY = screenPosition.y / Screen.height;

        // Обновляем позицию VisualElement с учётом разрешения
        _dialogue.style.left = normalizedX * _uiDocument.rootVisualElement.resolvedStyle.width -
                               _dialogue.resolvedStyle.width / 2;
        _dialogue.style.top = (1 - normalizedY) * _uiDocument.rootVisualElement.resolvedStyle.height -
                              _dialogue.resolvedStyle.height / 2;

        // Маркер для тестов
        //_testMarker.style.left = normalizedX * _uiDocument.rootVisualElement.resolvedStyle.width - _testMarker.resolvedStyle.width / 2;
        //_testMarker.style.top = (1 - normalizedY) * _uiDocument.rootVisualElement.resolvedStyle.height - _testMarker.resolvedStyle.height / 2;
    }

    public void SetCurrentActor(EDialogueActor newActor)
    {
        _currentActor = null;

        foreach (var actor in _actors)
        {
            if (actor.Actor() == newActor)
            {
                _currentActor = actor;
            }
        }
    }

    public void OneMessage(DialogueMessage message)
    {
        StopAllCoroutines();

        StartCoroutine(TypeSentence(message));
    }

    public void StartDialogue(DialogueMessage[] dialogues)
    {
        // Если кто-то что-то говорит - убираем
        StopAllCoroutines();
        _isTyping = false;

        // Скрываем диалог
        HideDialogue();

        // Заполняем новую очередь фразами из базы
        _messages.Clear();
        foreach (DialogueMessage dialogue in dialogues)
        {
            _messages.Enqueue(dialogue);
        }

        // Запускаем отложенный на несколько милисекунд старт диалога
        // Нужен чтобы метод Update пропустил первое нажатие "E" для начала диалога, иначе теряется первая фраза
        StartCoroutine(StartDialogueWithDelay());

        // Начинаем печать первой фразы
        ShowNextSentence();

        // Отправляем событие о начале нового диалога
        _onStart?.Invoke();
    }

    public void ShowNextSentence()
    {
        // Если текст печатается, то пропускаем его до конца и убираем из очереди
        if (_isTyping)
        {
            StopAllCoroutines();

            if (_messages.Count > 0)
            {
                // Достаем фразу из очереди (с удалением)
                var dialogue = _messages.Dequeue();

                Debug.Log("Завершение печати (принудительно): " + dialogue.Actor + ": " + dialogue.Message);

                _dialogue.Q<Label>("Message").text = _currentActor.Name() + " " + dialogue.Message;

                _isTyping = false;
                
                if (dialogue.EndAction != null)
                {
                    dialogue.EndAction.Execute();
                }
                
                return;
            }
        }

        // Завершаем диалог
        if (_messages.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            // Достаем фразу из очереди (без удаления)
            var dialogue = _messages.Peek();

            Debug.Log("Отдано на печать: " + dialogue.Actor + ": " + dialogue.Message + " # " + dialogue.StartAction + " # " + dialogue.EndAction);

            if (dialogue.StartAction != null)
            {
                dialogue.StartAction.Execute();
            }

            // Печатаем букву за буквой
            StartCoroutine(TypeSentence(dialogue));
        }
    }

    IEnumerator TypeSentence(DialogueMessage dialogue)
    {
        // Получаем спикера
        SetCurrentActor(dialogue.Actor);

        // Проигрываем звук начал диалога
        SoundManager.Instance.PlaySound(SoundType.Dialogue);

        if (_currentActor != null)
        {
            _isTyping = true;

            _dialogue.Q<Label>("Message").text = _currentActor.Name() + " ";

            ShowDialogue();

            foreach (char letter in dialogue.Message)
            {
                _dialogue.Q<Label>("Message").text += letter;

                yield return new WaitForSeconds(_typingSpeed);
            }

            // Небольшую паузу стоит оставить, чтобы сразу диалог не переключали, как закончился
            yield return new WaitForSeconds(1f);

            Debug.Log("Завершение печати (по таймауту): " + dialogue.Actor + ": " + dialogue.Message);

            // Напечатали - убираем из очереди
            if (_messages.Count > 0)
            {
                _messages.Dequeue();
            }

            // Если не в диалоге, то фразы меняются сами
            // Если в диалоге - то автоматом не меняем, только по клавише E
            if (!_isDialogueActive)
            {
                yield return new WaitForSeconds(3f);

                if (_messages.Count > 0)
                {
                    // Проверяем следующую фразу
                    ShowNextSentence();
                }
                else
                {
                    HideDialogue();
                }
            }

            _isTyping = false;
        }
        else
        {
            Debug.Log("Отсутствует персонаж " + dialogue.Actor + "из диалога");
        }
    }

    IEnumerator StartDialogueWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        _isDialogueActive = true;
    }

    private void EndDialogue()
    {
        GameUI.Instance.HideNoty();

        HideDialogue();

        _isDialogueActive = false;
        _isTyping = false;

        _onEnd?.Invoke();

        Debug.Log("Диалог завершен!");
    }

    private void ShowDialogue()
    {
        _dialogue.visible = true;
    }

    public void HideDialogue()
    {
        _dialogue.visible = false;
    }
}
