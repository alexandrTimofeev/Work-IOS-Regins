using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

// Глобальные системы
public static class G
{
    public static IInput _Input;
    public static ReadOnlyGameData GlobalData;

    public static void Init()
    {
        Debug.Log("G initialized (global systems)");

        _Input = InputFabric.GetOrCreateInpit(true);
        GameGlobalData.ClearInstance();
        GlobalData = GameGlobalData.Instance.Data;
    }
}

// Локальные G-системы сцены Game
public static class GameG
{
    //Stundart

    public static GameSessionData SessionData;
    //public static GameMain Main;
    public static ScoreSystem ScoreSys;
    public static ResourceManager ResourceManager;
    public static ButtonGameActionMediator ButtonGameMediator;
    public static GrappableObjectMediator GrappableObjectMediator;    

    public static CellPlacer CellPlacer;
    public static BonusExecuter BonusExecuter;

    //ForThisGame
    public static GameSessionManagerMono GameSessionManagerMono;
    public static LevelData CurrentLevelData;
    public static ReignsPlayer ReignsPlayer;
    public static ReignsUI ReignsUI;
    public static ReignsInput ReignsInput;

    public static void Init()
    {
        Debug.Log("GameG initialized (scene systems)");
        InitBase();

        //ForThisGame
        GameSessionManagerMono = Object.FindAnyObjectByType<GameSessionManagerMono>(FindObjectsInactive.Include);
        CurrentLevelData = LevelData.LoadFromResources(LevelSelectWindow.CurrentLvl);

        ReignsPlayer = new ReignsPlayer();
        ReignsUI = Object.FindAnyObjectByType<ReignsUI>(FindObjectsInactive.Include);
    }

    private static void InitBase()
    {
        CellPlacer = Object.FindAnyObjectByType<CellPlacer>();

        ScoreSys = new ScoreSystem();
        SessionData = new GameSessionData();
        ButtonGameMediator = new ButtonGameActionMediator();
        GrappableObjectMediator = new GrappableObjectMediator();
        ResourceManager = new ResourceManager();
        ReignsInput = new ReignsInput();

        BonusExecuter = new GameObject().AddComponent<BonusExecuter>();
    }
}

// Локальные G-системы сцены Menu
public static class MenuG
{
    public static void Init()
    {
        Debug.Log("MenuG initialized (scene systems)");
    }
}
