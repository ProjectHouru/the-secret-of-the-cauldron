using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueBase: Singleton<DialogueBase>
{
    [SerializeField] private DialogueWitchMoveAction _witchMoveToPoint;

    private List<string> _witchBossPhrases = new()
    {
     "Да где-ж оно лежит то?",
     "Ага, нашла! А нет, это просто старые тряпки.",
     "Ай, память у бабушки дырявая. Здесь же рухлядь эта лежала!",
     "Мышиный помет, ухо единорога... Ты жив там еще, милок?",
     "Помрет, не помрет, помрет, не помрет... Клятая ромашка.",
    };

    private List<string> _witchPhrases = new()
    {
     "Куда ж я положила паучьи мозги, корга я старая? А вот они! А нет... это печень дождевых червей. Проклятье!",
     "Да куда ж запропал тот самый порошок из глаз мертвецов? Ужас, что за беспорядок!",
     "Ага, вот и горькие корни! Нет, это просто старые тряпки. Надо убрать!",
     "Так, так, что-то мне подсказывает, что я прятала мох из пещер в… Или это просто старые щётки? Ох, голова моя уже не та!",
     "Ужас, куда же я дела порошок из пепла того беззубого старого вампира? Это чтоль? Нет, это просто пачка сахара… Прах тебя побери!",
     "Где же мои старые очки! Как я без них вообще вижу?",
     "Как же я ненавижу этот беспорядок! Ну вот же оно - зловещее манго, которое леший мне принёс третьего дня! А, нет, это просто сгнившее яблоко.",
     "Флакон с слезами призрака! То что нужно! Добавить сейчас или просто выпить залпом? Хм..",
     "Кровь невидимок или это просто сок из свеклы? Дать бы тому ротозею попробовать, да копыта откинет же!",
     "Вот важнейший ингредиент: крылья летучей мыши! Ах, нет, это просто старые перчатки... Как я могла перепутать?",
     "Мышь! Сожрала ухо единорога! Пропади ты, нечисть! Пошла прочь!",
     "Чешуя со спины дракона! Ой, нет, это просто старая пуговица... Интересно, этот простак вытащит ещё одну у Жоры? Ладно, поищу ещё.",
     "Глаз циклопа! О, нашла! Нашла! Что? Нет, это просто большая бусина...",
     "Ух... фу! Какая мерзкая стухшая луковица. А ведь и нет другой. Ну, ладно.",
     "Какой милый маленький грибочек! И сколопендровый листочек! Ну, что за прелесть!",
     "А это укроп или полынь? А есть вообще разница то?",
    };

    private List<string> _phrases = new();

    private int _witchPhraseIndex = 0;

    private Dictionary<QuestId, DialogueMessage[]> Base()
    {
         return new Dictionary<QuestId, DialogueMessage[]>
         {
             { QuestId.FirstMeeting, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Здравствуйте, бабушка! А чого Вы готовите здеся?"),
                 new DialogueMessage( EDialogueActor.Witch, "О, милок, привет-привет! Варю я блюдо особое, праздничное. Как оно... Хи.. хэ... Хи-лу-иннское."),
                 new DialogueMessage( EDialogueActor.Witch, "Тьфу ты, бесова душа, не выговоришь! Но без особых ингредиентов оно не получится! Вот напасть то…"),
                 new DialogueMessage( EDialogueActor.Player, "О, так я могу помочь то! Легше легкого. А что Вам нужно то, бабуль?"),
                 new DialogueMessage( EDialogueActor.Witch, "А нужны мне ядовитые колокольчики в первую руку, дорогой! Без них блюдо не получится таким... ужасающим!"),
                 new DialogueMessage( EDialogueActor.Player, "Воно чого… А где ж я могу найти таковы?"),
                 new DialogueMessage( EDialogueActor.Witch, "Да вон, на болоте, родной! Рукой подать, отсюдова видно! Тама где светлячки едва ли тебя покусают. "),
                 new DialogueMessage( EDialogueActor.Witch, "Но будь осторожен — там ядовитые лужи! Сапог то резиновый найдётся, милый?"),
                 new DialogueMessage( EDialogueActor.Player, "Та лаптями обойдусь, бабусь! А чого за варево у Вас то в котле то?"),
                 new DialogueMessage( EDialogueActor.Witch, "Ха-ха! Это секрет! Но если всё получится, то на выходе будет жутко вкусно, уж поверь мне, дружок!"),
             }},
             { QuestId.ComebackWithBells, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Я собрал ядовитые колокольчики, бабуль!"),
                 new DialogueMessage( EDialogueActor.Witch, "Молодец! Откуда ты их выудил? Ох и вонь то! Наверняка, в болотной жиже извозился!"),
                 new DialogueMessage( EDialogueActor.Player, "Не без этого, но я вернулся!"),
                 new DialogueMessage( EDialogueActor.Witch, "Ладно, давай их сюда. Три... четыре... Так, теперь добавим их в котел!"),
                 new DialogueMessage( EDialogueActor.Player, "А что дальше?"),
                 new DialogueMessage( EDialogueActor.Witch, "Теперь нужно добавить крошку Хрустящего черепа. Без неё зелье будет скучным!"),
                 new DialogueMessage( EDialogueActor.Player, "Я отправлюсь за черепом!"),
                 new DialogueMessage( EDialogueActor.Witch, "Да погоди же, торопыга! Кроме черепа, который лежит у гробницы древнего короля на кладбище, придётся тебе найти парочку тыкв!"),
                 new DialogueMessage( EDialogueActor.Player, "А откуда они там водятся то?!"),
                 new DialogueMessage( EDialogueActor.Witch, "Так вестимо откуда - тыквоголовый бушует. Увидишь как придешь... Токмо аккуратней будь, Ну, ступай, дорогой, ступай!"),
             }},
             { QuestId.ComebackWithSkullAndTeeth, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Бабуля, достал я тыквы! Вот только не знаю, что теперича с тыквоголовым делать?"),
                 new DialogueMessage( EDialogueActor.Witch, "О, смелый какой! Рада, рада, что жив в итоге... Да чой той тыкве станется, побушует да сам уйдет!"),
                 new DialogueMessage( EDialogueActor.Player, "Непросто это было, но справился в итоге!"),
                 new DialogueMessage( EDialogueActor.Witch, "Превосходно! Теперь добавим тыквы в котел!"),
             }}, 
             { QuestId.BringPumpkinsToCauldron, new []{ 
                 new DialogueMessage( EDialogueActor.Player, "Бабуль? А этот тут откуда? Чой теперь делать?..."),
                 new DialogueMessage( EDialogueActor.Witch, "Сдюжил, значится, раз, сдюжишь и второй. Беги, малохольный, бабушка ченить придумает!"),
             }},
             { QuestId.BossFightFinishCast, new []{ 
                 new DialogueMessage( EDialogueActor.Witch, "СВЯТЫЕ МАТРЕШКИ! И ПЮРЕ ИЗ МОРОШКИ!"),
             }},
             { QuestId.BossFightFinishCastEnd_OnlyDialog, new []{ 
                 new DialogueMessage( EDialogueActor.Witch, "Поди вон, окаянный!"),
                 new DialogueMessage( EDialogueActor.Witch, "О, смотри-ка. Сработало!"),
             }},
             {QuestId.BringContinue_OnlyDialog, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Фух... Теперь что делать?"),
                 new DialogueMessage( EDialogueActor.Witch, "О, теперь лишь последние штрихи! Где Череп то?"),
                 new DialogueMessage( EDialogueActor.Player, "Да здеся он, запазухой упрятал. Ща..."),
                 new DialogueMessage( EDialogueActor.Witch, "Да, что ж ты  творишь, эвон раскрошится раньше времени то?"),
                 new DialogueMessage( EDialogueActor.Player, "Ну, я.."),
                 new DialogueMessage( EDialogueActor.Witch, "Как я! Аха-ха-ха-ха! Давай его сюда... Эх, нос то уже раскрошился. Тьфу ты, бесова душа!"),
                 new DialogueMessage( EDialogueActor.Player, "Да не было ж носа, бабуль. Вот те крест!"),
                 new DialogueMessage( EDialogueActor.Witch, "Верю, верю, милок!"),
                 new DialogueMessage( EDialogueActor.Witch, "Вот и раскрошила я черепушку! На-ка, милый, дай её в котёл! Растуды её в качель!"),
                 new DialogueMessage( EDialogueActor.Player, "Мне кинуть? В котёл?"),
                 new DialogueMessage( EDialogueActor.Witch, "Да, да! Кидай! Не сумневайся. Всё ладно будет! А я тут постою.", _witchMoveToPoint),
             }},
             {QuestId.BringSkullToCauldron, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Кинул! Ну, что, бабуль? Долго ещё?"),
                 new DialogueMessage( EDialogueActor.Witch, "Да всё-всё уже, шустряк! Нужно только последний штрих сделать."),
                 new DialogueMessage( EDialogueActor.Witch, "Берём сперва укропу, затем кошачью щёку, 25 картошек, 17 дохлых вошек, ведро воды и гриб туды! Охапка дров и торт готов!"),
             }},
             {QuestId.TheCake_OnlyDialog, new [] {
                 new DialogueMessage( EDialogueActor.Player, "Ух ты! Это торт?"),
                 new DialogueMessage( EDialogueActor.Witch, " Да, жутко вкусный! Поздравляю тебя с Хеллоуином!!"),
                 new DialogueMessage( EDialogueActor.Player, "И Вас тоже, бабуль! Желаю жуткого счастья!"),
             }},
             { QuestId.DeadGraveyard, new [] {
                 new DialogueMessage( EDialogueActor.Witch, "Помер? Опять? Нет, живой! Ну, ничего, бабушка уже ванную приготовила для тебя. Уринотерапия творит чудеса!"),
             }},
             { QuestId.DeadSwamp, new [] {
                 new DialogueMessage( EDialogueActor.Witch, "Ох уж это болото... Вставай милок! Ох и вонь то от тебя тиной..."),
             }},
             { QuestId.DeadWitchPumpkin, new [] {
                 new DialogueMessage( EDialogueActor.Witch, "Ну вставай, родимый, че разлегся то! Не для тебя бабушка землю прахом удобряла, попортишь урожай!"),
             }},
         };
    }

    public string GetWitchMessage()
    {
     // Устанавливаем набор фраз
     SetPhrasesList();
     
     // Перемещиваем
     if (_witchPhraseIndex == 0)
     {
         ShiftPhrases();
     }
     
     // Достаем фразу из списка
     var message = _phrases[_witchPhraseIndex];

     _witchPhraseIndex++;

     if (_witchPhraseIndex >= _phrases.Count)
     {
         _witchPhraseIndex = 0;
     }

     return message;
    }

    private void SetPhrasesList()
    {
     if (GameLogic.Instance.QuestSystem && GameLogic.Instance.QuestSystem.CurrentQuest.Id == QuestId.BossFight)
     {
         if (_phrases != _witchBossPhrases)
         {
             _witchPhraseIndex = 0;
             _phrases = _witchBossPhrases;
         }
     }
     else  if (_phrases != _witchPhrases)
     {
         _witchPhraseIndex = 0;
         _phrases = _witchPhrases;
     }
    }

    private void ShiftPhrases()
    {
     _phrases = _phrases.OrderBy(i => Guid.NewGuid()).ToList();
    }

    public DialogueMessage[] GetDialogue(QuestId questId)
    {
     if (Base().TryGetValue(questId, out var messages))
     {
         return messages;
     }

     return null;
    }
}
