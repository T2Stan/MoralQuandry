using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text GameOverTextLabel;

    public static string GameOverText;

    private void Awake()
    {
        GameOverTextLabel.text = GameOverText;
    }
    
    public void ReturnToMainMenu()
    {
        SceneLoader.LoadScene("MainMenu", Texture2D.blackTexture);
    }
}