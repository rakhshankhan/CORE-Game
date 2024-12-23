using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : Node
{
    [Input] public DialogueNode prevNode;

    // Localization key for the prompt text
    public string promptKey;

    // Response keys
    public string responseKey1;
    public string responseKey2;
    public string responseKey3;
    public string responseKey4;

    // Output ports for each response
    [Output] public DialogueNode option1;
    [Output] public DialogueNode option2;
    [Output] public DialogueNode option3;
    [Output] public DialogueNode option4;

    public override object GetValue(NodePort port)
    {
        return null;
    }
}
