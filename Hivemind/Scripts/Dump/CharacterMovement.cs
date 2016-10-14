using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float runMultiplier = 2.0f;
    [Range(0, 1)] [SerializeField] float crouchMultiplier = .4f;
    [SerializeField] bool allowJump = false;
    [SerializeField] float jumpForce = 200.0f;
    [SerializeField] LayerMask m_WhatIsGround;

    Rigidbody2D _rigidBody2D;
    Animator _animator;

    Transform _ceilingCheck;
    Transform _groundCheck;

    bool allowMovement = true;
    bool _grounded;
    bool _facingRight = true;

    const float _ceilingRadius = .1f;
    const float _groundedRadius = .25f;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _groundCheck = transform.Find("GroundCheck");
        _ceilingCheck = transform.Find("CeilingCheck");
    }

    private void FixedUpdate()
    {
        _grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                _grounded = true;
        }
        _animator.SetBool("Ground", _grounded);
        
        _animator.SetFloat("vSpeed", _rigidBody2D.velocity.y);
    }

    public void Move(float move, bool run, bool crouch, bool jump)
    {
        //if (move == 0 && _rigidBody2D.velocity.x > 2) move = Mathf.Sign(_rigidBody2D.velocity.x); 

        if (!allowMovement)
        {
            move = 0;
            run = false;
            crouch = false;
            jump = false;
        }

        // Update crouching stuff
        if (crouch && !_animator.GetBool("Crouch"))
            if (Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingRadius, m_WhatIsGround))
                crouch = true;
        _animator.SetBool("Crouch", crouch);

        if (_grounded) // || allowAirControl)
        {
            // Set speed based on crouch/run
            move = (crouch ? move * crouchMultiplier : move);
            move = (run ? move * runMultiplier : move);

            // Update animator
            _animator.SetBool("Run", run);
            _animator.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            _rigidBody2D.velocity = new Vector2(move * moveSpeed, _rigidBody2D.velocity.y);

            // Get facing direction if stationary
            if (move != 0)
            {
                if (transform.localScale.x > 0) _facingRight = true;
                else _facingRight = false;
            }
            
            // Flip character based on movement and facing direction
            if (move > 0 && !_facingRight)
                Flip();
            else if (move < 0 && _facingRight)
                Flip();
        }

        // Jump
        if (_grounded && jump && _animator.GetBool("Ground") && allowJump)
        {
            // Add a vertical force to the player.
            _grounded = false;
            _animator.SetBool("Ground", false);
            _rigidBody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }
    
     void Flip()
    {
        _facingRight = !_facingRight;
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void AllowCharacterMovement(bool allow)
    {
        this.allowMovement = allow;
    }
}
