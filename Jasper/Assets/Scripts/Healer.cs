using UnityEngine;
using System.Collections;

public class Healer : MonoBehaviour {

    public float remainingHealth = 100f;
    public float healthPerSec = 5f;

	void Update () {
        if (remainingHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        DisplayRemainingHealth();
	}

    void OnTriggerStay2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            PlayerState player = hitObject.GetComponent<PlayerState>();
            if (player.currentHealth < player.maxHealth)
            {
                player.currentHealth += healthPerSec * Time.deltaTime;
                remainingHealth -= healthPerSec * Time.deltaTime;
            }                
        }
    }

    private void DisplayRemainingHealth()
    {
        GameObject text = transform.FindChild("Health Text").gameObject;
        text.GetComponent<TextMesh>().text = ((int)remainingHealth).ToString() + "\n";
    }
}
