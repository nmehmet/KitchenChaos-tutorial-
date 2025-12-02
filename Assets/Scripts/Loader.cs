using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        SampleScene,
        LoadingScene
    }

    public static Scene targetScene;

    public static void Load(Scene _targetScene)
    {
        Loader.targetScene = _targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
