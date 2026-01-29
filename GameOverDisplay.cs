using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI wrongText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctText.text = "Satisfied Customer: " + PlayerPrefs.GetInt("Correct");
        wrongText.text = "Wrong Masks: " + PlayerPrefs.GetInt("Wrong");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
