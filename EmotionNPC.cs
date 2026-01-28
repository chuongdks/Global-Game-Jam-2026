using UnityEngine;

public class EmotionNPC : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Sad Villager";

    [Header("Dialogues")]
    public string[] introDialogues;     // initial conversation lines
    public string[] successDialogue;    // Right Mask Dialogues
    public string[] wrongMaskDialogue;  // Wrong Mask Dialogues

    [Header("Win Condition")]
    public string correctMaskName;
    public int goldReward;

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
        // check if player is bringing a masks
        if (heldItem != null)
        {
            CheckMask(heldItem);
        }
        else
        {
            // If hand is empty, start normal conversation
            manager.StartDialogue(introDialogues);
        }
    }

    private void CheckMask(GameObject heldItem)
    {
        if (heldItem.name.Contains(correctMaskName))
        {
            // Show success in console (or trigger a special success dialogue line)
            //Debug.Log($"{npcName}: {successDialogue}");
            //Debug.Log($"You earned {goldReward} gold!");
            manager.StartDialogue(successDialogue);
            Destroy(heldItem);
        }
        else
        {
            // Debug.Log($"{npcName}: {wrongMaskDialogue}");
            manager.StartDialogue(wrongMaskDialogue);
            Destroy(heldItem);
        }
    }
}
