using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    
    RaycastHit hit;
    public Interactable lastInteractableHit;
    public Interactable currentInteractable;
    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Physics.SphereCast(transform.GetChild(0).position, 
        //        10, Camera.main.transform.forward, out hit))
        // {
        //     print(hit.transform.gameObject);
        // }

        if (playerManager.isInteracting)
        {
            // STOP INTERACTING
            if(Input.GetMouseButtonDown(1))
            {
                currentInteractable.OnEndInspect();
                playerManager.ChangeState(PlayerStates.FreeRoam);
            }
            
            return;
        }
        
        // Shoot a ray from -> to
        Ray ray = new Ray(transform.GetChild(0).position, 
            Camera.main.transform.forward);
        
        // if the ray hits an object
        if (Physics.Raycast(ray, out hit, 10))
        {
            // the object hit is an interactable
            // print(hit.transform.name);
            if (hit.transform.root.GetComponent<Interactable>())
            {
                currentInteractable = hit.transform.root.GetComponent<Interactable>();
                
                currentInteractable.EnableOutline();
                
                // If player hovers over another interactable immediately
                if (lastInteractableHit && lastInteractableHit != currentInteractable)
                {
                    lastInteractableHit.DisableOutline();
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    // hit.transform.GetComponent<Interactable>().enabled = true;
                    currentInteractable.DisableOutline();
                    currentInteractable.OnInspect();
                    playerManager.ChangeState(PlayerStates.Inspector);
                }
                
                lastInteractableHit = currentInteractable;
                return;
            }
        }
        
        if (lastInteractableHit)
        {
            lastInteractableHit.DisableOutline();
        }
    }
    
    // VISUALIZE IN EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.GetChild(0).position, 
            Camera.main.transform.forward * 10);
        
        
        // Gizmos.color = Color.cyan;
        // Gizmos.DrawWireSphere(transform.GetChild(0).position, 10);
    }
}
