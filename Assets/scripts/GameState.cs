using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using MQ;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameState : MonoBehaviour
{
    [SerializeField] private TextAsset content;

    [Header("Game")] [SerializeField] private GameObject menuDialog;
    [SerializeField] private GameObject gameOverDialog;
    [SerializeField] private Inventory startingInventory;

    [Header("Fungus Hookups")] [SerializeField]
    private SayDialog sayDialog;

    [Header("Labels")] [SerializeField] private Character defaultCharacter;
    [SerializeField] private Text textLabel;
    [SerializeField] private Text characterDescription;
    [SerializeField] private Text characterDesire;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text loveLabel;
    [SerializeField] private Text hopeLabel;
    [SerializeField] private Text joyLabel;
    [SerializeField] private Text partsLabel;
    [SerializeField] private Text appearancesLabel;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource GiveSound;
    [SerializeField] private AudioSource IgnoreSound;
    [SerializeField] private AudioSource ProposeSound;
    [SerializeField] private AudioSource RecycleSound;


    private CharacterModel[] characterModels;
    private Character[] characters;
    private int currentIndex = -1;
    private Inventory playerInventory;
    private Random rnd;

    private CharacterModel CurrentCharacterModel => characterModels[currentIndex];

    private void Awake()
    {
        characters = GetComponentsInChildren<Character>();

        var lines = content.text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
        characterModels = new CharacterModel[lines.Length - 1];
        
        var playerName = string.IsNullOrWhiteSpace(GetName.PlayerName) ? "Quandary" : GetName.PlayerName;

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

            var quandaryDialog = data[6].Replace("{$PlayerName}", playerName);
            string[] separator = {"  "};
            characterModel.QuandaryDialogQueue = quandaryDialog.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            int index = 7;
            characterModel.GiveEffect = ParseChoiceEffect(data, ref index);
            characterModel.ProposeEffect = ParseChoiceEffect(data, ref index);
            characterModel.IgnoreEffect = ParseChoiceEffect(data, ref index);
            characterModel.RecycleEffect = ParseChoiceEffect(data, ref index);
        }

        rnd = new Random(GetName.NameSeed);
        rnd.Shuffle(characterModels);
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
        QueueDialog(CurrentCharacterModel.QuandaryDialogQueue, DisplayChoices);
    }

    private void DisplayChoices()
    {
        menuDialog.SetActive(true);
    }

    public void OnClickedGive()
    {
        menuDialog.SetActive(false);
        GiveSound.Play();
        StartCoroutine(Wait(GiveSound.clip.length, CurrentCharacterModel.GiveEffect));
    }

    private IEnumerator Wait(float time, ChoiceEffect effect)
    {
        yield return new WaitForSeconds(time);
        MakeChoiceWithEffect(CurrentCharacterModel.GiveEffect);
    }

    public void OnClickedPropose()
    {
        menuDialog.SetActive(false);
        ProposeSound.Play();
        StartCoroutine(Wait(ProposeSound.clip.length, CurrentCharacterModel.ProposeEffect));
    }

    public void OnClickedIgnore()
    {
        menuDialog.SetActive(false);
        IgnoreSound.Play();
        StartCoroutine(Wait(IgnoreSound.clip.length, CurrentCharacterModel.IgnoreEffect));
    }

    public void OnClickedRecycle()
    {
        menuDialog.SetActive(false);
        RecycleSound.Play();
        StartCoroutine(Wait(RecycleSound.clip.length, CurrentCharacterModel.RecycleEffect));
    }

    private void MakeChoiceWithEffect(ChoiceEffect choiceEffect)
    {
        playerInventory.AddChoiceEffect(choiceEffect);
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
        scoreLabel.text = playerInventory.Score.ToString();
        loveLabel.text = playerInventory.Love.ToString();
        hopeLabel.text = playerInventory.Hope.ToString();
        joyLabel.text = playerInventory.Joy.ToString();
        partsLabel.text = playerInventory.Parts.ToString();
        appearancesLabel.text = playerInventory.Appearances.ToString();
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

        characterDescription.text = CurrentCharacterModel.CharacterDescription;
        characterDesire.text = CurrentCharacterModel.Desire;
        sayDialog.SetCharacter(character);
        if (character.Portraits.Count > 0)
        {
            sayDialog.SetCharacterImage(character.Portraits[0]);
        }

        AudioSource audio = character.GetComponent<AudioSource>();
        audio.Play();

        return character;
    }

    private void QueueDialog(string[] dialogQueue, Action onComplete, int index = 0)
    {
        if (dialogQueue.Length == index)
        {
            onComplete?.Invoke();
        }
        else
        {
            DisplayDialog(dialogQueue[index], () => { QueueDialog(dialogQueue, onComplete, index + 1); },
                index == dialogQueue.Length - 1);
        }
    }

    private void DisplayDialog(string dialog, Action onComplete, bool italic = false)
    {
        textLabel.fontStyle = italic ? FontStyle.Italic : FontStyle.Normal;
        sayDialog.Say(dialog, true, true, false, true, true, null, onComplete);
    }
}