using UnityEngine;
using System.Collections;

public class SpiderBoss : Enemy {
    public GameObject projectilePrefab;
    public float projectileDamage = 5f;
    public float projectileForce = 400f;
    float shotFrequency = 1f;
    private float shotCooldown = 0f;
    private bool highShot = false;
    private int currentPhase = 1;
		
	void Update () {
        if (currentPhase == 1)
        {
            PhaseOne();
        }
	}

    private void PhaseOne()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (shotCooldown <= 0)
        {
            GameObject projectile = null;
            if (highShot)
            {
                projectile = (GameObject)Instantiate(projectilePrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);                
            }
            else
            {
                projectile = (GameObject)Instantiate(projectilePrefab, transform.position - new Vector3(0, .5f, 0), Quaternion.identity);
            }
            highShot = !highShot;

            projectile.GetComponent<ProjectileManager>().Initialize(this.tag, this.currentDirection, projectileDamage, projectileForce);

            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
           
            shotCooldown = shotFrequency;
        }
        else
        {
            shotCooldown -= Time.deltaTime;
        }
    }
}
