using UnityEngine;
using System.Collections;

public class RolyPoly : Enemy {

    public float damage = 10;
    RolyPolyFeetCollision feet;

    void Awake()
    {
        base.Awake();
        this.currentDirection = Direction.Left;
        feet = this.transform.FindChild("FeetCollider").GetComponent<RolyPolyFeetCollision>();
    }

    void FixedUpdate()
    {
        base.Update();
        if (this.currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        if (!feet.isGrounded)
        {
            this.currentDirection = this.OppositeDirection(this.currentDirection);            
        }

        this.Move(this.currentDirection);
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            this.DealDamage(hitObject.GetComponent<Entity>(), damage);
        }
        else if (hitObject.CompareTag("Terrain") || hitObject.CompareTag("Breakable"))
        {            
            currentDirection = this.OppositeDirection(this.currentDirection);
        }
    }
}
