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
        var menuUi = GameObject.Find("Menu UI");
        if (menuUi != null)
        {
            Destroy(menuUi);
        }
        SceneLoader.LoadScene("MainMenu", Texture2D.blackTexture);
    }
}