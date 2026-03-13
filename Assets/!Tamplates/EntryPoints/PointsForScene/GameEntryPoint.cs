using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

// EntryPoint сцены Game
public class GameEntryPoint : ISceneEntryPoint
{
    public string SceneName => GameSceneManager.GameSceneName;
    public void InitGSystems() => GameG.Init();
    public void OnSceneLoaded()
    {
        Debug.Log("Game scene loaded");

        GameG.SessionData.Init(G.GlobalData);
        PocketRandomazer.Clear();

        InitInterface();
        InitButtonMediator();
        InitInterfaceCommands();

        InitBonusMediator();

        InitResourceReaction();
        InitPlayer();

        InitAchivObserv();

        /*InitPlayer();
        InitFinal();
        InitLevel();*/

        AudioManager.Init();

        ApplyCharacterSettings();
        CardEvent startCard = GameG.GameSessionManagerMono.GetStartCard();
        if(startCard != null)
            GameG.ReignsPlayer.cardQueue.Enqueue(startCard);
        GameG.ReignsPlayer.ShowNextCard();

        GamePause.SetPause(false);
    }

    public void OnSceneUnloaded()
    {
        GameG.ReignsInput.Clear();
    }

    private static void InitInterface()
    {
        InterfaceManager.Init();
        //GameG.ScoreSys.OnAddScore += (score, point) => InterfaceManager.CreateScoreFlyingText(score, point);

        InterfaceManager.BarMediator.ShowForID("Score", 0);
        GameG.ScoreSys.OnScoreChange += (info) =>
        {
            InterfaceManager.BarMediator.ShowForID("Score", info.Value);
            if(info.Point.HasValue)
                InterfaceManager.CreateScoreFlyingText(info.Delta, info.Point.Value);
        }; 

        //InterfaceManager.BarMediator.ShowForID("BulletsCount", GameG.Player.Gun.CountBullets.Value);
        //GameG.Player.OnCountBulletChange += (value) => InterfaceManager.BarMediator.ShowForID("BulletsCount", value);
        //GameG.Player.OnReloadingTimeChange += (wait) => InterfaceManager.BarMediator.ShowForID("Reloading", 
        //    (GameG.Player.Gun.ReloadingTime - wait) / GameG.Player.Gun.ReloadingTime);

        //InterfaceManager.BarMediator.ShowForID("Life", GameG.SessionData.LifeContainer.Value);
        //GameG.SessionData.LifeContainer.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("Life", value);
    }

    private void InitButtonMediator()
    {
        GameG.ButtonGameMediator.OnClick += (actionInfo) =>
        {
            switch (actionInfo.ActionType)
            {
                case ButtonGameActionType.None:
                    break;
                case ButtonGameActionType.Delite:
                    break;
            }
        };
    }

    private void InitPlayer()
    {
        GameG.ReignsInput.Init(G._Input, GameG.ReignsUI);

        GameG.ReignsUI.Init(GameG.ReignsPlayer, G.GlobalData.ReigsUIData);
        GameG.ReignsUI.OnChoise += GameG.ReignsPlayer.PlayerChoice;
        GameG.ReignsUI.OnEndAnimationChoise += () =>
        {
            if (GameG.GameSessionManagerMono.LoseCard == "")
                GameG.ReignsPlayer.ShowNextCard();
            else
                GameG.ReignsPlayer.PlayGameOverCard(GameG.GameSessionManagerMono.LoseCard);
        };
        GameG.ReignsPlayer.OnGameOver += () =>
        {
            InterfaceManager.ShowLoseWindow(GameG.GameSessionManagerMono.DaysInGame,
            LeaderBoard.GetBestScore());
            LeaderBoard.SaveScore(GameG.GameSessionManagerMono.DaysInGame);
            GameG.ReignsInput.Clear();
        };

        GameG.ReignsPlayer.IsLoseFunc = () => GameG.GameSessionManagerMono.LoseCard;

        GameG.ReignsPlayer.Init(loseCardEvents: G.GlobalData.LoseCardEvents);
    }

    private void InitPlayerDamage()
    {
        /*GameG.Player.OnDamage += (d) =>
        {
            EffectsManagerMono.HitStop(G.GlobalData.HitStopPlayerHit, 0.18f);
            EffectsManagerMono.CameraShake(G.GlobalData.CameraShakePunch, isIndependentUpdate: true);
            GameG.SessionData.LifeContainer.RemoveValue(1);
        };

        GameG.SessionData.LifeContainer.OnDownfullValue += (d) => LoseDownfall();*/
    }

    private void InitFinal()
    {
        /*GameG.Player.OnFinal += () =>
        {
            InterfaceManager.ShowWinWindow(GameG.ScoreSys.Score, LeaderBoard.GetBestScore());
            LeaderBoard.SaveScore($"Level{LevelSelectWindow.CurrentLvl}", GameG.ScoreSys.Score);
            GameG.Player.gameObject.SetActive(false);
            LevelSelectWindow.CompliteLvl();
        };*/
    }

    private void Lose()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            InterfaceManager.ShowLoseWindow(GameG.ScoreSys.Score, LeaderBoard.GetBestScore());
            //GameG.Player.gameObject.SetActive(false);
        });
    }

    private void InitBonusMediator()
    {
        GameG.BonusExecuter.Init();
    }

    public void InitInterfaceCommands()
    {
        InterfaceManager.OnClickCommand += (command) =>
        {
            switch (command)
            {
                case InterfaceComand.OpenPause:
                    GameG.GameSessionManagerMono.Pause(true);
                    break;
                case InterfaceComand.ClosePause:
                    GameG.GameSessionManagerMono.Pause(false);
                    break;

                default:
                    break;
            }
        };
    }

    private void InitLevel()
    {
        GameG.GameSessionManagerMono.SetLevelData(GameG.CurrentLevelData);
    }

    private void InitResourceReaction()
    {
        GameG.ResourceManager.OnOverfullResource += (title) =>
        {
            GameG.GameSessionManagerMono.Lose($"{title}_Overfull");            
        };

        GameG.ResourceManager.OnDownfullResource += (title) =>
        {
            GameG.GameSessionManagerMono.Lose($"{title}_Downfull");
        };

        GameG.ResourceManager.Init();
    }

    private void InitAchivObserv()
    {
        GameG.ReignsPlayer.OnShowCard += (card) =>
        {
            GameG.GameSessionManagerMono.OnNextDay();
        };
    }

    private void ApplyCharacterSettings()
    {
        ChoiseCharacterData choiseCharacterData = G.GlobalData.CharactersPlayer.FirstOrDefault((chd) => 
        chd.ID == ChoiseCharacterSystem.CurrentID);
        if (choiseCharacterData != null)
            GameG.GameSessionManagerMono.ApplyCharacter(choiseCharacterData);
    }
}