using UnityEngine;
using System.Collections;

public static class Statics {

    //Enum
    public enum Game_Status
    {
        pause,
        play,
        win,
        game_over
    }

    //Variables Globales
    public static Game_Status game_satus;

    //Game Objects
    public static GameObject Token_Root;
    public static GameObject Win_Text;

    //Scripts
    public static TokensController tokens_controller;
    public static Main main_controller;

}
