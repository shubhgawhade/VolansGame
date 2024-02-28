using System;
using UnityEngine;

public class Interactable : InteractablesManager
{
    [SerializeField] private InteractablesManager interactablesManager;
    
    public enum OutlineType
    {
        All,
        Custom
    }

    public OutlineType outlineType = OutlineType.All;

    [SerializeField] public Transform[] childOutlineObjs;
    
    private Transform cameraLoc;
    
    private void Awake()
    {
        switch (outlineType)
        {
            case OutlineType.All:
                
                childOutlineObjs = transform.gameObject.GetComponentsInChildren<Transform>();
                
                break;
        }
        
        cameraLoc = transform.GetChild(0);
        
        InteractableControls();
    }

    protected override void InteractableControls()
    {
        base.InteractableControls();
        
        print(gameObject.name + " CONTROLS");
    }

    public void EnableOutline()
    {
        switch (outlineType)
        {
            case OutlineType.All:
                        
                foreach (Transform childObj in childOutlineObjs)
                {
                    childObj.gameObject.layer = 7;
                }
                        
                break;
                    
                    
            case OutlineType.Custom:
                
                foreach (Transform childObj in childOutlineObjs)
                {
                    childObj.gameObject.layer = 7;
                }
                        
                break;
        }
    }
    
    public void DisableOutline()
    {
        switch (outlineType)
        {
            case OutlineType.All:
                        
                foreach (Transform childObj in childOutlineObjs)
                {
                    childObj.gameObject.layer = 0;
                }
                        
                break;
                    
                    
            case OutlineType.Custom:
                
                foreach (Transform childObj in childOutlineObjs)
                {
                    childObj.gameObject.layer = 0;
                }
                        
                break;
        }
    }

    public void OnInspect()
    {
        // set the inspector camera to the viewpoint
        interactablesManager.ReturnInspectorCamera().transform.position = cameraLoc.position;
        interactablesManager.ReturnInspectorCamera().transform.rotation = cameraLoc.rotation;
        
        // enabled the inspector camera
        interactablesManager.ReturnInspectorCamera().gameObject.SetActive(true);
        
        // set the priority higher than the main camera for it to transition
        interactablesManager.ReturnInspectorCamera().Priority = 50;
    }

    public void OnEndInspect()
    {
        interactablesManager.ReturnInspectorCamera().Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
