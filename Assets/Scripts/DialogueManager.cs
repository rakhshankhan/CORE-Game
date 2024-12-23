using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using XNode;

public class DialogueManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public LanguageSO currentLanguage;

    public TextMeshProUGUI promptText;
    public List<Button> responseButtons;
    public TextMeshProUGUI endMessageText;  // Reference to the end message text

    private DialogueNode currentNode;

    void Start()
    {
        // Help from ChatGPT
        // Initialize the dialogue (assuming the first node is the entry point)
        if (dialogueTree.nodes.Count > 0)
        {
            currentNode = dialogueTree.nodes[0] as DialogueNode;
            DisplayCurrentNode();
            endMessageText.gameObject.SetActive(false);  // Hide end message at start
        }
    }

    void DisplayCurrentNode()
    {
        // Display the prompt text from the current node
        promptText.text = GetLocalizedText(currentNode.promptKey);

        // Hide the end message text if it was previously shown
        endMessageText.gameObject.SetActive(false);

        // List of response keys and corresponding output ports for easier handling
        List<string> responseKeys = new List<string>
        {
            currentNode.responseKey1,
            currentNode.responseKey2,
            currentNode.responseKey3,
            currentNode.responseKey4
        };

        List<NodePort> outputPorts = new List<NodePort>
        {
            currentNode.GetOutputPort("option1"),
            currentNode.GetOutputPort("option2"),
            currentNode.GetOutputPort("option3"),
            currentNode.GetOutputPort("option4")
        };

        // Display responses and set up button actions
        for (int i = 0; i < responseButtons.Count; i++)
        {
            if (i < responseKeys.Count && !string.IsNullOrEmpty(responseKeys[i]))
            {
                responseButtons[i].gameObject.SetActive(true);

                // Help from ChatGPT
                // Get the TextMeshProUGUI component from the button's child
                TextMeshProUGUI buttonText = responseButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = GetLocalizedText(responseKeys[i]);
                }
                else
                {
                    Debug.LogError("No TextMeshProUGUI component found on the button's child.");
                }

                // Clear any existing listeners and set new listeners for each button
                responseButtons[i].onClick.RemoveAllListeners();
                int responseIndex = i;  // Capture index for use in the listener
                responseButtons[i].onClick.AddListener(() => OnResponseSelected(outputPorts[responseIndex]));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);  // Hide unused buttons
            }
        }
    }

    void OnResponseSelected(NodePort outputPort)
    {
        // Proceed to the next node if a valid connection exists
        if (outputPort != null && outputPort.ConnectionCount > 0)
        {
            var nextNode = outputPort.Connection.node;

            if (nextNode is DialogueNode)
            {
                currentNode = nextNode as DialogueNode;
                DisplayCurrentNode();
            }
            else if (nextNode is DialogueControlNode)
            {
                HandleControlNode(nextNode as DialogueControlNode);
            }
            else
            {
                Debug.LogError("Unknown node type encountered.");
            }
        }
        else
        {
            Debug.Log("End of dialogue or no connection.");
            ShowEndMessage("Dialogue has ended.");
        }
    }

    void HandleControlNode(DialogueControlNode controlNode)
    {
        switch (controlNode.dialogueControl)
        {
            case DialogueControlNode.Option.EndDialogue:
                ShowEndMessage("Dialogue has ended. Please restart to try again.");  // Show end message when dialogue ends
                break;

            case DialogueControlNode.Option.RestartDialogue:
                currentNode = dialogueTree.nodes[0] as DialogueNode;
                DisplayCurrentNode();
                break;

            case DialogueControlNode.Option.ContinueDialogue:
                NodePort nextPort = controlNode.GetOutputPort("nextNode");
                if (nextPort != null && nextPort.ConnectionCount > 0)
                {
                    var nextDialogueNode = nextPort.Connection.node as DialogueNode;
                    if (nextDialogueNode != null)
                    {
                        currentNode = nextDialogueNode;
                        DisplayCurrentNode();
                    }
                    else
                    {
                        Debug.LogError("The next node is not a DialogueNode.");
                    }
                }
                else
                {
                    Debug.Log("No next node to continue to.");
                }
                break;

            default:
                Debug.LogError("Unhandled dialogue control option.");
                break;
        }
    }

    void ShowEndMessage(string message)
    {
        // Display the end message on the screen
        endMessageText.text = message;
        endMessageText.gameObject.SetActive(true);

        promptText.gameObject.SetActive(false);
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    string GetLocalizedText(string key)
    {
        if (currentLanguage.languageDictionary.TryGetValue(key, out string value))
        {
            return value;
        }
        else
        {
            return $"[Missing Text for '{key}']";  // Placeholder for missing localization
        }
    }
}
