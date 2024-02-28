using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // initialize and set the default player state
    [SerializeField] 
    private PlayerStates playerStates = PlayerStates.FreeRoam;

    public bool isInteracting;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerStates)
        {
            case PlayerStates.FreeRoam:

                isInteracting = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
                // ENABLE PLAYER MOVEMENT
                transform.GetComponent<PlayerInput>().enabled = true;
                
                // ENABLE CURSOR IN EDITOR
                #if UNITY_EDITOR
                Cursor.visible = true;
                #endif
                
                break;
            
            
            case PlayerStates.Inspector:
                
                isInteracting = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
                // DISABLE PLAYER MOVEMENT
                transform.GetComponent<PlayerInput>().enabled = false;
                
                break;
            
            
            case PlayerStates.Interrogation:
                
                break;
        }
    }

    public void ChangeState(PlayerStates changeTo)
    {
        playerStates = changeTo;
    }
}
