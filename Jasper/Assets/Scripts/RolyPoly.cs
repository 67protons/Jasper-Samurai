using UnityEngine;
using System.Collections;

public class RolyPoly : Entity {

    public float damage = 10;

    void Awake()
    {
        base.Awake();
        this.currentDirection = Direction.Left;
    }

    void Update()
    {
        base.Update();
        if (this.currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        this.Move(this.currentDirection);
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            this.DealDamage(hitObject.GetComponent<Entity>(), damage);
        }
        else if (hitObject.CompareTag("Terrain") || hitObject.CompareTag("Enemy"))
        {            
            currentDirection = this.OppositeDirection(this.currentDirection);
        }
    }
}
