using UnityEngine;

public class NPCNavigation : MonoBehaviour
{
    public enum NPCState { Entering, Waiting, Exiting }
    public NPCState currentState = NPCState.Entering;

    [Header("NPC Speed")]
    public Transform stationPoint;
    public Transform exitPoint;
    public float walkSpeed = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == NPCState.Entering)
        {
            MoveTowards(stationPoint.position);
            if (Vector2.Distance(transform.position, stationPoint.position) < 0.1f)
            {
                currentState = NPCState.Waiting;
            }
                
        }
        else if (currentState == NPCState.Exiting)
        {
            MoveTowards(exitPoint.position);
            if (Vector2.Distance(transform.position, exitPoint.position) < 0.1f)
            {
                // Destroy(gameObject);
            }
        }
    }

    void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, walkSpeed * Time.deltaTime);
        // Add animator logic here later 
        // animator.SetFloat("Speed", 1);
    }
}
