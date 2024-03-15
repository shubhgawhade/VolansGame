using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour
{
    private NarrativeReader narrativeReader;
    private StringBuilder dialogueAdd = new();
    
    public NarrativeData currentLine;
    public string[] currentDialogue;

    [SerializeField] private TextMeshProUGUI textGUI;
    [SerializeField] private GameObject dialogueChoiceButtonPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform dialogueChoicesParent;
    [SerializeField] private TextReader textReader;
    
    public int row = -1;
    public bool isWaitingForChoice;
    
    private void Awake()
    {
        narrativeReader = GetComponent<NarrativeReader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isWaitingForChoice)
        {
            Debug.LogWarning("NEXT DIALOGUE");
            
            if (currentLine.linkId == -1 || row == -1)
            {
                print($" Previous line : {row} IS LINKED TO {row + 1}");
                row++;
            }
            else
            {
                print($"Previous line : {row} IS LINKED TO {currentLine.linkId}");
                row = currentLine.linkId;
            }

            if (row >= narrativeReader.dataLink.Length)
            {
                Debug.LogError($"Line {row} DOES NOT EXIST! \nGAME OVER or WRONG ROW REFERENCE");
                
                return;
            }
            
            currentLine = narrativeReader.dataLink[row].narrativeData;
            
            NextLine();
        }
    }

    public void NextLine()
    {
        // If this line has choices
        if (currentLine.hasChoice)
        {
            ToggleUI();
            
            print($"Line {row} : Speaker {currentLine.speaker} has {currentLine.numOfChoices} choices");

            isWaitingForChoice = true;
            // row++;
            
            // Use the numOfChoices to find the next number of lines to be used as options
            // Send these lines as buttons
            for (int i = 1; i <= currentLine.numOfChoices; i++)
            {
                // row++;

                if (narrativeReader.dataLink[row + i].narrativeData.prereq == "")
                {
                    print($"Line {row + i} : Choice {i} : {narrativeReader.dataLink[row + i].narrativeData.dialogue}");

                    InstantiateDialogueChoice(i);
                }
                else
                {
                    print($"Line {row + i} : needs prerequisite : {narrativeReader.dataLink[row + i].narrativeData.prereq}  " +
                          $"\nChoice {i} : {narrativeReader.dataLink[row + i].narrativeData.dialogue}");
                    
                    // CHECK IF THE PREREQ IS MET AND THEN INSTANTIATE
                    InstantiateDialogueChoice(i);
                }
            }
        }
        else
        {
            if (currentLine.prereq != "")
            {
                print($"Line {row} : Speaker {currentLine.speaker} needs \"{currentLine.prereq}\" prerequisite knowledge");
                
                // CHECK IF THE PLAYER KNOWS PREREQ AND SET A TOGGLE "KNOWS"
            }
            else
            {
                // KNOWS = true
            }
            
            // IF THE PLAYER KNOWS
            // Send dialogue to TextReader.cs
            ParseUI($"{currentLine.speaker} : {currentLine.dialogue}");
            print($"Line {row} : Speaker {currentLine.speaker} says \"{currentLine.dialogue}\"");
        }
    }
    
    public void ToggleUI()
    {
        textGUI.text = "";
        textGUI.gameObject.SetActive(!textGUI.gameObject.activeSelf);
    }

    public void ParseUI(string line)
    {
        textGUI.text = "";
        dialogueAdd.Clear();
        currentDialogue = line.Split("$", StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in currentDialogue)
        {
            dialogueAdd.AppendLine(s);
            textGUI.text = dialogueAdd.ToString();
        }
    }

    private List<GameObject> options = new List<GameObject>();
    private void InstantiateDialogueChoice(int dialogueOption)
    {
        GameObject tempChoiceButton = null;
        OptionScroller tempOptionScroller = null;
        
        foreach (GameObject option in options)
        {
            if (!option.activeSelf)
            {
                tempChoiceButton = option;
                tempOptionScroller = tempChoiceButton.GetComponent<OptionScroller>();
                
                // tempOptionScroller.optionId = dialogueOption;
                // option.SetActive(true);
                
                break;
            }
        }

        if (!tempChoiceButton)
        {
            tempChoiceButton = Instantiate(dialogueChoiceButtonPrefab, dialogueChoicesParent);
            tempOptionScroller = tempChoiceButton.GetComponent<OptionScroller>();
            tempOptionScroller.narrativeManager = this;
            tempOptionScroller.scrollRect = scrollRect;
            
            options.Add(tempChoiceButton);
        }

        tempOptionScroller.option = narrativeReader.dataLink[row + dialogueOption].narrativeData.dialogue;
        tempOptionScroller.optionId = dialogueOption;
        tempChoiceButton.SetActive(true);
    }

    public void OptionSelected(int option)
    {
        ToggleUI();
        
        foreach (GameObject o in options)
        {
            o.SetActive(false);
        }
        
        
        isWaitingForChoice = false;
        row = narrativeReader.dataLink[row + option].narrativeData.linkId;
        
        Debug.LogWarning($"SELECTED OPTION : {option} IS LINKED TO {row}");
        
        currentLine = narrativeReader.dataLink[row].narrativeData;
        NextLine();
    }
}
