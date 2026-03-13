using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharChoiceButton : MonoBehaviour
{
    [SerializeField] private Button button;

    [Space]
    [SerializeField] private Image Portrait;
    [SerializeField] private Image progressBarR1;
    [SerializeField] private Image progressBarR2;
    [SerializeField] private Image progressBarR3;
    [SerializeField] private Image progressBarR4;
    [SerializeField] private Image progressBarR1_icon;
    [SerializeField] private Image progressBarR2_icon;
    [SerializeField] private Image progressBarR3_icon;
    [SerializeField] private Image progressBarR4_icon;
    [SerializeField] private Gradient gradientColorValue;

    [Space]
    [SerializeField] private GameObject nonInteractGroop;
    [SerializeField] private TextMeshProUGUI textAchiv;

    private ChoiseCharacterData characterData;

    public Action<ChoiseCharacterData> OnChoise;

    public void Init(ChoiseCharacterData choiseCharacter)
    {
        this.characterData = choiseCharacter;

        Portrait.sprite = characterData.Portrait;
        SetProgressBar(progressBarR1, progressBarR1_icon, characterData.R1 / 100f);
        SetProgressBar(progressBarR2, progressBarR2_icon, characterData.R2 / 100f);
        SetProgressBar(progressBarR3, progressBarR3_icon, characterData.R3 / 100f);
        SetProgressBar(progressBarR4, progressBarR4_icon, characterData.R4 / 100f);

        button.onClick.AddListener(() => Choise());

        if(choiseCharacter.AchivForOpen != "None" &&
            !AchieviementSystem.IsUnlockAchiv(choiseCharacter.AchivForOpen))
        {
            nonInteractGroop.SetActive(true);
            textAchiv.text = $"Get Achievement\n<color=yellow>[{AchieviementSystem.GetAchivInfo(choiseCharacter.AchivForOpen).Title}]";
            button.interactable = false;
        }
    }

    private void SetProgressBar(Image progressBar, Image icon, float v)
    {
        progressBar.fillAmount = v;
        progressBar.color = gradientColorValue.Evaluate(v);
        icon.color = gradientColorValue.Evaluate(v);
    }

    public void Choise()
    {
        OnChoise?.Invoke(characterData);
    }
}
