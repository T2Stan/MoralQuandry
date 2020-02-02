﻿using System;
using System.Collections.Generic;
using Fungus;
using MQ;
using TMPro;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject menuDialog;
    [SerializeField] private GameObject gameOverDialog;

    [SerializeField] private TextAsset content;
    [SerializeField] private SayDialog sayDialog;
    [SerializeField] private Character defaultCharacter;
    [SerializeField] private Inventory startingInventory;

    [SerializeField] private TextMeshProUGUI LoveLabel;
    [SerializeField] private TextMeshProUGUI HopeLabel;
    [SerializeField] private TextMeshProUGUI JoyLabel;
    [SerializeField] private TextMeshProUGUI PartsLabel;
    [SerializeField] private TextMeshProUGUI AppearancesLabel;

    private CharacterModel[] characterModels;
    private Character[] characters;
    private int currentIndex = 0;
    private Inventory playerInventory;

    private CharacterModel CurrentCharacterModel => characterModels[currentIndex];

    private void Awake()
    {
        characters = GetComponentsInChildren<Character>();

        var lines = content.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        characterModels = new CharacterModel[lines.Length - 1];

        for (var i = 1; i < lines.Length; ++i)
        {
            var data = lines[i].Split('\t');
            var characterModel = characterModels[i - 1] = new CharacterModel();
            characterModel.TriggerCondition = data[0];
            characterModel.CharacterId = data[1];
            characterModel.CharacterName = data[2];
            characterModel.CharacterDescription = data[3];
            characterModel.Desire = data[4];
            characterModel.ToyType = data[5];
            characterModel.QuandryDialog = data[6];

            int index = 7;
            characterModel.GiveEffect = ParseChoiceEffect(data, ref index);
            characterModel.ProposeEffect = ParseChoiceEffect(data, ref index);
            characterModel.IgnoreEffect = ParseChoiceEffect(data, ref index);
            characterModel.RecycleEffect = ParseChoiceEffect(data, ref index);
        }

        menuDialog.SetActive(false);
        gameOverDialog.SetActive(false);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        playerInventory = new Inventory(startingInventory);
        UpdateInventoryDisplay();
        ShowNextQuandry();
    }

    private static ChoiceEffect ParseChoiceEffect(IReadOnlyList<string> data, ref int i)
    {
        var choiceEffect = new ChoiceEffect();
        int.TryParse(data[i++], out choiceEffect.LoveEffect);
        int.TryParse(data[i++], out choiceEffect.HopeEffect);
        int.TryParse(data[i++], out choiceEffect.JoyEffect);
        int.TryParse(data[i++], out choiceEffect.PartsEffect);
        int.TryParse(data[i++], out choiceEffect.AppearancesEffect);
        return choiceEffect;
    }

    private void ShowNextQuandry()
    {
        currentIndex++;
        if (currentIndex >= characterModels.Length)
        {
            currentIndex = 0;
        }

        DisplayCharacter(CurrentCharacterModel.CharacterName);
        DisplayDialog(CurrentCharacterModel.QuandryDialog, DisplayChoices);
    }

    private void DisplayChoices()
    {
        menuDialog.SetActive(true);
    }

    public void OnClickedGive()
    {
        MakeChoiceWithEffect(CurrentCharacterModel.GiveEffect);
    }

    public void OnClickedPropose()
    {
        MakeChoiceWithEffect(CurrentCharacterModel.ProposeEffect);
    }

    public void OnClickedIgnore()
    {
        MakeChoiceWithEffect(CurrentCharacterModel.IgnoreEffect);
    }

    public void OnClickedRecycle()
    {
        MakeChoiceWithEffect(CurrentCharacterModel.RecycleEffect);
    }

    private void MakeChoiceWithEffect(ChoiceEffect choiceEffect)
    {
        playerInventory.AddChoiceEffect(choiceEffect);
        menuDialog.SetActive(false);
        UpdateInventoryDisplay();

        if (playerInventory.Love <= 0)
        {
            GameOverWithEnding("Love");
        }
        else if (playerInventory.Hope <= 0)
        {
            GameOverWithEnding("Hope");
        }
        else if (playerInventory.Joy <= 0)
        {
            GameOverWithEnding("Joy");
        }
        else if (playerInventory.Parts <= 0)
        {
            GameOverWithEnding("Parts");
        }
        else if (playerInventory.Appearances <= 0)
        {
            GameOverWithEnding("Appearances");
        }
        else
        {
            ShowNextQuandry();
        }
    }

    private void UpdateInventoryDisplay()
    {
        LoveLabel.text = playerInventory.Love.ToString();
        HopeLabel.text = playerInventory.Hope.ToString();
        JoyLabel.text = playerInventory.Joy.ToString();
        PartsLabel.text = playerInventory.Parts.ToString();
        AppearancesLabel.text = playerInventory.Appearances.ToString();
    }

    private void GameOverWithEnding(string ending)
    {
        var character = DisplayCharacter($"End_{ending}");
        gameOverDialog.SetActive(true);
        DisplayDialog(character.GetDescription(), null);
    }

    public void ReturnToMainMenu()
    {
        SceneLoader.LoadScene("MainMenu", Texture2D.blackTexture);
    }

    private Character DisplayCharacter(string characterName)
    {
        Character character = null;
        foreach (var c in characters)
        {
            if (c.NameText == characterName)
            {
                character = c;
                break;
            }
        }

        if (character == null)
        {
            character = defaultCharacter;
        }

        sayDialog.SetCharacter(character);
        sayDialog.SetCharacterImage(character.Portraits[0]);

        return character;
    }

    private void DisplayDialog(string dialog, Action onComplete)
    {
        sayDialog.Say(dialog, true, false, false, false, false, null, onComplete);
    }
}