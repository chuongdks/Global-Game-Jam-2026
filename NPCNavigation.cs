using UnityEngine;

public class NPCNavigation : MonoBehaviour
{
    public enum NPCState { Entering, Waiting, Exiting }
    public NPCState currentState = NPCState.Entering;

    [Header("NPC Speed")]
    private Transform stationPoint;
    private Transform exitPoint;
    public float walkSpeed = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == NPCState.Entering && stationPoint != null)
        {
            MoveTowards(stationPoint.position);
            if (Vector2.Distance(transform.position, stationPoint.position) < 0.1f)
            {
                currentState = NPCState.Waiting;
            }
                
        }
        else if (currentState == NPCState.Exiting && exitPoint != null)
        {
            MoveTowards(exitPoint.position);
            if (Vector2.Distance(transform.position, exitPoint.position) < 0.1f)
            {
                // Destroy(gameObject);
            }
        }
        else if (currentState == NPCState.Waiting)
        {
            // problaby do something for Scene 2 cuz NPC has already spawned in
        }
    }

    void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, walkSpeed * Time.deltaTime);
        // Add animator if scope allow
        // animator.SetFloat("Speed", 1);
    }

    // Helper function: ShopManager script give destination points to the NPC
    public void SetupPath(Transform station, Transform exit)
    {
        stationPoint = station;
        exitPoint = exit;
    }
}
