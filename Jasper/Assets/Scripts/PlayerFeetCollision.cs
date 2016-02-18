using UnityEngine;
using System.Collections;

public class PlayerFeetCollision : MonoBehaviour {
    [HideInInspector]
    public bool isGrounded = false;

    private Player player;

    void Awake()
    {
        player = this.transform.parent.GetComponent<Player>();
    }

    void OnTriggerStay2D(Collider2D hitObject)
    {        
        if (hitObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
        if (hitObject.CompareTag("Breakable"))
        {           
            if (player.smashing)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)this.transform.position + this.GetComponent<CircleCollider2D>().offset, .4f);
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Breakable"))
                    {
                        Destroy(hit.transform.parent.parent.gameObject);
                    }
                }                
            }
            else
            {
                isGrounded = true;
            }
        }
    }    

    void OnTriggerExit2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Terrain") || hitObject.CompareTag("Breakable"))
        {
            isGrounded = false;
        }        
    }
}