using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene {
        GameScene,
        Loading,
        MainMenu,
        GameSceneOutside,
        GameOver
    }

    // private variables
    private ShopManager shopManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
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
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Scene 1 (Shop), go to Scene 2 (Outside)
            if (currentSceneName == Scene.GameScene.ToString()) 
            {
                if (shopManager != null)
                {
                    PlayerPrefs.SetInt("Correct", shopManager.correctCount);
                    PlayerPrefs.SetInt("Wrong", shopManager.wrongCount);
                    PlayerPrefs.Save();
                }
                SceneManager.LoadScene(Scene.GameSceneOutside.ToString());
            }
            // Scene 2 go to GameOver scene
            else if (currentSceneName == Scene.GameSceneOutside.ToString())
            {
                SceneManager.LoadScene(Scene.GameOver.ToString());
            }
        }
    }
}
