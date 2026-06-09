public enum QuestId
{
    None,
    OnboardingMovement, // Знакомство с передвежением
    OnboardingCollecting, // Взаимодействие с предметами
    OnboardingFightOne, // Разбей вазу
    OnboardingFightTwo, // Разбей вазы
    FirstMeeting, // Встреча с бабулькой
    CollectPoisonBells, // Собрать колокольчики с болот
    ComebackWithBells, // Принести колокольчики
    CollectSkullAndTeeth, // Собрать череп да тыквы
    ComebackWithSkullAndTeeth, // Принести череп и тыквы
    BringPumpkinsToCauldron, // Брось тыквы в котел
    BossFight, // Вот неожиданность
    BossFightFinishCast, // Бабулька зарешает
    BossFightFinishCastEnd_OnlyDialog, // Бабулька зарешала
    BringContinue_OnlyDialog, // Продолжение квеста
    BringSkullToCauldron, // Брось череп в котел
    TheCake_OnlyDialog, // Ух ты! Это торт!
    #region DeathQuest
    DeadGraveyard, // Помер на кладище
    DeadSwamp, // Помер на болоте
    DeadWitchPumpkin, // Помер в локации ведьмы
    #endregion
}