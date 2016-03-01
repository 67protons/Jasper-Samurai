using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    public enum Direction { Left, Right };
    //public enum Type { Player, Enemy };

    [HideInInspector]
    public Direction currentDirection = Direction.Left;

    public float maxHealth = 100f;
    [HideInInspector]
    public float currentHealth;

    public Vector2 moveSpeed;
         
    protected Vector2 stationarySpeed = new Vector2(0f, 0f);

    private SpriteRenderer sprite;    

    public virtual void Awake()
    {
        currentHealth = maxHealth;        
        sprite = this.GetComponent<SpriteRenderer>();        
    }

    public virtual void Update()
    {
        ManageState();
    }

    private void ManageState()
    {                
        if (currentDirection == Direction.Left)
            this.transform.localScale = new Vector3(-1, 1, 1);            
        else
            this.transform.localScale = new Vector3(1, 1, 1);                
    }

    public void Move(Direction directionToMove)
    {
        Vector2 totalSpeed = stationarySpeed;

        switch (directionToMove)
        {
            case Direction.Left:                
                totalSpeed.x -= moveSpeed.x;                                
                break;
            case Direction.Right:                
                totalSpeed.x += moveSpeed.x;                
                break;            
        }

        this.transform.Translate(totalSpeed * Time.deltaTime);
        currentDirection = directionToMove;
    }

    public Direction OppositeDirection(Direction direciton)
    {
        switch (direciton)
        {
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                return Direction.Left;
        }
    }

    public void DealDamage(Entity victim, float damage)
    {
        victim.currentHealth -= damage;
    }

    public void ReceiveDamage(float damage)
    {        
        this.currentHealth -= damage;
    }
}
