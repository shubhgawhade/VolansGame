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
    
    public int extraCommaCounter;
    
    void Awake()
    {
        // Load the CSV file as a string
        fullstring = new StringBuilder(narrative.text);
        rows = fullstring.ToString().Split("\n");

        dataLink = new DataLink[rows.Length];
        
        // Go through each row
        for (int row = 0; row < rows.Length; row++)
        {
            NarrativeData tempNarrativeData = new NarrativeData();

            print($"Line {row} : {rows[row]}");
            
            // Loop through the 8 columns in each row
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

                        string rawPrereq;
                        string rawRowData1 = rows[row];
                        
                        // Gets the position of the character for the current column
                        int characterPosition1 = GetColumnCharacterIndex(rawRowData1, column);
                        
                        if (characterPosition1 == -1)
                        {
                            Debug.LogWarning("CHARACTER POSITION FOR COLUMN NOT FOUND");
                            
                            return;
                        }

                        // READ DATA IF THE FIRST CHARACTER IS NOT DOUBLE QUOTES
                        // If there aren't double quotes, read data from this column
                        if (rawRowData1[characterPosition1 + 1].ToString() != "\"")
                        {
                            rawPrereq = rows[row].Split(",")[column];
                                    
                            print($"Line {row} : PREREQ DOESN'T HAVE DOUBLE QUOTES {rawPrereq}");
                                    
                            tempNarrativeData.prereq = rawPrereq;
                            break;
                        }
                        
                        // If there are enclosed in double quotes, the column data contains a comma. return the text
                        // Every column after this function needs to account for any extra commas in the row
                        // extraCommaCounter must be added to the column value
                        rawPrereq = ReturnTextFromQuotes(rawRowData1, characterPosition1, column);
                        
                        print($"Line {row} : PREREQ: {rawPrereq}");
                        tempNarrativeData.prereq = rawPrereq;
                        
                        break;

                    case 5:

                        string rawRowData = rows[row];
                        string rawDialogue;

                        // Gets the position of the character for the current column
                        // The previous value might've had commas enclosed in double quotes
                        // extraCommaCounter accounts for the extra commas to find the right character location
                        int characterPosition = GetColumnCharacterIndex(rawRowData, column + extraCommaCounter);
                        
                        if (characterPosition == -1)
                        {
                            Debug.LogWarning("CHARACTER POSITION FOR COLUMN NOT FOUND");
                            
                            return;
                        }

                        // READ DATA IF THE FIRST CHARACTER IS NOT DOUBLE QUOTES
                        if (rawRowData[characterPosition + 1].ToString() != "\"")
                        {
                            rawDialogue = rows[row].Split(",")[column + extraCommaCounter];
                                    
                            print($"Line {row} : DIALOGUE DOESN'T HAVE DOUBLE QUOTES {rawDialogue}");
                                    
                            tempNarrativeData.dialogue = rawDialogue;
                            break;
                        }
                        
                        rawDialogue = ReturnTextFromQuotes(rawRowData, characterPosition, column+extraCommaCounter);
                        
                        print($"Line {row} : DIALOGUE: {rawDialogue}");
                        tempNarrativeData.dialogue = rawDialogue;
                            
                        break;
                    
                        
                    case 6:

                        string rawAnxietyLevel = rows[row].Split(",")[column + extraCommaCounter];

                        int.TryParse(rawAnxietyLevel, out tempNarrativeData.anxietyLevel);
                            
                        break;
                    

                    case 7:

                        string rawlinkID;
                        
                        // Next dialogue link ID
                        // rawlinkID = rows[row].Split(",")[column];
                        // print($"COUNTER : {extraCommaCounter} Line {row}");
                        rawlinkID = rows[row].Split(",")[column + extraCommaCounter];
                        

                        if (int.TryParse(rawlinkID, out tempNarrativeData.linkId))
                        {
                            print($"Line {row} : IS LINKED TO LINE {tempNarrativeData.linkId}");
                        }
                        else
                        {
                            print($"Line {row} : IS LINKED TO THE NEXT LINE");
                            tempNarrativeData.linkId = -1;
                        }

                        // Reset the counter for the next row
                        extraCommaCounter = 0;
                        
                        break;
                }
            }

            dataLink[row] = new DataLink
            {
                rawData = rows[row],
                narrativeData = tempNarrativeData
            };
        }
    }

    // Gets the position of the character for the current column
    public int GetColumnCharacterIndex(string rawRowData, int column)
    {
        int commaCounter = 0;

        for (int i = 0; i < rawRowData.Length; i++)
        {
            if (rawRowData[i].ToString() == ",")
            {
                commaCounter++;
            }

            // print($"COLUMN : {commaCounter}, {column}");
            // Return the position of the comma at the given column
            if (commaCounter == column)
            {
                return i;
            }
        }

        return -1;
    }

    // Returns the text between double quotes
    public string ReturnTextFromQuotes(string rawRowData, int startIndex, int column)
    {
        int doubleQuotes = 0;
        string rawDialogue = "";
        for (int j = startIndex + 1; j < rawRowData.Length; j++)
        {
            if (rawRowData[j].ToString() == "\"")
            {
                doubleQuotes++;
                j++;
            }

            // Start of dialogue
            if (doubleQuotes == 1)
            {
                if (rawRowData[j].ToString() == ",")
                {
                    extraCommaCounter++;
                }
                // print($"{rawRowData[j]}, COUNTER : {extraCommaCounter} : {column}");
                                        
                rawDialogue += rawRowData[j].ToString();
            }
            // End of dialogue
            else if(doubleQuotes == 2)
            {
                break;
            }
        }
        
        return rawDialogue;
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