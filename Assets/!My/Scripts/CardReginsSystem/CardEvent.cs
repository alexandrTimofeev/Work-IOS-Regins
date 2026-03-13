using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// Карточка события
/// </summary>
[Serializable]
public class CardEvent : ICloneable
{
    public string EventID;
    public string EventImageID;
    public string CharacterID;
    public string Title;
    public string Text;
    public string LeftText;
    public string RightText;
    public int[] LeftResources = new int[4];   // People, Army, Gold, Senate
    public int[] RightResources = new int[4];
    public string NextLeft;
    public string NextRight;
    public int Weight;
    public string Condition;
    public bool onlyChoiceThisCard = false;
    public int WaitToPlay;

    public CardEvent(string eventID, string eventImageID, string characterID, string title, string text, string leftText, string rightText, int waitToPlay, int[] leftResources, int[] rightResources, string nextLeft, string nextRight, int weight, string condition, bool onlyChoiceThisCard)
    {
        EventID = eventID;
        EventImageID = eventImageID;
        CharacterID = characterID;
        Title = title;
        Text = text;
        LeftText = leftText;
        RightText = rightText;
        WaitToPlay = waitToPlay;
        LeftResources = leftResources;
        RightResources = rightResources;
        NextLeft = nextLeft;
        NextRight = nextRight;
        Weight = weight;
        Condition = condition;
        this.onlyChoiceThisCard = onlyChoiceThisCard;
    }

    public CardEvent()
    {
    }

    /// <summary>
    /// Выполнение действий карты
    /// </summary>
    public void Execute(CardActionContext context)
    {
        int[] delta = context.IsRightChoice ? RightResources : LeftResources;
        GameG.ResourceManager.AddOrRemoveResource(delta[0] * 10, delta[1] * 10, 
            delta[2] * 10, delta[3] * 10);
    }

    public CardEvent CloneForEdit()
    {
        return new CardEvent(EventID, EventImageID, CharacterID, Title, Text, LeftText, RightText, WaitToPlay, (int[])LeftResources.Clone(), (int[])RightResources.Clone(), NextLeft, NextRight, Weight, Condition, onlyChoiceThisCard);
    }

    public object Clone()
    {
        return CloneForEdit();
    }
}