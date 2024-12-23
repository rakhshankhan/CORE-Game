using UnityEngine;
using XNode;

public class DialogueControlNode : Node
{
    public enum Option { EndDialogue, ContinueDialogue, RestartDialogue }
    public Option dialogueControl;

    [Input] public DialogueNode prevNode;

    // For ContinueDialogue, we need an output port
    [Output] public DialogueNode nextNode;

    public override object GetValue(NodePort port)
    {
        return null;
    }
}
