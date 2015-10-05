﻿using UnityEngine;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.


	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%

	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.


	//Modification
    //Déplacement aerien
	[Range(0.0f,1.0f)]
	[SerializeField] 
    public float airControl = 1.0f;			// Whether or not a player can steer while jumping;
	//Modulation de la hauteur du saut
    [SerializeField] 
    public float jumpDuration = 0.20f;			// Max effective time the player can press on the jump key
	[SerializeField] 
    float jumpForce = 15.0f;					// Amount of force added when the player jumps.	
	
    //Saut multiple
    [SerializeField] 
    int numberOfJumpInTheAir = 2;				// Number of time the player can jump while in the air a value of -1 means infinite jumps
	[SerializeField] 
    float jumpForceInTheAir = 500.0f;          // Force applied to the player when double jumping

    //Saut Mural
    [SerializeField]
    LayerMask WhatIsWall;
    [SerializeField]
    float forceWallJump = 500f;
    [SerializeField]
    float wallJumpDuration = 0.1f;
    Transform wallCheck;
    float wallRadius = 0.2f;
    
    //JetPack
    [SerializeField] 
    bool jetpackEnabled = true;
    [SerializeField]
    float forceJetpack = Physics.gravity.magnitude;



    [SerializeField]
    bool showMaxJumpHeight = true;

    bool isJumping;
    float yPositionFromJump;
	int numberOfJumpLeft;



	public bool jumpButtonDown {                                //True if the jump button has just been pressed down
		get;
		set;
	}
	public bool jumpButton{                                     //True if the jump button is down
		get; 
		set; 
	}

    void Awake()
	{
		// Setting up references.
		numberOfJumpLeft = numberOfJumpInTheAir;
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
        wallCheck = transform.Find("WallCheck");
		anim = GetComponent<Animator>();
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

        if (!isJumping)
        {
            yPositionFromJump = rigidbody2D.position[1];
        }
		if (jumpButton && grounded && !isJumping) {
            isJumping = true;
			anim.SetBool("Ground", false);
			StartCoroutine(JumpRoutine());
		}
	}

    void Update()
    {
        if(showMaxJumpHeight)
        {
            ShowMaxJumpHeight();
        }

    }

	public void Move(float move, bool crouch)
	{
		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		// Reduce the speed if crouching by the crouchSpeed multiplier
		move = (crouch ? move * crouchSpeed : move);

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(move));

		// Move the character
		float xSpeed = grounded ? move * maxSpeed : move * maxSpeed * airControl;

		rigidbody2D.velocity = new Vector2 (xSpeed, rigidbody2D.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(move > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(move < 0 && facingRight)
			// ... flip the player.
			Flip();

        if (jumpButton && Physics2D.OverlapCircle(wallCheck.position, wallRadius, WhatIsWall) && isJumping)
        {
            //Il va fallor réécrire le mouvement
        }
	}

	IEnumerator JumpRoutine()
	{
		float timer = 0;
		while (jumpButton && timer <= jumpDuration) 
        {
			float proportionCompleted = timer / jumpDuration;
			Vector2 jumpVector = Vector2.Lerp(new Vector2(0,jumpForce),Vector2.zero, proportionCompleted);
			rigidbody2D.AddForce(jumpVector);
            timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
		while (!grounded) 
        {
			if(jumpButtonDown && (numberOfJumpLeft != 0 || numberOfJumpInTheAir == -1))
			{
				numberOfJumpLeft--;
				rigidbody2D.AddForce(new Vector2(0,jumpForceInTheAir));
			} 
            else if(jumpButton && jetpackEnabled && numberOfJumpLeft == 0)
            {
                rigidbody2D.AddForce(new Vector2(0, forceJetpack));
                yield return new WaitForFixedUpdate();
            }
            yield return null;
		}

        isJumping = false;
		anim.SetBool("Ground", true);
		numberOfJumpLeft = numberOfJumpInTheAir;
	}

    public void ShowMaxJumpHeight()
    {
        //TODO
        Debug.DrawLine(new Vector3(-100, yPositionFromJump + 2, 0), new Vector3(100, yPositionFromJump + 2, 0), Color.red);
    }

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}