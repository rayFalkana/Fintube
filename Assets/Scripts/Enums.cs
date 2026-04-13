public class Enums 
{
    [System.Serializable]
    public enum Memory
    {
        MEM_INTRO_START,
        MEM_INTRO_END,
        MEM_OTTO_START,
        MEM_OTTO_END,
        MEM_ELISE_START,
        MEM_ELISE_END,
        MEM_STEVE_START,
        MEM_STEVE_END,
        PART_ONE_END,
        DOG_SUBLEVEL_2_1,
        DOG_SUBLEVEL_2_2,
        DOG_LEVEL_1,
        ENTITY_LEVEL_3_1,
        ENTITY_LEVEL_3_2
    }

    //Here you can update any list of audio to be played in your game, just put inside the enum for BGM and SFX
    [System.Serializable]
    public enum BGMType
    {
        AREA_ONE,
    }

    [System.Serializable]
    public enum SFXType
    {
        BUTTON_CLICK,
        BUTTON_CLICK_PLATE,
        PLAYER_SKILL,
        GATE_OPEN,
        ELEVATOR_ARRIVE,
        DOOR_OPEN,
        PLAYER_WINDING_KEY,
        PLAYER_USING_KEY,
        PLAYER_GETITEMS
    }

    [System.Serializable]
    public enum TerrainType
    {
        DEFAULT,
        GRASS,
    }

    [System.Serializable]
    public enum Dotween
    {
        DEFAULT,
        BLACKSCREEN_FADEINOUT,
        MOVE_TO_ONE_TARGET,
        DELAY_INVOKE
    }

    [System.Serializable]
    public enum TriggerState
    {
        NONE,
        TRIGGER,
        INVENTORY,
        TRIGGER_REQ,
        PASSING_THROUGH,
        OBJECT_GRAB_ONE_MANY,
        TRIGGER_DOUBLE,
        TRIGGER_REQ_DOUBLE,
        TRIGGER_REQ_JUST,
        OBJECT_GRAB_ONE_SELECTIVE
    }

    [System.Serializable]
    public enum Subject
    {
        SUBJECT_ZERO,
        SUBJECT_ONE,
        SUBJECT_TWO,
        SUBJECT_THREE
    }

    [System.Serializable]
    public enum NPCType
    {
        TALK_INTERACT,
        TALK_INTERACT_EVENT,
        TALK_ONCE,
        TALK_PASSING_THROUGH,
        TALK_INTERACT_SCRIPT
    }

    [System.Serializable]
    public enum GalleryState
    {
        EVENT_1,
        EVENT_2,
        EVENT_3,
        EVENT_4,
        EVENT_5
    }

    [System.Serializable]
    public enum InputState
    {
        KEYBOARD,
        GAMEPAD,
        GAMEPAD_USB,
        JOYSTICK,
    }

    [System.Serializable]
    public enum ObjectPushType
    {
        NONE,
        GRAB_LEFT_RIGHT,
        PUSH,
        GRAB,
    }

    [System.Serializable]
    public enum BoxRotateType
    {
        NONE,
        TBL,
        TBS
    }
}

