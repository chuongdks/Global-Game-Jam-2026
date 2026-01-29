using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Rendering.MaterialUpgrader;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;

    private Queue<string> sentences = new Queue<string>();  // Dialogue queue
    public bool isDialogueActive { get; private set; }

    // Reference to Player script
    public PlayerMovement player;

    private string currentFullSentence;     // Tracks the full text of the current line
    private bool isTyping;                  // Tracks if the typewriter effect is running
    private NPCNavigation currentNPCNav;// Tracks current NPC

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Helper function: EmotionNPC use this, not DialogueManager (this is the starting point of the class somehow)
    public void StartDialogue(string[] lines, NPCNavigation npcNav)
    {
        isDialogueActive = true;
        player.enabled = false; // Freeze player
        dialoguePanel.SetActive(true);
        isTyping = false;

        // reference to NPC's navigation
        currentNPCNav = npcNav;

        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    // Update is called once per frame
    void Update()
    {
        // If dialogue is open and player presses E, show the next line
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // If text still typing, stop effect, show full text
                StopAllCoroutines();
                textComponent.text = currentFullSentence;
                isTyping = false;
            }
            else
            {
                // If not, move to next dialogue
                DisplayNextSentence();
            }            
        }
    }

    // Helper function: Display next sentence
    public void DisplayNextSentence()
    {
        // end dialogue and unfreeze player
        if (sentences.Count == 0)
        {            

            EndDialogue();
            return;
        }

        // store next sentence in currentFullSentence
        currentFullSentence = sentences.Dequeue();

        // Stop any current scrolling before starting a new one
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentFullSentence));
    }

    // Typewriter Effect 
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;   // Mark as finished so press E for next dialogue
    }

    // Helper function: At the End of the dialogue array
    void EndDialogue()
    {
        isDialogueActive = false;
        player.enabled = true; // Unfreeze player
        dialoguePanel.SetActive(false);

        // In final dialgoue, Manager can kick the NPC out now instead of NPC walk off while talking to worker
        if (currentNPCNav != null)
        {
            currentNPCNav.currentState = NPCNavigation.NPCState.Exiting;
            currentNPCNav = null; // Clear for the next person
        }
    }
}
