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
    [HideInInspector]
    public bool ducking;
    [HideInInspector]
    public bool smashing;
    private bool movingLeft = false, movingRight = false;
    private SoundManager _soundManager;

    ///Charge Jump Ability
    public bool chargeJumpUnlocked = true;
    public float chargeJumpMultiplier = 100f;
    public float chargeCostPerSec = 40f;
    private float chargeJumpPotential;
    private bool charging = false;  // Prevent other movement while charging.

    ///Parry Ability
    public bool parryUnlocked = true;
    public float parryDuration = 0.5f;
    [HideInInspector]
    public bool parrying = false;
    //private float parryTimer = 0f;

    ///Dash Ability
    public bool dashUnlocked = true;
    public float dashForce = 800f;
    public float dashDuration = .4f;
    public float dashCost = 20f;
    [HideInInspector]
    public bool dashing = false;
    private bool rightTriggerDown = false;  //For controller support because Left Trigger is an axis    

    [HideInInspector]
    public Animator playerAnimator;
    PlayerMeleeManager meleeManager;
    PlayerFeetCollision feet;

    ParticleSystem particleSystem;

    public override void Awake()
    {
        base.Awake();
        currentDirection = Direction.Right;
        currentSpirit = maxSpirit;
        playerAnimator = GetComponent<Animator>();
        meleeManager = transform.FindChild("MeleeCollider").GetComponent<PlayerMeleeManager>();
        feet = transform.FindChild("FeetCollider").GetComponent<PlayerFeetCollision>();
        Physics2D.IgnoreCollision(feet.GetComponent<Collider2D>(), meleeManager.GetComponent<Collider2D>());

        ducking = false;
        smashing = false;

        particleSystem = this.transform.FindChild("ParticleSystem").GetComponent<ParticleSystem>();
        particleSystem.enableEmission = false;
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }   

    public override void Update()
    {
        base.Update();
        ManageState();
        
        ///Stationary
        if (Input.GetAxis("Horizontal") == 0 /*&& Input.GetAxis("D-Pad X Axis") == 0*/)
        {
            playerAnimator.SetBool("walking", false);
            movingLeft = false;
            movingRight = false;
        }
        else
        {
            ///Move Left
            if (Input.GetAxis("Horizontal") < 0 /*|| Input.GetAxis("D-Pad X Axis") < 0*/)
            {
                movingLeft = true;
                movingRight = false;
            }

            ///Move Right
            if (Input.GetAxis("Horizontal") > 0 /*|| Input.GetAxis("D-Pad X Axis") > 0*/)
            {
                movingRight = true;
                movingLeft = false;
            }        
        }        
        if (!ducking && (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < -0.5 /* || Input.GetAxis("D-Pad Y Axis") < -0.5*/))
        {            
            if (feet.isGrounded)
                Duck();
            else
                Smash();
        }
        if (Input.GetAxis("Vertical") == 0 /*&& Input.GetAxis("D-Pad Y Axis") == 0*/)
        {
            StandUp();
        }

        ///Charge Jump
        if (chargeJumpUnlocked && (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.JoystickButton3)))
        {
            Charge();
        }
        if (Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.Mouse2) || Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            ChargeJump();
        }

        ///Parry
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Parry();
        }

        if (!charging)  //Prevent these methods of movement while charging for ChargeJump
        {           
            ///Jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                Jump();
            }

            ///Dash
            if (dashUnlocked && (Input.GetKeyDown(KeyCode.Space) || (!rightTriggerDown && Input.GetAxis("Right Trigger") == 1)))
            {
                rightTriggerDown = true;
                Dash();
            }
            if (Input.GetAxis("Right Trigger") == 0)
            {
                rightTriggerDown = false;
            }
        }        
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Instakill"))
        {
            currentHealth = 0;
        }

        if (hitObject.CompareTag("Enemy"))
        {
            if (dashing)
            {
                DealDamage(hitObject.GetComponent<Entity>(), 100);
            }
            else if (parrying){
                hitObject.GetComponent<Entity>().EnactParry();
            }
            else
            {
                _soundManager.PlayClip(_soundManager.playerDamage);
            }
        }

        if (hitObject.CompareTag("Respawn"))
        {
            //TODO : only reset checkpoint if new checkpoint.x value is greater than old checkpoint.x value
            currentCheckpoint = hitObject.gameObject;
        }
    }

    void FixedUpdate()
    {
        if (movingLeft)
        {
            Move(Direction.Left);
            playerAnimator.SetBool("walking", true);
        }
        if (movingRight)
        {
            Move(Direction.Right);
            playerAnimator.SetBool("walking", true);
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

        ///End smashing animation when feet touch the ground
        if (feet.isGrounded)
        {
            smashing = false;
            playerAnimator.SetBool("smashing", false);
        }
        playerAnimator.SetBool("parrying", parrying);

        ///Play proper player animations for ducking, jumping, falling, and landing
        if (ducking && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("duckWalking") &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("crouchSlash"))
            playerAnimator.Play("ducking");
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("dashing") &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("slashing") &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ducking"))
        {
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.y);
            if (this.GetComponent<Rigidbody2D>().velocity.y > 2)
            {
                playerAnimator.Play("jumping");
            }
            else if (this.GetComponent<Rigidbody2D>().velocity.y < -2 && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("smashing"))
            {
                playerAnimator.Play("falling");
            }
            if ((playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("falling") ||
                playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("jumping")) && feet.isGrounded)
            {
                playerAnimator.Play("land");
            }
        }               
    }


    /* Movement helper functions below */
    private void Jump()
    {
        if (feet.isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }
    }   

    private void Duck()
    {
        ducking = true;
        moveSpeed /= 2;
        BoxCollider2D playerCollider = this.GetComponent<BoxCollider2D>();
        Vector2 normalSize = playerCollider.size;
        playerCollider.size = new Vector2(normalSize.x, normalSize.y / 2);
        playerCollider.offset -= new Vector2(0, normalSize.y / 4);
        playerAnimator.SetBool("ducking", true);
    }

    private void StandUp()
    {
        if (ducking)
        {
            ducking = false;
            moveSpeed *= 2;
            BoxCollider2D playerCollider = this.GetComponent<BoxCollider2D>();
            Vector2 smallSize = playerCollider.size;
            playerCollider.size = new Vector2(smallSize.x, smallSize.y * 2);
            playerCollider.offset += new Vector2(0, smallSize.y / 2);
            playerAnimator.SetBool("ducking", false);
        }        
    }

    private void Smash()
    {
        smashing = true;        
        playerAnimator.SetBool("smashing", true);
        playerAnimator.Play("smashing");
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2000));
    }     

    private void Charge()
    {
        if (feet.isGrounded && currentSpirit > 0)
        {
            if (particleSystem.enableEmission == false)
                _soundManager.PlayClip(_soundManager.playerChargeJump);
            playerAnimator.SetBool("charging", true);
            charging = true;
            currentSpirit -= chargeCostPerSec * Time.deltaTime;
            if (chargeJumpPotential < jumpForce)
                chargeJumpPotential = jumpForce;
            chargeJumpPotential += chargeJumpMultiplier * Time.deltaTime;

            particleSystem.enableEmission = true;            
            particleSystem.startLifetime = chargeJumpPotential/750;
        }
    }

    private void ChargeJump()
    {
        _soundManager.Stop();
        particleSystem.enableEmission = false;
        playerAnimator.SetBool("charging", false);        
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, chargeJumpPotential));
        chargeJumpPotential = 0;
        charging = false;
    }

    private void Parry()
    {
        playerAnimator.Play("parrying");
        StopCoroutine(ParryCoroutine());
        StartCoroutine(ParryCoroutine());
    }

    private IEnumerator ParryCoroutine()
    {
        parrying = true;
        MakeInvulnerable(parryDuration);
        yield return new WaitForSeconds(parryDuration);
        parrying = false;
    }

    private void Dash()
    {
        if (currentSpirit > dashCost && !ducking)
        {
            currentSpirit -= dashCost;
            playerAnimator.Play("dashing");
            _soundManager.PlayClip(_soundManager.playerDash);
            StopCoroutine(DashCoroutine());
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        dashing = true;
        MakeInvulnerable(dashDuration);
        if (currentDirection == Direction.Right)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashForce, 0f));
        }
        else if (currentDirection == Direction.Left)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-dashForce, 0f));
        }
        yield return new WaitForSeconds(dashDuration);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
        dashing = false;
    }

    /* Helper state management functions below */

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