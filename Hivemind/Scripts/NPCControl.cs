using UnityEngine;
using UnityStandardAssets._2D;

public class NPCControl : MonoBehaviour {

    CharacterMovement m_Character;
    //PlatformerCharacter2D m_Character;
    float dir = 0;

    private void Awake()
    {
        m_Character = GetComponent<CharacterMovement>();
        //m_Character = GetComponent<PlatformerCharacter2D>();
    }
    
    void FixedUpdate ()
    {
        if (Input.GetKey(KeyCode.J)) dir = -1;
        else if (Input.GetKey(KeyCode.L)) dir = 1;
        else dir = 0;

        m_Character.Move(dir, Input.GetKey(KeyCode.K), false, false);
    }
}
