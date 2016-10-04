using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public GameObject projectile;

    GameObject shot;
    GameObject sporeShotSource;
    CharacterMovement _characterMovement;

    bool jump;
    
	void Start () {
        _characterMovement = GetComponent<CharacterMovement>();
        sporeShotSource = transform.FindChild("SporeShotSource").gameObject;
	}

    void Update()
    {
        jump = Input.GetButton("Jump");

        if (Input.GetButton("Fire1") && shot == null)
        {
            if (projectile) Shoot();
        }
    }
	
	void FixedUpdate ()
    {
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = Input.GetAxis("Horizontal");
        bool run = Input.GetButton("Run");
        _characterMovement.Move(h, run, crouch, jump);
        jump = false;
	}
    
    void Shoot()
    {
        // Gets mouse position from screen.
        Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 myPos = new Vector2(sporeShotSource.transform.position.x, sporeShotSource.transform.position.y);

        // Creates the projectile and sets its direction towards the mouse position.
        shot = (GameObject)Instantiate(projectile, sporeShotSource.transform.position, Quaternion.identity);
        shot.GetComponent<SporeShot>().SetDirection(target - myPos);
    }
}
