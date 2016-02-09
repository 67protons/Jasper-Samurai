using UnityEngine;
using System.Collections;
using System;

public class Player : Entity {

    public GameObject currentCheckpoint;
    public float maxSpirit = 100f;
    [HideInInspector]
    public float currentSpirit;
    public float spiritRegenPerSec = 5f;
    public float jumpForce = 500f;       

    ///Dash Ability
    public bool dashUnlocked = true;
    public float dashForce = 800f;
    public float dashDuration = .4f;
    public float dashCost = 20f;
    private bool dashing = false;   //For controller support because Left Trigger is an axis

    ///Charge Jump Ability
    public bool chargeJumpUnlocked = true;
    public float chargeJumpMultiplier = 100f;
    public float chargeCostPerSec = 40f;
    private float chargeJumpPotential = 0f;
    private bool charging = false;  // Prevent other movement while charging.

    public Animator playerAnimator;
    PlayerMeleeManager meleeManager;
    PlayerFeetCollision feet;

    public override void Awake()
    {
        base.Awake();
        currentDirection = Direction.Right;
        currentSpirit = maxSpirit;
        playerAnimator = GetComponent<Animator>();
        meleeManager = transform.FindChild("MeleeCollider").GetComponent<PlayerMeleeManager>();
        feet = transform.FindChild("FeetCollider").GetComponent<PlayerFeetCollision>();
        Physics2D.IgnoreCollision(feet.GetComponent<Collider2D>(), meleeManager.GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Instakill"))
        {
            currentHealth = 0;
        }

        if (hitObject.CompareTag("Enemy"))
        {
            Debug.Log("Ouch");
            //TODO : hitObject.GetComponent<EnemyController>().DealDamage();
        }

        if (hitObject.CompareTag("Respawn"))
        {
            //TODO : only reset checkpoint if new checkpoint.x value is greater than old checkpoint.x value
            currentCheckpoint = hitObject.gameObject;
        }
    }

    public override void Update()
    {
        base.Update();
        ManageState();
        
        ///Stationary
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("D-Pad X Axis") == 0)
        {
            playerAnimator.SetBool("walking", false);
        }
        else
        {
            ///Move Left
            if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("D-Pad X Axis") < 0)
            {
                Move(Direction.Left);
                playerAnimator.SetBool("walking", true);
            }

            ///Move Right
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("D-Pad X Axis") > 0)
            {
                Move(Direction.Right);
                playerAnimator.SetBool("walking", true);
            }        
        }

        if (!charging)  //Prevent these methods of movement while charging for ChargeJump
        {           
            ///Jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                Jump();
            }

            ///Dash
            if (dashUnlocked && (Input.GetKeyDown(KeyCode.Space) || (!dashing && Input.GetAxis("Right Trigger") == 1)))
            {
                dashing = true;
                Dash();
            }
            if (Input.GetAxis("Right Trigger") == 0)
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

    private void ManageState()
    {
        ///Respawn on Death
        if (currentHealth <= 0)
            Respawn();

        ///Recharge Spirit
        if (!charging)            
            SpiritRegen();

        ///Flip Melee Collider
        if (currentDirection == Direction.Left)
            meleeManager.transform.localScale = Vector3.left;
        else
            meleeManager.transform.localScale = Vector3.right;
    }    

    private void Jump()
    {
        if (feet.isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }        
    }

    private void Dash()
    {
        if (currentSpirit > dashCost)
        {
            currentSpirit -= dashCost;
            StopCoroutine(DashCoroutine());
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        if (currentDirection == Direction.Right)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashForce, 0f));
        }
        else if (currentDirection == Direction.Left)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-dashForce, 0f));
        }
        yield return new WaitForSeconds(dashDuration);
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void BeginChargeJump()
    {
        if (feet.isGrounded && currentSpirit > 0)
        {
            charging = true;
            currentSpirit -= chargeCostPerSec * Time.deltaTime;
            chargeJumpPotential += chargeCostPerSec * Time.deltaTime;
        }
    }

    private void ChargeJump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, (float)Math.Sqrt(chargeJumpPotential) * chargeJumpMultiplier));
        chargeJumpPotential = 0f;
        charging = false;
    }

    private void SpiritRegen()
    {
        if (currentSpirit <= maxSpirit)
        {
            currentSpirit += spiritRegenPerSec * Time.deltaTime;
        }
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        currentSpirit = maxSpirit;
        Vector2 spawnPoint = currentCheckpoint.transform.FindChild("Respawn Location").position;
        this.transform.position = spawnPoint;
    }
}


//void OnCollisionStay2D(Collision2D hitObject)
//{
//    if (hitObject.contacts[0].normal.y > 0)
//    {
//        isGrounded = true;
//    }
//}

//void OnCollisionExit2D(Collision2D hitObject)
//{
//    if (hitObject.contacts[0].normal.y > 0)
//    {
//        isGrounded = false;
//    }
//}