using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {

    private PlayerState state;  //Link the PlayerState script

    ///Basic movement variables
    public Vector2 moveSpeed;
    public float jumpForce = 300f;
    private bool movingLeft = false;
    private bool movingRight = false;
    private Vector2 stationarySpeed = new Vector2(0f, 0f);    

    ///Dash Ability
    public bool dashUnlocked = true;
    public float dashForce = 400f;
    public float dashDuration = .1f;
    public float dashCost = 20f;
    private bool dashing = false;   //For controller support because Left Trigger is an axis

    ///Charge Jump Ability
    public bool chargeJumpUnlocked = true;
    public float chargeJumpMultiplier = 100f;
    public float chargeCostPerSec = 40f;
    private float chargeJumpPotential = 0f;
    private bool charging = false;  // Prevent other movement while charging.
    //public float chargeJumpForce = 1000f;

    private Animator playerAnimator;
        

	void Start () 
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        print(playerAnimator);

        //DESIGN : Changing variables for smoother gameplay
        moveSpeed /= 10;
        if (this.GetComponent<PlayerState>() != null)
        {
            state = this.GetComponent<PlayerState>();
        }



        playerAnimator = GetComponent<Animator>();
	}
		
	void Update () 
    {
        


        ///Move Left
        if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("D-Pad X Axis") < 0)
        {
            movingLeft = true;
            movingRight = false;
            state.currentDirection = PlayerState.Direction.Left;
            playerAnimator.SetBool("walking", true);
        }
        if (Input.GetAxis("Horizontal") >= 0 && Input.GetAxis("D-Pad X Axis") >= 0)
            movingLeft = false;

        ///Move Right
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("D-Pad X Axis") > 0)
        {
            movingRight = true;
            movingLeft = false;
            state.currentDirection = PlayerState.Direction.Right;
            playerAnimator.SetBool("walking", true);
        }
        if (Input.GetAxis("Horizontal") <= 0  && Input.GetAxis("D-Pad X Axis") <= 0)
            movingRight = false;

        if(movingRight == false && movingLeft == false)
        {
            playerAnimator.SetBool("walking", false);
        }

        if (!charging)  //Prevent these methods of movement while charging for ChargeJump
        {            
            ///Jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                Jump();
            }

            ///Dash
            if (dashUnlocked && (Input.GetKeyDown(KeyCode.Space) || (!dashing && Input.GetAxis("Left Trigger") == 1)))
            {
                dashing = true;
                Dash();
            }
            if (Input.GetAxis("Left Trigger") == 0)
            {
                dashing = false;
            }
        }
              

        ///Charge Jump
        if (chargeJumpUnlocked && (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.JoystickButton3)))
        {
            BeginChargeJump();
        }
        if (Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.Mouse2) || Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            ChargeJump();
        }
	}

    void FixedUpdate()
    {
        Vector2 totalSpeed = stationarySpeed;

        if (movingLeft)
        {
            totalSpeed.x += -moveSpeed.x;
        }

        if (movingRight)
        {
            totalSpeed.x += moveSpeed.x;
        }

        transform.Translate(totalSpeed);
    }

    private void Jump()
    {
        if (state.isGrounded)
        {
            //state.isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Dash()
    {
        if (state.currentSpirit > dashCost)
        {
            state.currentSpirit -= dashCost;
            StopCoroutine(DashCoroutine());
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        if (state.currentDirection == PlayerState.Direction.Right)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashForce, 0f));
        }
        else if (state.currentDirection == PlayerState.Direction.Left)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-dashForce, 0f));
        }
        yield return new WaitForSeconds(dashDuration);
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void BeginChargeJump()
    {
        if (state.isGrounded && state.currentSpirit > 0)
        {
            charging = true;            
            state.currentSpirit -= chargeCostPerSec * Time.deltaTime;
            chargeJumpPotential += chargeCostPerSec * Time.deltaTime;            
        }
    }

    private void ChargeJump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, (float)Math.Sqrt(chargeJumpPotential) * chargeJumpMultiplier));
        chargeJumpPotential = 0f;
        charging = false;        
    }
}
