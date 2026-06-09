public enum SoundType
{
    None = 0,

    #region UI (1-10)

    ButtonClick = 1,

    #endregion

    #region Interaction (100-199)

    OpenChest = 100,
    ItemCollect = 105,
    ItemDrop = 106,
    Teleport = 109,

    #endregion

    #region Fighting System  (300-499)

    SwordAttack = 300,
    PlayerTakeDamage = 303,
    PlayerTakeDie = 304,
    
    ObjectTakeDamage = 305,
    ObjectDestroyDamage = 306,
    
    GhostAttack = 307,
    GhostTakeDamage = 310,
    GhostDestroyDamage = 311,
    
    PumpkinAttack = 312,
    PumpkinTakeDamage = 313,
    PumpkinDestroyDamage = 314,

    WispDamage = 315,
    
    #endregion

    #region Game (500-999)

    Win = 500,
    Dialogue = 501,
    Quest = 502,

    #endregion

    /*#region Other (1000+)

    #endregion*/
}