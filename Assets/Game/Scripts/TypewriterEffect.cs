using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textGUI;

    public int counter;
    public int numOfVisibleCharacters;

    public float timeBetweenCharacters = 0.05f;
    public float timeBetweenSpecialCharacters = 1.05f;

    public string currentText;
    public string previousText;
    
    // Start is called before the first frame update
    private void Awake()
    {
        // textGUI = GetComponent<TextMeshProUGUI>();
        
        textGUI.maxVisibleCharacters = 0;
        currentText = textGUI.text;
        textGUI.ForceMeshUpdate();
        
        // StartCoroutine(TypeWriter());
    }

    private void Update()
    {
        if (textGUI.text != previousText)
        {
            StopAllCoroutines();
            
            textGUI.maxVisibleCharacters = 0;
            currentText = textGUI.text;
            textGUI.ForceMeshUpdate();
            
            StartCoroutine(TypeWriter());
            previousText = currentText;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(TypeWriter());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            counter = numOfVisibleCharacters + 1;
            textGUI.maxVisibleCharacters = numOfVisibleCharacters;
        }
    }

    IEnumerator TypeWriter()
    {
        counter = 0;

        print(textGUI.textInfo.characterCount);
        numOfVisibleCharacters = textGUI.textInfo.characterCount;

        for (int i = 0; i < numOfVisibleCharacters; i++)
        {
            print(textGUI.textInfo.characterInfo[i].character);
        }

        while (counter < numOfVisibleCharacters)
        {
            if (textGUI.textInfo.characterInfo[counter].character == '.')
            {
                counter++;
                textGUI.maxVisibleCharacters = counter;
                yield return new WaitForSeconds(timeBetweenSpecialCharacters);
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenCharacters);
                counter++;
                textGUI.maxVisibleCharacters = counter;
            }
        }
    }
}