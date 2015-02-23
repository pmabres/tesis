using UnityEngine;
using UnityEngine.UI;
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
    public static int round_score;
    public static int global_score;
    public static bool inMultipler;
    public static float multipler;

    //Game Objects
    public static GameObject Token_Root;
    public static GameObject Win_Text;
    public static Text Score_Text;

    //Scripts
    public static TokensController tokens_controller;
    public static Main main_controller;



}
