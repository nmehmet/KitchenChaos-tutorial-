using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    

    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button OptionsButton;

    private void Awake()
    {
        ResumeButton.onClick.AddListener(() => ResumeGame());
        MainMenuButton.onClick.AddListener(() => GotoMainMenu());
        OptionsButton.onClick.AddListener(() => GoToOptions());
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    public void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    public void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
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
    private void GoToOptions()
    {
        OptionsUI.Instance.Show();
        //Hide();
    }
}
