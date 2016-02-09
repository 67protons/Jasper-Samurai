using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {
    
    //Public variables for design in inspector
    public GameObject currentCheckpoint;
    public int maxHealth = 50;    
    public float maxSpirit = 100f;
    public float spiritRegenPerSec = 5f;

    //Public Varibles for other scripts
    public enum Direction { Left, Right };
    [HideInInspector] 
    public float currentHealth = 50f;
    [HideInInspector]
    public float currentSpirit = 100f;
    //[HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public Direction currentDirection = Direction.Right;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //TESTING RESPAWNS
        //currentHealth -= 4*Time.deltaTime;
        //END TESTING        

        ManageState();

        if (currentHealth <= 0)
        {
            Respawn();
        }        
    }        

    private void ManageState()
    {
        if (currentDirection == Direction.Right && transform.localScale.x < 0)
        {
            Vector2 temp = transform.localScale;
            temp.x = -temp.x;
            transform.localScale = temp;
        }
        else if (currentDirection == Direction.Left && transform.localScale.x > 0)
        {
            Vector2 temp = transform.localScale;
            temp.x = -temp.x;
            transform.localScale = temp;
        }        

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        SpiritRegen();
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
