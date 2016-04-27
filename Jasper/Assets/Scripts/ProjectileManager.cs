using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {    
    public float lifespan = 2f;
    public string projectileOwner;

    private int xDir = 1;
    private float damage;
    private float projectileForce;    
    //private Entity.Type projectileOwner;

    public void Initialize(string ownerTag, Entity.Direction direction, float dmg, float force=100f)
    {
        projectileOwner = ownerTag;
        damage = dmg;
        projectileForce = force;
        if (direction == Entity.Direction.Left)
        {
            //this.GetComponent<SpriteRenderer>().flipX = true;
            this.transform.localScale = new Vector3(-1, 1, 1);
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
        if (hitObject.CompareTag("Terrain") || hitObject.CompareTag("Breakable") || hitObject.CompareTag("Dashable"))
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
            hitObject.GetComponent<Entity>().ReceiveDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
