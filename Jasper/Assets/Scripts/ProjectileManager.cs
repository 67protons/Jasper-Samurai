﻿using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {    
    public float lifespan = 2f;

    private int xDir = 1;
    private float damage;
    private float projectileForce;
    private string projectileOwner;
    //private Entity.Type projectileOwner;

    public void Initialize(string ownerTag, Entity.Direction direction, float dmg, float force=100f)
    {
        projectileOwner = ownerTag;
        damage = dmg;
        projectileForce = force;
        if (direction == Entity.Direction.Left)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            xDir = -1;
        }                    
    }

	void Start () {
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xDir * projectileForce, 0));
	}
		
	void Update () {
        lifespan -= Time.deltaTime;

        if (lifespan <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Terrain"))
        {
            Destroy(this.gameObject);
        }

        if (projectileOwner == "Player" && hitObject.CompareTag("Enemy"))
        {
            hitObject.GetComponent<Entity>().ReceiveDamage(damage);            
            Destroy(this.gameObject);
        }
        else if (projectileOwner == "Enemy" && hitObject.CompareTag("Player"))
        {

        }
    }
}
