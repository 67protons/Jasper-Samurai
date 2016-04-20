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
    private Vector2 leftSpawn, rightSpawn;
    private Player _player;

    void Awake()
    {
        base.Awake();
        leftSpawn = this.transform.FindChild("LeftSpawn").position;
        rightSpawn = this.transform.FindChild("RightSpawn").position;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }    
		
	void Update () {
        base.Update();

        if (this.currentHealth <= 500)
        {
            this.currentPhase = 2;
        }

        if (currentPhase == 1)
        {
            PhaseOne();
        }
        else if (currentPhase == 2){
            PhaseTwo();
        }
	}

    private void PhaseOne()
    {
        Shoot();
        FlipSides();
    }

    private void PhaseTwo()
    {
        WebSmash();
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

    private void WebSmash()
    {
        Debug.Log(_player.transform.position);
    }


    void FlipSides()
    {
        if ((int)this.currentHealth % 200 == 0)
        {
            this.transform.position = rightSpawn;
            this.currentDirection = Direction.Left;
        }
        else
        {
            this.transform.position = leftSpawn;
            this.currentDirection = Direction.Right;
        }
    }
}
