using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene {
        GameScene,
        Loading,
        MainMenu
    }

    public void load() {
        SceneManager.LoadScene(Scene.Loading.ToString());
        SceneManager.LoadScene(Scene.GameScene.ToString());       
    }
}
