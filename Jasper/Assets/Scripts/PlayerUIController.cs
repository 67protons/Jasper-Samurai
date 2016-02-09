using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    public Player player;
    public Text hpText;
    public Text spiritText;
    public Image hpBar;
    public Image spiritBar;
    

	void Update () {
        hpText.text = "Health: " + ((int)player.currentHealth).ToString();
        spiritText.text = "Spirit:   " + ((int)player.currentSpirit).ToString();
        hpBar.fillAmount = player.currentHealth / player.maxHealth;
        spiritBar.fillAmount = player.currentSpirit / player.maxSpirit;        
	}
}