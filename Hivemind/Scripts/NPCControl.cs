using UnityEngine;

/// <summary>
/// Controllable secondary character for testing purposes.
/// <para>J-key moves left, L-key moves right and K-key enables running</para>
/// </summary>
public class NPCControl : MonoBehaviour {
    
    float dir = 0;
    RayMovement rayMovement;

    private void Awake()
    {
        rayMovement = GetComponent<RayMovement>();
    }
    
    void Update ()
    {
        if (Input.GetKey(KeyCode.J)) dir = -1;
        else if (Input.GetKey(KeyCode.L)) dir = 1;
        else dir = 0;
        
        rayMovement.CharacterInput = new Vector2(dir, 0);
        rayMovement.Run = Input.GetKey(KeyCode.K);
    }
}
