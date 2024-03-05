using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    [SerializeField] private Transform inspectCameraLoc;
    
    private void Awake()
    {
        switch (outlineType)
        {
            case OutlineType.All:
                
                childOutlineObjs = transform.gameObject.GetComponentsInChildren<Transform>();
                
                break;
        }

        if (!inspectCameraLoc)
        {
            inspectCameraLoc = transform.GetChild(0);
        }
    }

    protected override void InteractableControls()
    {
        base.InteractableControls();
        
        print(gameObject.name + " CONTROLS");

        if (hasStartAnimation)
        {
            if (isInteracting)
            {
                GetComponent<Animator>().SetInteger("Page", 0);
            }
        }
    }

    public void EnableUI()
    {
        ui.SetActive(true);
    }
    
    public void DisabeUI()
    {
        ui.SetActive(false);
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
        interactablesManager.ReturnInspectorCamera().transform.position = inspectCameraLoc.position;
        interactablesManager.ReturnInspectorCamera().transform.rotation = inspectCameraLoc.rotation;
        
        // enabled the inspector camera
        interactablesManager.ReturnInspectorCamera().gameObject.SetActive(true);
        
        // set the priority higher than the main camera for it to transition
        interactablesManager.ReturnInspectorCamera().Priority = 50;
    }

    public void OnEndInspect()
    {
        interactablesManager.ReturnInspectorCamera().Priority = 0;
        
        if (hasStartAnimation)
        {
            if (isInteracting)
            {
                GetComponent<Animator>().SetInteger("Page", -1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // print(interactablesManager.ReturnPlayerManager().isInteracting);
        if(isInteracting)
        {
            InteractableControls();
        }
    }
}
