using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
    public enum Type
    {
        Healing,
        Spirit
    }
    public Type flowerType;
    public float remainingCharge = 100f;
    public float rechagePerSec = 5f;

	void Update () {
        if (remainingCharge <= 0)
        {
            Destroy(this.gameObject);
        }
        DisplayRemainingHealth();
	}

    void OnTriggerStay2D(Collider2D hitObject)
    {        
        if (hitObject.CompareTag("Player"))
        {
            Player player = hitObject.GetComponent<Player>();
            switch (flowerType)
            {
                case Type.Healing:
                    if (player.currentHealth < player.maxHealth)
                    {
                        player.currentHealth += rechagePerSec * Time.deltaTime;
                        remainingCharge -= rechagePerSec * Time.deltaTime;
                    }
                    break;
                case Type.Spirit:
                    if (player.currentSpirit < player.maxSpirit)
                    {
                        player.currentSpirit += rechagePerSec * Time.deltaTime;
                        remainingCharge -= rechagePerSec * Time.deltaTime;
                    }
                    break;
            }
        }
    }    

    private void DisplayRemainingHealth()
    {
        GameObject text = transform.FindChild("Remaining Text").gameObject;
        text.GetComponent<TextMesh>().text = ((int)remainingCharge).ToString() + "\n";
    }
}
