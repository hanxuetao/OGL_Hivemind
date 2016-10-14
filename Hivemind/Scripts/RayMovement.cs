using System.Collections;
using UnityEngine;
using System.Linq;

/// <summary>
/// Receives & handles input information and sends them to RayController.
/// <para>Used to move character and update sprites.</para>
/// </summary>
[RequireComponent(typeof(RayController2D))]
public class RayMovement : MonoBehaviour
{
    public bool allowMovement = true;
    [Tooltip("Normal walking speed.")]
    public float movementSpeed = 6;
    [Tooltip("When running, multiplies the movement speed with this number.")]
    public float runMultiplier = 2;

    [Space]

    public bool allowJumping = false;
    [Tooltip("Jump height in units.")]
    public float jumpHeight = 4;
    [Tooltip("Time it takes to reach jump height after pressing jump button.")]
    public float timeToJumpApex = .4f;

    [Space]

    public bool disableGravity = false;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = 0; // 0 for instant turning/stopping, .1f for smooth turning/stopping
    
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;
    Vector3 velocity;

    float verticalSlowMoveSpeed = 1.5f;
    float verticalFastMoveSpeed = 3.5f;

    [HideInInspector] public Vector2 CharacterInput;
    [HideInInspector] public bool Jump;
    [HideInInspector] public bool Run;

    [HideInInspector]
    public float facingDirection = 1; // 1 = right, -1 = left (used by CharacterInteraction)

    RayController2D controller;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        controller = GetComponent<RayController2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        if (!allowMovement)
        {
            CharacterInput = Vector2.zero;
            Run = false;
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (Jump && controller.collisions.below && allowJumping)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = CharacterInput.x * movementSpeed * (Run ? runMultiplier : 1);

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if (!disableGravity) velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (gameObject.name.Contains("RemoteControl"))
        {
            Debug.Log(velocity.x); // Unnecessary small numbers that never reach 0?
        }
        
        if ((velocity.x < -0.1 || velocity.x > 0.1) && allowMovement)
        {
            facingDirection = Mathf.Sign(velocity.x);
            spriteRenderer.flipX = (facingDirection > 0) ? false : true;
        }

        animator.SetBool("Run", Run);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    /// <summary>
    /// Turns the character to face a target character.
    /// <para>Requires sprite renderer to be attached to the same object.</para>
    /// </summary>
    /// <param name="target">Transform of the target.</param>
    public void FaceTarget(Transform target)
    {
        spriteRenderer.flipX = !target.GetComponent<SpriteRenderer>().flipX;
        facingDirection = (!spriteRenderer.flipX) ? 1 : -1;
    }

    /// <summary>
    /// Turns the character to face a target character.
    /// </summary>
    /// <param name="target">SpriteRenderer of the target.</param>
    public void FaceTarget(SpriteRenderer target)
    {
        spriteRenderer.flipX = !target.flipX;
        facingDirection = (!target.flipX) ? 1 : -1;
    }

    /// <summary>
    /// Turns the character to face a target object.
    /// </summary>
    /// <param name="target">Target object.</param>
    public void FaceTarget(Collider2D target)
    {
        spriteRenderer.flipX = target.bounds.center.x < transform.position.x;
        facingDirection = spriteRenderer.flipX ? 1 : -1;
    }

    /// <summary>
    /// Makes the character to walk downwards and towards a door.
    /// <para>Upon walking desired amount of units downwards, activates the previous level.</para>
    /// </summary>
    /// <param name="triggerObject">Object that had the DoorTrigger-script on.</param>
    /// <param name="unitsToWalkDownwards">How many units does the character walk downwards.</param>
    /// <returns></returns>
    public IEnumerator WalkToPreviousLevel(GameObject triggerObject, float unitsToWalkDownwards)
    {
        // Dangerously bad coding here. But this is just a testing ground.
        // Many of these can be removed, but can't remember which ones.
        allowMovement = false;
        Vector2 originalPos = transform.position;
        FaceTarget(triggerObject.GetComponent<Collider2D>());
        float transitionTargetX = triggerObject.transform.position.x;
        float transitionTargetY = transform.position.y - unitsToWalkDownwards;
        Vector3 transitionTarget = triggerObject.transform.root.position;
        transitionTarget.y = transform.position.y - unitsToWalkDownwards;
        Debug.Log(transitionTarget.ToString());
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().enabled = false;
        disableGravity = true;
        spriteRenderer.sortingOrder = 15;
        
        //while (transform.position.y > transitionTargetY)
        while (transform.position != transitionTarget)
        {
            animator.SetFloat("Speed", 1);
            //transform.Translate( transform.position.x, transform.position.y - unitsToWalkDownwards * Time.deltaTime, transform.position.z, Space.Self);
            //transform.Translate(Vector2.down * unitsToWalkDownwards * Time.deltaTime * verticalSmoothMoveSpeed, Space.Self);
            //transform.position += new Vector3(transform.position.x, -unitsToWalkDownwards) * Time.deltaTime * 3f;
            //transform.Translate(Vector2.down * Time.deltaTime * verticalSmoothMoveSpeed);
            //transform.Translate(transitionTargetX * Time.deltaTime * verticalSmoothMoveSpeed, transitionTargetY * Time.deltaTime * verticalSmoothMoveSpeed, 0);

            transform.position = Vector3.MoveTowards(transform.position, transitionTarget, verticalSlowMoveSpeed * Time.deltaTime);

            // Black hole gravity effect
            //float step = verticalFastMoveSpeed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, triggerObject.transform.position, step);

            //rb.MovePosition(rb.position + Vector2.down * verticalFastMoveSpeed * Time.deltaTime);

            yield return null;
        }
        GetComponent<Collider2D>().enabled = true;
        GetComponent<RayPlayerInput>().enablePlayerInput = true;
        disableGravity = false;
        triggerObject.GetComponent<DoorTrigger>().ActivateScene();
        //triggerObject.GetComponent<DoorTrigger>().LoadScene(0);
        originalPos.x = 40.74f;//FindObjectOfType<DoorTrigger>().transform.position.x;
        //triggerObject.transform.root.GetComponents(typeof(MonoBehaviour)).ToList().ForEach(s => Destroy(s));
        triggerObject.transform.root.GetComponent<Foreground>().enabled = false;
        triggerObject.transform.root.GetComponents<SpriteRenderer>().ToList().ForEach(sr => sr.color = new Color(255, 255, 255, 255));
        //Vector2 dis = triggerObject.transform.position - transform.position;
        //float originalTPosX = Mathf.Abs(dis.x);
        transform.position = originalPos;
        //triggerObject.transform.root.position = new Vector2(originalPos.x + originalTPosX, 0);
        triggerObject.transform.root.position = new Vector2(41.29f, 0);
        spriteRenderer.sortingOrder = 2;
        allowMovement = true;
    }

    /*
    public IEnumerator GoToLowerGroundLevel()
    {
        //transform.Translate(Vector2.down * Time.deltaTime * verticalFastMoveSpeed);
        yield return null;
    }

    public IEnumerator GoToHigherGroundLevel()
    {
        //transform.Translate(Vector2.up * Time.deltaTime * verticalFastMoveSpeed);
        
        yield return null;
    }
    */

    /*
    public void MoveToLowerGroundLevel()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground0"));
        Debug.Log(controller.collisionMask.value);
        int newLayer = 12;
        int layermask = 1 << newLayer;
        controller.collisionMask.value = layermask;
    }

    public void MoveToHigherGroundLevel()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground0"));
        int newLayer = 11;
        int layermask = 1 << newLayer;
        controller.collisionMask.value = layermask;
    }
    */
}