using UnityEngine;

public class EmotionNPC : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName;

    [Header("Dialogues")]
    public string[] introDialogues;     // initial conversation lines
    public string[] successDialogue;    // Right Mask Dialogues
    public string[] wrongMaskDialogue;  // Wrong Mask Dialogues

    [Header("Win Condition")]
    public string correctMaskName;
    public int goldReward;

    private DialogueManager manager;
    private ShopManager shop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindFirstObjectByType<DialogueManager>();
        shop = FindFirstObjectByType<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is the version called by PlayerInteraction.cs
    public void Interact(GameObject heldItem)
    {
        // check if player is bringing a masks
        if (heldItem != null)
        {
            CheckMask(heldItem);
        }
        else
        {
            // If hand is empty, start normal conversation
            manager.StartDialogue(introDialogues, null);
        }
    }

    // Check every mask transaction and give the correct response
    private void CheckMask(GameObject heldItem)
    {
        // Get reference to the navigation script and Shop Manager
        NPCNavigation nav = GetComponent<NPCNavigation>();
        
        // Position the mask on the NPC's face
        Transform faceSocket = transform.Find("FaceSocket");
        if (faceSocket != null)
        {
            heldItem.transform.SetParent(faceSocket);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }

        // check what mask the player gave to NPC and respond
        if (heldItem.name.Contains(correctMaskName))
        {
            // Show success in console (or trigger a special success dialogue line)
            //Debug.Log($"{npcName}: {successDialogue}");   //Debug.Log($"You earned {goldReward} gold!");
            manager.StartDialogue(successDialogue, nav);    // Pass 'nav' so manager tell THIS npc to leave the store
            shop.RecordResult(true); // Record transaction good
        }
        else
        {
            // Debug.Log($"{npcName}: {wrongMaskDialogue}");
            manager.StartDialogue(wrongMaskDialogue, nav);  // Pass 'nav' so manager tell THIS npc to leave the store
            shop.RecordResult(false); // Record transaction bad
        }

        // Cleanup and notify ShopManager
        shop.CustomerServed();  // Tell manager to prep next NPC
    }
}
