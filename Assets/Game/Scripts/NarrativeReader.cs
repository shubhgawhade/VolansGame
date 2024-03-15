using System;
using System.Text;
using UnityEngine;

//FORMAT
// ID, SPEAKER, HAS CHOICE, NUM OF CHOICES, PREREQ, DIALOGUE, ANXIETY LEVEL, LINK ID

public class NarrativeReader : MonoBehaviour
{
    public TextAsset narrative;

    private StringBuilder fullstring;
    
    public string[] rows;
    
    [Serializable]
    public class DataLink
    {
        public string rawData;
        public NarrativeData narrativeData;
    }

    public DataLink[] dataLink;

    private int numOfRows;

    public string[] dialogue;
    
    void Awake()
    {
        fullstring = new StringBuilder(narrative.text);
        rows = fullstring.ToString().Split("\n");

        dataLink = new DataLink[rows.Length];
        for (int row = 0; row < rows.Length; row++)
        {
            NarrativeData tempNarrativeData = new NarrativeData();

            print($"Line {row} : {rows[row]}");

            
            for (int column = 0; column < 8; column++)
            {
                switch (column)
                {
                    case 0:

                        string rawId = rows[row].Split(",")[column];

                        if (rawId == "")
                        {
                            print($"Line {row} is NOT CUSTOM Linked");

                            tempNarrativeData.id = -1;
                        }
                        else
                        {
                            print($"Line {row} is CUSTOM Linked");
                            
                            tempNarrativeData.id = int.Parse(rawId);
                        }
                        
                        break;
                    
                    
                    case 1:
                        
                        string rawSpeaker = rows[row].Split(",")[column];
                        
                        if (rawSpeaker == "")
                        {
                            Debug.LogWarning($"Line {row} doesn't have a speaker");
                        }
                        else
                        {
                            tempNarrativeData.speaker = rawSpeaker;
                        }
                        
                        break;
                    
                    
                    case 2:
                        
                        bool rawHasChoice = ReturnHasChoice(int.Parse(rows[row].Split(",")[column]));

                        tempNarrativeData.hasChoice = rawHasChoice;

                        if (rawHasChoice)
                        {
                            // This line has choices. Check the next column for how many choices exist.
                        }
                        else
                        {
                            column = 4-1;
                        }
                        
                        break; 
                    
                    
                    case 3:
                        
                        string rawNumOfChoices = rows[row].Split(",")[column];

                        if (rawNumOfChoices == "")
                        {
                            Debug.LogWarning($"Line {row} doesn't have num of choices");
                        }
                        else
                        {
                            // The number of choices is the next n number of rows as dialogue options
                            
                            tempNarrativeData.numOfChoices = int.Parse(rawNumOfChoices);
                            column = 7-1;
                        }
                        
                        break;
                    
                    
                    case 4:
                    
                        string rawPrereq = rows[row].Split(",")[column];

                        tempNarrativeData.prereq = rawPrereq;
                        
                        break;

                    case 5:

                        string rawRowData = rows[row];
                        string rawDialogue = null;
                        int counter = 0;

                        for (int i = 0; i < rawRowData.Length; i++)
                        {
                            if (rawRowData[i].ToString() == ",")
                            {
                                counter++;
                            }

                            // Read from the 5th column
                            if (counter == column)
                            {
                                int doubleQuotes = 0;
                                for (int j = i + 1; j < rawRowData.Length; j++)
                                {
                                    if (rawRowData[j].ToString() == "\"")
                                    {
                                        doubleQuotes++;
                                        j++;
                                    }

                                    // Start of dialogue
                                    if (doubleQuotes == 1)
                                    {
                                        rawDialogue += rawRowData[j].ToString();
                                    }
                                    // End of dialogue
                                    else if(doubleQuotes == 2)
                                    {
                                        
                                        break;
                                    }
                                    
                                    // print(rawRowData[j]);
                                }
                                
                                print(rawDialogue);
                                if (rawDialogue == null)
                                {
                                    rawDialogue = rows[row].Split(",")[column];
                                    
                                    print($"Line {row} : DIALOGUE DOESN'T HAVE DOUBLE QUOTES {rawDialogue}");
                                    
                                    tempNarrativeData.dialogue = rawDialogue;
                                }
                                else
                                {
                                    print($"Line {row} : DIALOGUE: {rawDialogue}");
                                    tempNarrativeData.dialogue = rawDialogue;
                                }
                                
                                break;
                            } 
                        }
                            
                        break;
                    
                        
                    case 6:

                        string rawAnxietyLevel = rows[row].Split(",")[column];

                        int.TryParse(rawAnxietyLevel, out tempNarrativeData.anxietyLevel);
                            
                        break;
                    

                    case 7:
                        
                        // Next dialogue link ID
                        string rawlinkID = rows[row].Split(",")[column];

                        if (int.TryParse(rawlinkID, out tempNarrativeData.linkId))
                        {
                            print($"Line {row} : IS LINKED TO LINE {tempNarrativeData.linkId}");
                        }
                        else
                        {
                            print($"Line {row} : IS LINKED TO THE NEXT LINE");
                            tempNarrativeData.linkId = -1;
                        }

                        // if (tempNarrativeData.linkId == 0)
                        // {
                        //     tempNarrativeData.linkId = -1;
                        // }
                        
                        break;
                }
            }
            
            // NarrativeData tempNarrativeData = new NarrativeData
            // {
            //     id = int.Parse(rows[i].Split(",")[0]),
            //     speaker = rows[i].Split(",")[1],
            //     hasChoice = ReturnHasChoice(int.Parse(rows[i].Split(",")[2])),
            //     numOfChoices = int.Parse(rows[i].Split(",")[3]),
            //     prereq = rows[i].Split(",")[4],
            //     dialogue = rows[i].Split(",")[5],
            //     anxietyLevel = int.Parse(rows[i].Split(",")[6]),
            //     linkId = int.Parse(rows[i].Split(",")[7])
            // };

            dataLink[row] = new DataLink
            {
                rawData = rows[row],
                narrativeData = tempNarrativeData
            };
        }
    }

    public bool ReturnHasChoice(int choice)
    {
        if (choice == 1)
        {
            return true;
        }

        return false;
    }
}
