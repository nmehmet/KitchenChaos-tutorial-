using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ResumeButton;

    private void Awake()
    {
        ResumeButton.onClick.AddListener(() => ResumeGame());
        MainMenuButton.onClick.AddListener(() => GotoMainMenu());
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void GotoMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }
    private void ResumeGame()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }
}
