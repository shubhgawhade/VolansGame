using Cinemachine;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineVirtualCamera inspectorCam;

    [SerializeField] protected bool hasStartAnimation;
    [SerializeField] protected bool hasUI;

    [SerializeField] protected GameObject ui;

    // private PlayerManager playerManager;
    
    
    public bool isInteracting;

    private void Awake()
    {
        // playerManager = player.GetComponent<PlayerManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected virtual void InteractableControls()
    {
        print("DEFAULT CONTROLS");
    }

    public CinemachineVirtualCamera ReturnInspectorCamera()
    {
        return inspectorCam;
    }
    
    // public PlayerManager ReturnPlayerManager()
    // {
    //     return playerManager;
    // }
}
