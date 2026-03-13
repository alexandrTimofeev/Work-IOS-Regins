using System;
using System.Collections.Generic;
using UnityEngine;

public static class ChoiseCharacterSystem
{
    public static string CurrentID { get; private set; }
    private static List<ChoiseCharacterVisualGO> choiseCharacterVisualGOs = new List<ChoiseCharacterVisualGO>();

    public static event Action<string> OnSelectCharacter;

    public static void SelectCharacter(string ID)
    {
        CurrentID = ID;
        choiseCharacterVisualGOs.ForEach(go => go.UpdateChoice(ID));
        OnSelectCharacter?.Invoke(ID);

        Debug.Log($"CurrentID Character {CurrentID}");
    }

    public static void AddCharcterObject(ChoiseCharacterVisualGO choiseCharacterVisualGO)
    {
        choiseCharacterVisualGOs.Add(choiseCharacterVisualGO);
    }

    public static void RemoveCharcterObject(ChoiseCharacterVisualGO choiseCharacterVisualGO)
    {
        choiseCharacterVisualGOs.Remove(choiseCharacterVisualGO);
    }
}

[Serializable]
public class ChoiseCharacterData : ICloneable
{
    public string ID;
    public string Name;

    [Space]
    public Sprite Portrait;
    public int R1;
    public int R2;
    public int R3;
    public int R4;

    [Space]
    public string AchivForOpen = "None";
    public CardEvent StartCardEvent;

    public ChoiseCharacterData(string iD, string name, Sprite portrait, int r1, int r2, int r3, int r4, string achivForOpen, CardEvent startCardEvent)
    {
        ID = iD;
        Name = name;
        Portrait = portrait;
        R1 = r1;
        R2 = r2;
        R3 = r3;
        R4 = r4;
        AchivForOpen = achivForOpen;
        StartCardEvent = startCardEvent;
    }

    public object Clone()
    {
        return new ChoiseCharacterData(ID, Name, Portrait, R1, R2, R3, R4, AchivForOpen, StartCardEvent);
    }
}

public class ChoiseCharacterVisualGO : MonoBehaviour
{
    public string ID;

    public void Start()
    {
        ChoiseCharacterSystem.AddCharcterObject(this);
        UpdateChoice(ChoiseCharacterSystem.CurrentID);
    }

    private void OnDestroy()
    {
        ChoiseCharacterSystem.RemoveCharcterObject(this);
    }

    public void UpdateChoice (string id)
    {
        gameObject.SetActive(ID == id);
    }
}