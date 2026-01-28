using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed and Body")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;

    [Header("Detection Settings")]
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask interactMask; // Cheeck for Game object thorugh Mask (NPC, Mask,...)

    [Header("Current Holding")]
    [SerializeField] private Transform handPosition; // A child object of Player where the mask sits
    private GameObject currentHeldMask;

    [Header("UI Settings")]
    public GameObject nameTagPrefab;    // Blueprint Canvas prefab for UI above Object's head
    private GameObject currentUIAboveHead;       // Instance of each UI above Object's head

    private Collider2D currentHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Handle Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // set value to the Animator parameter
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical",   movement.y);
        animator.SetFloat("Speed",      movement.sqrMagnitude);

        // Check for Object Layer Mask for every frame the main character exist
        currentHit = Physics2D.OverlapCircle(
            transform.position,
            interactRange,
            interactMask
        );
        HandleNameTag();
        HandleInteractionInput();
    }

    // better for physics since no fps involve. Called every frame on a fixed timer
    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Handle interacting with Game Object
    void HandleInteractionInput()
    {
        int maskLayer = LayerMask.NameToLayer("Mask");
        int npcLayer = LayerMask.NameToLayer("NPC");

        if (Input.GetKeyDown(KeyCode.E) && currentHit != null)
        {
            if (currentHit.gameObject.layer == npcLayer)
            {
                currentHit.GetComponent<EmotionNPC>().Interact(currentHeldMask);
            }
            else if (currentHit.gameObject.layer == maskLayer)
            {
                TryPickUpMask(currentHit);
            }
        }
    }

    // Helper Function: Pick up the Object with mask layer
    private void TryPickUpMask(Collider2D maskCollider)
    {       
        // Check for Mask Layer nearby
        if (maskCollider != null)
        {
            // drop object (or destroy it) if already hold object
            if (currentHeldMask != null)
            {
                // Option A: destroy the old one
                Destroy(currentHeldMask);

                // Option B: Drop it at player position
                //currentHeldMask.transform.SetParent(null);
                //currentHeldMask.transform.position = transform.position;
                //currentHeldMask.GetComponent<Collider2D>().enabled = true;
                
            }

            // Pick up new mask
            currentHeldMask = Instantiate(maskCollider.gameObject);

            // Attach it to player's Hand Object
            currentHeldMask.transform.SetParent(handPosition);
            currentHeldMask.transform.localPosition = Vector3.zero;
            currentHeldMask.transform.localRotation = Quaternion.identity;

            // Disable mask's collider 
            currentHeldMask.GetComponent<Collider2D>().enabled = false;

            Debug.Log("Picked up: " + currentHeldMask.name);
        }
    }

    // Handle getting near object
    void HandleNameTag()
    {
        if (currentHit != null)
        {
            ShowNameTag(currentHit.transform);  // UI appear on top of the Mask Object
        }
        else
        {
            HideNameTag();
        }
    }

    // Helper function: Show the name of Game Object when near
    private void ShowNameTag(Transform target)
    {
        // add a specific mask here to see that name only
        if (currentUIAboveHead == null)
        {
            currentUIAboveHead = Instantiate(nameTagPrefab);
        }

        // Position UI text above mask object
        currentUIAboveHead.transform.position = target.position + new Vector3(0, 0.8f, 0);

        // Set the text to the mask's name
        TextMeshProUGUI tmpText = currentUIAboveHead.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = target.gameObject.name;
        }

        currentUIAboveHead.SetActive(true);
    }

    // Helper function: Hide name tag if not nearby
    private void HideNameTag()
    {
        if (currentUIAboveHead != null)
        {
            currentUIAboveHead.SetActive(false);
        }
    }

    // Visualizes the interaction range in the Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
