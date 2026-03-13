using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [Space, Header("Resources")]
    public IntContainer R1Container;
    public IntContainer R2Container;
    public IntContainer R3Container;
    public IntContainer R4Container;

    [Space, Header("Game Parametrs")]
    public CardEvent[] LoseCardEvents;
    public ReignsUIData ReigsUIData;
    public ChoiseCharacterData[] CharactersPlayer;
    public CardEvent[] StartTexts;

    [Space, Header("Effects")]
    public float HitStopPlayerHit = 0.5f;
    public Vector3 CameraShakePunch = Vector3.one;


    //[Space]
    //public ChoiseCharacterData[] CharacterDatas;

    /// <summary>
    /// Возвращает полностью защищённый фасад
    /// </summary>
    public ReadOnlyGameData GetReadOnly()
    {
        return ReadOnlyGameData.Create(this);
    }
}

public class ReadOnlyGameData
{
    public ReadOnlyIntContainer R1Container { get; private set; }
    public ReadOnlyIntContainer R2Container { get; private set; }
    public ReadOnlyIntContainer R3Container { get; private set; }
    public ReadOnlyIntContainer R4Container { get; private set; }

    //Game Parametrs
    public CardEvent[] LoseCardEvents;
    public ReignsUIData ReigsUIData;
    public ChoiseCharacterData[] CharactersPlayer;
    public CardEvent[] StartTexts;

    //Effects
    public float HitStopPlayerHit { get; private set; }
    public Vector3 CameraShakePunch { get; private set; }


    // Приватный конструктор, чтобы не создавать объект напрямую
    private ReadOnlyGameData() { }

    /// <summary>
    /// Фабричный метод для создания ReadOnlyGameData
    /// из GameData.
    /// </summary>
    public static ReadOnlyGameData Create(GameData data)
    {
        var readOnly = new ReadOnlyGameData
        {
            //Resources
            R1Container = new ReadOnlyIntContainer(data.R1Container),
            R2Container = new ReadOnlyIntContainer(data.R2Container),
            R3Container = new ReadOnlyIntContainer(data.R3Container),   
            R4Container = new ReadOnlyIntContainer(data.R4Container),

            //Game Parametrs
            LoseCardEvents = Utils.GetClones(data.LoseCardEvents),
            ReigsUIData =  data.ReigsUIData.CloneForEdit(),
            CharactersPlayer = Utils.GetClones(data.CharactersPlayer),
            StartTexts = Utils.GetClones(data.StartTexts),

            //Effects
            HitStopPlayerHit = data.HitStopPlayerHit,
            CameraShakePunch = data.CameraShakePunch,
        };
        return readOnly;
    }
}