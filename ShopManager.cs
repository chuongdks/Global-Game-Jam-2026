using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] npcPrefabs; // different NPCs prefabs 
    public Transform spawnPoint;
    public Transform stationPoint;
    public Transform exitPoint;

    private GameObject currentNPC;
    private int currentNPCIndex = 0; // Tracks NPC to spawn next

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnNextNPC();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnNextNPC()
    {
        // Pick a random NPC from the array
        // int randomIndex = Random.Range(0, npcPrefabs.Length);
        // currentNPC = Instantiate(npcPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Spawn NPC by order
        if (currentNPCIndex >= npcPrefabs.Length)
        {
            Debug.Log("No more customers for the day");
            return;
        }

        currentNPC = Instantiate(npcPrefabs[currentNPCIndex], spawnPoint.position, Quaternion.identity);

        // Assign the walk points to the NPC
        var nav = currentNPC.GetComponent<NPCNavigation>();
        nav.stationPoint = stationPoint;
        nav.exitPoint = exitPoint;
        currentNPCIndex++;
    }

    // This will be called by the NPC script after the transaction is done
    public void CustomerServed()
    {
        Invoke("SpawnNextNPC", 2f); // Wait 2 seconds before the next person arrives
    }
}
