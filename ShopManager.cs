using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public GameObject[] npcPrefabs;     // different NPCs prefabs 
    public Transform spawnPoint;
    public Transform stationPoint;
    public Transform exitPoint;

    private GameObject currentNPC;
    private int currentNPCIndex = 0;    // Tracks NPC to spawn next

    [Header("Score Tracking")]
    public int correctCount = 0;
    public int wrongCount = 0;

    [Header("Exit Settings")]
    public GameObject exitBlocker;      // Door Object blocking the shop exit
    public GameObject exitVisualPrompt; // Visual indicator for player to see the exit after finishing job

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // reset PlayerPrefs for the NPC's mask
        for (int i = 0; i < npcPrefabs.Length; i++) 
        {
            string key = "NPC_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                Debug.Log("Reset mask for: " + key);
            }
        }
        PlayerPrefs.Save();

        SpawnNextNPC();
    }

    // Update is called once per frame
    void Update()
    {
        // Pressing R and Delete to resets the game
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Data Cleared!");
            // Optional: Reload the scene to see the NPCs lose their masks instantly
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void SpawnNextNPC()
    {
        // Pick a random NPC from the array
        //int randomIndex = Random.Range(0, npcPrefabs.Length);
        //currentNPC = Instantiate(npcPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Spawn NPC by order
        if (currentNPCIndex >= npcPrefabs.Length)
        {
            Debug.Log("No more customers for the day");
            OpenShopExit();
            // ShowGameOver();
            return;
        }

        currentNPC = Instantiate(npcPrefabs[currentNPCIndex], spawnPoint.position, Quaternion.identity);

        // Assign the walk points to the NPC
        var nav = currentNPC.GetComponent<NPCNavigation>();
        nav.SetupPath(stationPoint, exitPoint);
        currentNPCIndex++;
    }

    // called by emotionNPC script after transaction is done
    public void CustomerServed()
    {
        Invoke("SpawnNextNPC", 2f); // Wait 2 seconds before the next person arrives
    }

    // record the correct and wrong mask given
    public void RecordResult(bool isCorrect)
    {
        if (isCorrect)
        {
            correctCount++;
            Debug.Log("Score: " + correctCount + " Correct");
        }
        else
        { 
            wrongCount++;
            Debug.Log("Score: " + wrongCount + " Wrong");
        }
    }

    // 
    void OpenShopExit()
    {
        if (exitBlocker != null)
        {
            // disable the door object
            exitBlocker.SetActive(false);

            // Future scope: Trigger door open animation 
            // exitBlocker.GetComponent<Animator>().SetTrigger("Open");
        }

        if (exitVisualPrompt != null)
        {
            exitVisualPrompt.SetActive(true); // Show the player where to go
        }
    }

    // save score and show game over scene
    void ShowGameOver()
    {
        // Store stats in a static class or PlayerPrefs to read them in the next scene
        PlayerPrefs.SetInt("Correct", correctCount);
        PlayerPrefs.SetInt("Wrong", wrongCount);
        SceneManager.LoadScene("GameOver");
    }
}
