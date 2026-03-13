using System;
using TMPro;
using UnityEngine;

public class GameSessionManagerMono : MonoBehaviour
{
    [SerializeField] private GameObject reloadBack;
    [SerializeField] private TextMeshProUGUI countBulletsTmp;

    public string LoseCard { get; private set; } = "";
    public int DaysInGame { get; private set; } = 0;

    public void ReloadState(bool show)
    {
        reloadBack.SetActive(show);
        countBulletsTmp.color = show ? (Color.gray - new Color(0, 0, 0, 0.5f)) : Color.white;
    }

    public void Pause(bool isPause)
    {
        GamePause.SetPause(isPause);
    }

    public void SetLevelData(LevelData levelData)
    {
        if(levelData == null)
            return;
    }

    public void Lose(string v)
    {
        LoseCard = v;
        PlayerPrefs.SetInt("Survive", DaysInGame);
        AchieviementSystem.ForceUnlock(v);

        Debug.Log($"LOSE: {v}");
    }

    public void OnNextDay()
    {
        DaysInGame++;
    }

    private ChoiseCharacterData choiseCharacterData;
    public void ApplyCharacter(ChoiseCharacterData choiseCharacterData)
    {
        this.choiseCharacterData = choiseCharacterData;

        GameG.ResourceManager.SetResources(choiseCharacterData.R1, choiseCharacterData.R2, choiseCharacterData.R3,
            choiseCharacterData.R4);
    }

    public CardEvent GetStartCard()
    {
        return choiseCharacterData.StartCardEvent;
    }
}
