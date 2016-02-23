using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMeleeManager : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> enemiesInMeleeRange = new List<GameObject>();

    private Player player;

    void Awake()
    {
        player = this.transform.parent.GetComponent<Player>();
    }

    //Clean up enemies that have been destroyed but might still be in the list.
    void FixedUpdate(){
        List<GameObject> toDelete = new List<GameObject>();

        foreach (GameObject enemy in enemiesInMeleeRange)
        {
            if (enemy == null)
            {
                toDelete.Add(enemy);
            }        
        }

        foreach (GameObject enemy in toDelete)
        {
            enemiesInMeleeRange.Remove(enemy);
        }
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Enemy"))
        {
            enemiesInMeleeRange.Add(hitObject.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D hitObject){
        if (hitObject.CompareTag("Enemy"))
        {
            enemiesInMeleeRange.Remove(hitObject.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D hitObject)
    {        
        if (hitObject.CompareTag("Dashable"))
        {            
            if (player.dashing)
            {
                Destroy(hitObject.transform.parent.parent.gameObject);
                //Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)this.transform.position + this.GetComponent<CircleCollider2D>().offset, .4f);
                //foreach (Collider2D hit in hits)
                //{
                //    if (hit.CompareTag("Dashable"))
                //    {
                //        Destroy(hit.transform.parent.parent.gameObject);
                //    }
                //}
            }            
        }
    }
}
