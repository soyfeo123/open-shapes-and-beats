using System;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;
}

[Serializable]
public class DialogueLine
{
    public string Line;
    public DialogueSpeaker Speaker;
    public bool Automatic;
    public float AutomaticDelay;
}
