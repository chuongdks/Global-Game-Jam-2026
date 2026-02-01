using System.Diagnostics;
using UnityEngine;

public class EmotionNPC : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName;

    [Header("Dialogues")]
    public string[] introDialogues;         // initial conversation lines
    public string[] successDialogue;        // Right Mask Dialogues
    public string[] wrongMaskDialogue;      // Wrong Mask Dialogues
    public string[] maskedSuccessGreeting;  // Scene 2 NPC with Correct Mask dialogues
    public string[] maskedWrongGreeting;    // Scene 2 NPC with Wrong Mask dialogues

    [Header("Win Condition")]
    public string correctMaskName;
    public int goldReward;

    public string uniqueID;             // Give each NPC a name 

    // private variables
    private DialogueManager dialogueManager;
    private ShopManager shopManager;
    private bool hasIntroduced = false;     // Tracks if Intro dialogues has played
    private bool hasMaskEquipped = false;   // Tracks if NPC has a mask on Start
    private string savedMaskName; // Stores the name of the mask loaded

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        shopManager = FindFirstObjectByType<ShopManager>();

        // Check if this NPC already has a mask saved
        if (PlayerPrefs.HasKey(uniqueID))
        {
            savedMaskName = PlayerPrefs.GetString(uniqueID);
            LoadSavedMask(savedMaskName);
            hasMaskEquipped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called by PlayerMovement script
    public void Interact(GameObject heldItem)
    {
        // check If NPCs already has a mask
        if (hasMaskEquipped)
        {
            // Compare the name of the mask they are wearing to the 'correct' one
            if (savedMaskName.Contains(correctMaskName))
            {
                dialogueManager.StartDialogue(maskedSuccessGreeting, null, false);
            }
            else
            {
                dialogueManager.StartDialogue(maskedWrongGreeting, null, false);
            }
            return;
        }

        // trigger intro Dialogue when first time talking to the NPC
        if (!hasIntroduced)
        {
            dialogueManager.StartDialogue(introDialogues, null, false);
            hasIntroduced = true; 
            return;
        }

        // check if player is bringing a masks
        if (heldItem != null)
        {
            CheckMask(heldItem);
        }
        else
        {
            // If hand is empty, start Intro Dialogue
            dialogueManager.StartDialogue(introDialogues, null, false);
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

            // Tell PlayerMovement scriptto clear currentHeldMask
            FindFirstObjectByType<PlayerMovement>().ClearHeldMask();
        }

        // Save the mask choice permanently
        UnityEngine.Debug.Log($"Saving {heldItem.name} to NPC {uniqueID}");
        PlayerPrefs.SetString(uniqueID, heldItem.name);
        PlayerPrefs.Save();

        // check what mask the player gave to NPC and respond
        if (heldItem.name.Contains(correctMaskName))
        {
            // Show success in console (or trigger a special success dialogue line)
            dialogueManager.StartDialogue(successDialogue, nav, true);    // Pass 'nav' so dialogueManager tell THIS npc to leave the store
            shopManager.RecordResult(true); // Record transaction good
        }
        else
        {
            dialogueManager.StartDialogue(wrongMaskDialogue, nav, true);  // Pass 'nav' so dialogueManager tell THIS npc to leave the store
            shopManager.RecordResult(false); // Record transaction bad
        }
        // Game idea: SPecific Mask trigger a different dialogues
        //else if(heldItem.name.Contains(weirdMaskName))
        //{
        //    // Debug.Log($"{npcName}: {wrongMaskDialogue}");
        //    dialogueManager.StartDialogue(differentDialogue, nav);  // Pass 'nav' so dialogueManager tell THIS npc to leave the store
        //    shopManager.RecordResult(false); // Record transaction bad
        //}


        // Cleanup and notify ShopManager
        // shopManager.CustomerServed();  // Tell dialogueManager to prep next NPC
    }

    // Helper function: Load the Mask onto NPC face on spawn
    void LoadSavedMask(string maskName)
    {
        UnityEngine.Debug.Log($"NPC {uniqueID} is attempting to load mask: {maskName}");

        GameObject maskPrefab = Resources.Load<GameObject>(maskName); 
        if (maskPrefab != null)
        {
            GameObject newMask = Instantiate(maskPrefab);
            Transform faceSocket = transform.Find("FaceSocket");
            newMask.transform.SetParent(faceSocket);
            newMask.transform.localPosition = Vector3.zero;
            newMask.GetComponent<Collider2D>().enabled = false;
            UnityEngine.Debug.Log("Mask loaded successfully!");
        }
        else
        {
            UnityEngine.Debug.LogError($"Could not find mask prefab named {maskName} in Resources folder!");
        }
    }
}
