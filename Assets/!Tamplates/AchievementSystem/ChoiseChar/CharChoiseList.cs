using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharChoiseList : MonoBehaviour
{
    [SerializeField] private CharChoiceButton choiceButtonPref;
    [SerializeField] private Transform container;

    private List<CharChoiceButton> choiceList = new List<CharChoiceButton>();

    private void Awake()
    {
        GetComponent<WindowUI>().OnOpen += (win) => Open();
        //GetComponent<WindowUI>().OnClose += () => Clear();
    }

    public void Open()
    {
        Clear();

        StartCoroutine(CreateAllButton());
    }

    private IEnumerator CreateAllButton()
    {
        ChoiseCharacterData[] choiseCharacterDatas = G.GlobalData.CharactersPlayer;
        for (int i = 0; i < choiseCharacterDatas.Length; i++)
        {
            CreateButton(choiseCharacterDatas[i]);
            yield return new WaitForSeconds(0.35f);
        }
    }

    private void CreateButton(ChoiseCharacterData choiseCharacterData)
    {
        CharChoiceButton charChoice = Instantiate(choiceButtonPref, container);
        charChoice.Init(choiseCharacterData);

        charChoice.OnChoise += ChoiseWork;
        choiceList.Add(charChoice);
    }

    private void ChoiseWork(ChoiseCharacterData data)
    {
        ChoiseCharacterSystem.SelectCharacter(data.ID);
        FindFirstObjectByType<FadeEffectObject>(FindObjectsInactive.Include)
            .StartBlackFade(() => GameSceneManager.LoadGame());
    }

    private void Clear()
    {
        StopAllCoroutines();

        foreach (var item in choiceList)
        {
            Destroy(item.gameObject);
        }
        choiceList.Clear();
    }
}