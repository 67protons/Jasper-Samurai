using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    private PlayerState state;

	void Start () {
        if (this.GetComponent<PlayerState>() != null)
        {
            state = this.GetComponent<PlayerState>();
        }
	}
		
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D hitObject)
    {
        if (hitObject.contacts[0].normal.y >= 0)
        {
            state.isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D hitObject)
    {
        if (hitObject.contacts[0].normal.y > 0)
        {
            state.isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Instakill"))
        {
            state.currentHealth = 0;
        }

        if (hitObject.CompareTag("Enemy"))
        {
            Debug.Log("Ouch");
            //TODO : hitObject.GetComponent<EnemyController>().DealDamage();
        }

        if (hitObject.CompareTag("Respawn"))
        {
            //TODO : only reset checkpoint if new checkpoint.x value is greater than old checkpoint.x value
            state.currentCheckpoint = hitObject.gameObject;
        }
    }
}
