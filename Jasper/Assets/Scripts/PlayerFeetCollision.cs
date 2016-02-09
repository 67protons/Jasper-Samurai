using UnityEngine;
using System.Collections;

public class PlayerFeetCollision : MonoBehaviour {
    [HideInInspector]
    public bool isGrounded = false;

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Terrain"))
        {
            isGrounded = false;
        }
    }
}