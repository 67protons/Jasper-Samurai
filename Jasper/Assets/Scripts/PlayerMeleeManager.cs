using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMeleeManager : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> enemiesInMeleeRange = new List<GameObject>();

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
}
