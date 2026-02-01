using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene {
        GameScene,
        Loading,
        MainMenu,
        GameSceneOutside
    }

    // attach to button to load into any scene
    public void load() {
        SceneManager.LoadScene(Scene.Loading.ToString());
        SceneManager.LoadScene(Scene.GameScene.ToString());       
    }

    // attach to a Collider for player to transition scebe
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Scene.GameSceneOutside.ToString());
        }
    }
}
