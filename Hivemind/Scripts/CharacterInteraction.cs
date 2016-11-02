using UnityEngine;

/// <summary>
/// Handles interaction with other NPC game objects
/// </summary>
public class CharacterInteraction : MonoBehaviour
{
    //Reference to our diagUI script for quick access
    public DialogueUI diagUI;
    public float reachDistance = 2.5f;

    //CharacterMovement cm;
    RayMovement rm;
    RayPlayerInput rpi;

    GameObject discussionPartner;
    RayMovement discussionPartnerRM;
    RayNPC discussionPartnerRNPC;

    [HideInInspector] public bool TryInteraction = false;

    // Remote controlled NPC check for testing purposes
    bool isControlledNPC;

    void Start()
    {
        //cm = GetComponent<CharacterMovement>();
        rpi = GetComponent<RayPlayerInput>();
        rm = GetComponent<RayMovement>();
        diagUI = FindObjectOfType<DialogueUI>();
    }

    void Update()
    {

        // If dialog is on, disable movement
        if (diagUI.dialogue.isLoaded)
        {
            //cm.AllowCharacterMovement(false);
            rm.allowMovement = false;
            rpi.enablePlayerInput = false;

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                diagUI.dialogue.EndDialogue();
            }
        }

        // Otherwise allow movement to characters involved in discussion
        if (!diagUI.dialogue.isLoaded && discussionPartner)
        {
            discussionPartner.GetComponent<RayNPC>().SetAIBehaviourActive(!isControlledNPC);
            discussionPartner.GetComponent<RayMovement>().allowMovement = true;
            discussionPartner = null;

            //cm.AllowCharacterMovement(true);
            rm.allowMovement = true;
            rpi.enablePlayerInput = true;
        }
        
        if (TryInteraction)
        {
            TryInteract();
        }

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 3), transform.right * reachDistance * rm.facingDirection, Color.red);

    }

    /// <summary>
    /// Casts a ray to see if we hit an NPC and, if so, we interact
    /// </summary>
    void TryInteract()
    {
        if (discussionPartner)
        {
            DoInteraction();
            return;
        }

        // Multi ray
        RaycastHit2D[] rHit = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 3), Vector2.right * rm.facingDirection, reachDistance, -1);
        if (rHit.Length > 0)
        {
            foreach (RaycastHit2D hit in rHit)
            {
                if (hit.collider.tag == "NPC")
                {
                    // Check for a ghost object
                    if (hit.collider.name.StartsWith("Ghost"))
                    {
                        // Get the ghost's original from the character pair list - not needed if ghost is original's child
                        //CharacterPair cp = FindObjectOfType<GhostManager>().characters.Find(c => c.Ghost == hit.collider.gameObject);

                        discussionPartner = hit.collider.transform.parent.gameObject;
                    }
                    else
                    {
                        discussionPartner = hit.collider.gameObject;
                    }

                    if (!discussionPartner.GetComponent<VIDE_Assign>()) return;

                    discussionPartnerRM = discussionPartner.GetComponent<RayMovement>();
                    discussionPartnerRNPC = discussionPartner.GetComponent<RayNPC>();
                    isControlledNPC = discussionPartner.GetComponent<NPCControl>();

                    discussionPartnerRM.allowMovement = false;
                    discussionPartnerRM.FaceTarget(GetComponentInChildren<SpriteRenderer>());
                    discussionPartnerRNPC.SetAIBehaviourActive(false);

                    DoInteraction();

                    // Break the loop so that only one conversation is active in case many NPC's got raycasted
                    break;
                }
            }
        }
    }

    void DoInteraction()
    {
        //Lets grab the NPC's DialogueAssign script...
        VIDE_Assign assigned = discussionPartner.GetComponent<VIDE_Assign>();

        if (!diagUI.dialogue.isLoaded)
        {
            //... and use it to begin the conversation
            diagUI.Begin(assigned);
        }
        else
        {
            //If conversation already began, let's just progress through it
            diagUI.NextNode();
        }
        diagUI.npcName.text = discussionPartner.name;

    }

    void OnDestroy()
    {
        if (diagUI && diagUI.dialogue)
            diagUI.dialogue.EndDialogue();
    }
}
