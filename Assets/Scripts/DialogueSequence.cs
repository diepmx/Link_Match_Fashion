// DialogueSequence.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Story/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines;

}

[System.Serializable]
public class DialogueLine
{
    public string speakerId;
    [TextArea] public string text;
    public Sprite portrait;
    public Sprite background;
}

