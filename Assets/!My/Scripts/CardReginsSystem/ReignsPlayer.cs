using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReignsPlayer
{
    [SerializeField] private List<CardEvent> AllEvents = new List<CardEvent>();
    public List<CardEvent> GameOverEvents = new List<CardEvent>();

    private List<CardEvent> usedRandomCards = new List<CardEvent>();
    public Queue<CardEvent> cardQueue = new Queue<CardEvent>();
    private List<(int, CardEvent)> delayCards = new List<(int, CardEvent)>();
    private CardEvent currentCard;

    public Action<CardEvent> OnShowCard;

    public Func<string> IsLoseFunc; // Функция для проверки поражения, по умолчанию всегда возвращает false

    public event Action<string> OnLose;
    public Action OnGameOver;

    /// <summary>
    /// Инициализация: подгружаем события и карты поражения
    /// </summary>
    public void Init(CardEvent[] cardEvents = null, CardEvent[] loseCardEvents = null)
    {
        AllEvents.Clear();
        if (cardEvents == null)
            cardEvents = EventLoader.Load("events");
        AllEvents.AddRange(cardEvents);

        GameOverEvents.Clear();
        GameOverEvents.AddRange(loseCardEvents);

        usedRandomCards.Clear();
        cardQueue.Clear();
        currentCard = null;
    }

    /// <summary>
    /// Показывает карту игроку
    /// </summary>
    public void ShowCardEvent(CardEvent card, bool ingnoreWait = false)
    {
        if(!ingnoreWait && card.WaitToPlay > 0)
        {
            delayCards.Add((card.WaitToPlay, card));
            ShowNextCard();
            return;
        }

        currentCard = card;
        OnShowCard?.Invoke(card);
    }

    /// <summary>
    /// Игрок сделал выбор
    /// </summary>
    public void PlayerChoice(bool right)
    {
        if (currentCard == null) return;

        // Создаём контекст действия
        CardActionContext context = new CardActionContext
        {
            IsRightChoice = right
        };

        if((context.IsRightChoice && currentCard.NextRight == "GameOver") ||
            (!context.IsRightChoice && currentCard.NextLeft == "GameOver"))
        {
            OnGameOver?.Invoke();
            return;
        }

        // Выполняем действие карты
        currentCard.Execute(context);

        // Сначала проверяем ресурсы
        string loseCard = IsLoseFunc?.Invoke();
        if (!string.IsNullOrEmpty(loseCard))
        {
            OnLose?.Invoke(loseCard);
            //PlayGameOverCard(loseCard);
            return;
        }

        // Определяем следующую карту
        string nextCardID = right ? currentCard.NextRight : currentCard.NextLeft;
        if (!string.IsNullOrEmpty(nextCardID))
        {
            CardEvent nextCard = AllEvents.Find(c => c.EventID == nextCardID);
            if (nextCard != null)
            {
                cardQueue.Enqueue(nextCard);
            }
        }

        // Показ следующей карты
        //ShowNextCard();
    }

    /// <summary>
    /// Показывает следующую карту: очередь или случайная
    /// </summary>
    public void ShowNextCard()
    {
        foreach ((int, CardEvent) delayCardsCort in delayCards)
        {
            int value = delayCardsCort.Item1;
            value--;
        }

        for (int i = 0; i < delayCards.Count; i++)
        {
            (int, CardEvent) delayCardsCort = delayCards[i];
            if(delayCardsCort.Item1 <= 0)
            {
                ShowCardEvent(delayCardsCort.Item2, true);
                return;
            }
        }

        if (cardQueue.Count > 0)
        {
            ShowCardEvent(cardQueue.Dequeue());
            return;
        }

        // Берём случайную карту среди тех, что можно выбирать
        List<CardEvent> availableCards = AllEvents.FindAll(c => !c.onlyChoiceThisCard && !usedRandomCards.Contains(c));

        if (availableCards.Count == 0)
        {
            usedRandomCards.Clear();
            availableCards = AllEvents.FindAll(c => !c.onlyChoiceThisCard);
        }

        CardEvent randomCard = availableCards[UnityEngine.Random.Range(0, availableCards.Count)];
        usedRandomCards.Add(randomCard);

        ShowCardEvent(randomCard);
    }

    /// <summary>
    /// Воспроизводит карту поражения по тегу
    /// </summary>
    public void PlayGameOverCard(string tag)
    {
        CardEvent overCard = GameOverEvents.Find(c => c.EventID == tag);
        if (overCard == null)
        {
            Debug.LogError("Не найдена карта поражения с тегом: " + tag);
            return;
        }

        cardQueue.Clear();
        ShowCardEvent(overCard);
    }
}

/// <summary>
/// Контекст выполнения действия карты
/// </summary>
public class CardActionContext
{
    public bool IsRightChoice;
}
