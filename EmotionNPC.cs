using UnityEngine;

public class EmotionNPC : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Sad Villager";

    [Header("Dialogues")]
    public string[] dialogueLines; // The initial conversation lines
    public string successDialogue = "Oh! This Joy mask is exactly what I needed. Thank you!";
    public string wrongMaskDialogue = "Hmm. This Doesnt feels right. But you'll figure it out!";

    [Header("Win Condition")]
    public string correctMaskName = "Mask_Confuse(Clone)";
    public int goldReward = 50;

    private DialogueManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindFirstObjectByType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is the version called by PlayerInteraction.cs
    public void Interact(GameObject heldItem)
    {
        // check if player is bringing a masksd
        if (heldItem != null)
        {
            CheckMask(heldItem);
        }
        else
        {
            // If hand is empty, start normal conversation
            manager.StartDialogue(dialogueLines);
        }
    }

    private void CheckMask(GameObject heldItem)
    {
        if (heldItem.name.Contains(correctMaskName))
        {
            // Show success in console (or trigger a special success dialogue line)
            Debug.Log($"{npcName}: {successDialogue}");
            Debug.Log($"You earned {goldReward} gold!");
            Destroy(heldItem);
        }
        else
        {
            Debug.Log($"{npcName}: {wrongMaskDialogue}");
        }
    }
}
