public class Names
{
    #region TAGS
    public const string PlayerTag = "Player";
    public const string InteractableTag = "Interactable";
    public const string NPCTag = "NPC";
    public const string RespawnTag = "Respawn";
    #endregion

    #region SCENE
    public const string GameplaySceneName = "Isometric2025";
    public const string GallerySceneName = "Gallery";
    #endregion

    #region MapAction
    public const string ContMainMenu = "Player_MainMenu";
    public const string ContGameplay = "Player_Sandbox";
    public const string ContGallery = "Player_Gallery";
    public const string ContMenuGallery = "Player_Rhythm";
    #endregion

    #region NAME CHARACTER
    public static string NameChar(Enums.Subject _person)
    {
        switch (_person)
        {
            case Enums.Subject.SUBJECT_ZERO: return "Jane";
            case Enums.Subject.SUBJECT_ONE: return "Michelle";
            case Enums.Subject.SUBJECT_TWO: return "";
            case Enums.Subject.SUBJECT_THREE: return "";
        }
        return "";
    }
    #endregion

    #region INTERACT ID NAME
    public const string PromoteID = "Promote";
    public const string UseBlackScreenID = "UseBlackScreen";
    #endregion

    #region Controller ID NAME
    public const string USBGamepad = "USB Gamepad";
    #endregion

    #region LAYER MASK
    public const string PlayerMask = "Player";
    public const string WhatStopsMask = "WhatStopPlayer";
    public const string GrassMask = "Grass";
    public const string PushObjectMask = "PushObject";
    public const string DefaultMask = "Default";
    #endregion
}
