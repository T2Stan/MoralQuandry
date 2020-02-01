﻿using System.Collections.Generic;
using Fungus;
using MQ;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private TextAsset content;
    [SerializeField] private Flowchart flowchart;
    [SerializeField] private SayDialog sayDialog;
    
    private CharacterModel[] characterModels;
    private Character[] characters;
    private int currentIndex = 0;

    private void Awake()
    {
        characters = GetComponents<Character>();
        
        var lines = content.text.Split('\n');
        characterModels = new CharacterModel[lines.Length - 1];

        for (var i = 1; i < lines.Length; ++i)
        {
            var data = lines[i].Split('\t');
            var characterModel = characterModels[i-1] = new CharacterModel();
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

        Debug.Log(JsonUtility.ToJson(characterModels));

        SetNextCharacter();
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

    private void SetNextCharacter()
    {
        if (currentIndex >= characterModels.Length)
        {
            currentIndex = 0;
        }
    }
}