using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractionPerimeter : MonoBehaviour
{

    [Tooltip("Interaction perimeter radius.")]
    public float perimeterRadius = 10f;

    public Material glowMaterial;
    public Material defaultMaterial;

    // Because pivot is bottom center, this is added to transform.position to center the perimeter to character
    float perimeterCenterY = 3.5f;

    // Use this for initialization
    void Start () {
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        cc.radius = perimeterRadius;
        cc.offset = Vector2.up * perimeterCenterY;

        CharacterManager.OnCharacterChange += CharacterManager_OnCharacterChange;
        CharacterManager.OnCharacterDeath += CharacterManager_OnCharacterDeath;
    }

    void CharacterManager_OnCharacterDeath(int i)
    {
        transform.SetParent(null);
    }

    void CharacterManager_OnCharacterChange()
    {
        TryFindNewParent();
    }
    
    void Update () {
        if (transform.parent == null)
        {
            TryFindNewParent();
        }
    }

    void TryFindNewParent()
    {
        if (CharacterManager.GetCurrentCharacterObject() != null)
        {
            transform.SetParent(CharacterManager.GetCurrentCharacterObject().transform);
            transform.localPosition = Vector3.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * perimeterCenterY, perimeterRadius);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "NPC" || col.GetComponent<Trigger>() != null)
        {
            if (col.GetComponentInChildren<SpriteRenderer>())
                col.GetComponentInChildren<SpriteRenderer>().material = glowMaterial;

            if (col.GetComponent<SpriteRenderer>())
                col.GetComponent<SpriteRenderer>().material = glowMaterial;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "NPC" || col.GetComponent<Trigger>() != null || col.tag == "Player")
        {
            if (col.GetComponentInChildren<SpriteRenderer>())
                col.GetComponentInChildren<SpriteRenderer>().material = defaultMaterial;

            if (col.GetComponent<SpriteRenderer>())
                col.GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }
}
