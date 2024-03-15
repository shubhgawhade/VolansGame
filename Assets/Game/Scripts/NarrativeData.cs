using System;

[Serializable]
public class NarrativeData
{
    // string[] row;
    public int id;
    public string speaker;
    public bool hasChoice;
    public int numOfChoices;
    public string prereq;
    public string dialogue;
    public int anxietyLevel;
    public int linkId;
}