using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fungus;
using LitJson;
using MQ;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameState : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private TextAsset contentJson;

    [Header("Game")] [SerializeField] private GameObject menuDialog;
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
    public static Inventory playerInventory;
    private Random rnd;

    private CharacterModel CurrentCharacterModel => characterModels[currentIndex];

    private void Awake()
    {
        var playerName = string.IsNullOrWhiteSpace(GetName.PlayerName) ? "Quandary" : GetName.PlayerName;
        JsonData json = JsonMapper.ToObject(contentJson.text);
        var characterModelList = new List<CharacterModel>();
        foreach (JsonData characterJson in json)
        {
            var characterModel = new CharacterModel();
            characterModel.ParseJson(characterJson, playerName);
            characterModelList.Add(characterModel);
        }

        characterModels = characterModelList.ToArray();
        characters = GetComponentsInChildren<Character>();
        rnd = new Random(GetName.NameSeed);
        rnd.Shuffle(characterModels);
        menuDialog.SetActive(false);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        playerInventory = new Inventory(startingInventory);
        UpdateInventoryDisplay();
        ShowNextQuandary();
    }
    
    private void ShowNextQuandary()
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
            ShowNextQuandary();
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
        var character = GetCharacterByName($"End_{ending}");
        GameOver.GameOverText = character.GetDescription();
        SceneLoader.LoadScene("GameOver", Texture2D.blackTexture);
    }

    private Character GetCharacterByName(string characterName)
    {
        return characters.FirstOrDefault(c => c.NameText == characterName);
    }

    private Character DisplayCharacter(string characterName)
    {
        Character character = GetCharacterByName(characterName);

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
    
    public void ReturnToMainMenu()
    {
        var menuUi = GameObject.Find("Menu UI");
        if (menuUi != null)
        {
           Destroy(menuUi);
        }
        SceneLoader.LoadScene("MainMenu", Texture2D.blackTexture);
    }
}