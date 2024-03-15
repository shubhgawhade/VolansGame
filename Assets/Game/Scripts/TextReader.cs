using System;
using System.Text;
using TMPro;
using UnityEngine;

public class TextReader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGUI;
    
    public TextAsset textAsset;
    public TextAsset textAsset2;
    // public string[] speakers;
    // public string currentSpeaker;
    public string[] currentDialogue;
    public string[] lines;

    public string[] vars;
    
    public string dialogue;
    public bool nextDialogue;
    public int dialogueTracker;

    
    public TextAsset previousTextAsset;
    
    private StringBuilder strB;
    private StringBuilder lineAdd = new();
    
    private void Awake()
    {
        // textGUI = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (textAsset != previousTextAsset)
    //     {
    //         LoadScript();
    //         previousTextAsset = textAsset;
    //         dialogueTracker = 0;
    //         NextDialogue();
    //         // ToggleUI();
    //     }
    //
    //     ParseUI();
    //
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         LoadScript();
    //
    //         NextDialogue();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         textAsset = textAsset2;
    //         // ToggleUI();
    //     }
    // }
    
    public void NextDialogue()
    {
        if (dialogueTracker == -1)
        {
            print("DIALOGUE ENDED");
            dialogueTracker = 0;
            
            // ToggleUI();
        }
        else
        {
            nextDialogue = true;
        }
    }
    
    public void ToggleUI()
    {
        textGUI.gameObject.SetActive(!textGUI.gameObject.activeSelf);
    }

    public void ParseUI(string line)
    {
        textGUI.text = "";
        lineAdd.Clear();
        currentDialogue = line.Split("$", StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in currentDialogue)
        {
            lineAdd.AppendLine(s);
            textGUI.text = lineAdd.ToString();
        }
    }

    public void ParseUI()
    {
        if (nextDialogue)
        {
            for (int i = dialogueTracker; i <= lines.Length; i++)
            {
                // currentSpeaker = speakers[i];
                
                currentDialogue = lines[i].Split("$", StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in currentDialogue)
                {
                    lineAdd.AppendLine(s);
                }

                // print(lines[i]);
                nextDialogue = false;
                dialogueTracker = i + 1;
                            
                if (dialogueTracker == lines.Length)
                {
                    nextDialogue = false;
                    dialogueTracker = -1;
                }
                
                textGUI.text = lineAdd.ToString();
                    
                if(!nextDialogue)
                {
                    lineAdd.Clear();
                    break;
                }
            }
        }
    }
    
    public void LoadScript()
    {    
        strB = new StringBuilder(textAsset.text);
        lines = strB.ToString().Split("\n");

        // dialogue = variables[0];

        // vars = new string[lines.Length];
        // for (int i = 0; i < lines.Length; i++)
        // {
        //     vars[i] = lines[i].Split('"', StringSplitOptions.RemoveEmptyEntries)[1];
        // }
        
        // speakers = new string[lines.Length];
    
        for (int i = 0; i < lines.Length; i++)
        {
            // speakers[i] = lines[i].Split(":")[0];
    
            // switch (speakers)
            // {
            //     
            // }
        }
    }
}