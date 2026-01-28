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
    public PlayerMovement playerMovement;

    private string currentFullSentence;     // Tracks the full text of the current line
    private bool isTyping;                  // Tracks if the typewriter effect is running

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Helper function: EmotionNPC use this, not DialogueManager (this is the starting point function)
    public void StartDialogue(string[] lines)
    {
        isDialogueActive = true;
        playerMovement.enabled = false; // Freeze player
        dialoguePanel.SetActive(true);
        isTyping = false;

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

    public void DisplayNextSentence()
    {
        // end dialogue and unfreeze player
        if (sentences.Count == 0)
        {            
            isDialogueActive = false;
            playerMovement.enabled = true; // Unfreeze player
            dialoguePanel.SetActive(false);
            return;
        }

        // store next sentence in currentFullSentence
        currentFullSentence = sentences.Dequeue();
        Debug.Log(currentFullSentence);

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
}
