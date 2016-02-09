using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacks : MonoBehaviour {
    public GameObject projectilePrefab;
    public float slashDamage = 100f;
    public float projectileDamage = 20f;
    public float projectileForce = 400f;
    public float projectileCost = 5f;

    private Player state;
    private List<GameObject> enemiesInMeleeRange = new List<GameObject>();
    private bool slashing = false;  //For controller support because Right Trigger is an axis

    void Start()
    {        
        state = this.GetComponent<Player>();        

        enemiesInMeleeRange = transform.GetComponentInChildren<PlayerMeleeManager>().enemiesInMeleeRange;
    }

	void Update () 
    {        
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0) || (!slashing && Input.GetAxis("Right Trigger") == 1))
        {            
            slashing = true;            
            StopCoroutine(Slash());
            StartCoroutine(Slash());
        }
        if (Input.GetAxis("Right Trigger") == 0)
        {
            slashing = false;            
        }
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {            
            Shoot();                      
        }
	}

    private IEnumerator Slash()
    {        
        state.playerAnimator.Play("slashing");
        yield return new WaitForSeconds(.25f);
        foreach (GameObject enemy in enemiesInMeleeRange){            
            state.DealDamage(enemy.GetComponent<Entity>(), 100);
        }        
    }

    private void Shoot()
    {
        if (state.currentSpirit > projectileCost)
        {
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            projectile.GetComponent<ProjectileManager>().Initialize(this.tag, state.currentDirection, projectileDamage, projectileForce);

            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), transform.FindChild("MeleeCollider").GetComponent<Collider2D>());

            state.currentSpirit -= projectileCost;
        }
    }
}
