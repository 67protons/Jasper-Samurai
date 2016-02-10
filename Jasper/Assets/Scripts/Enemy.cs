using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    private Transform healthBar;

    public override void Awake()
    {
        base.Awake();
        healthBar = this.transform.FindChild("HealthBar").FindChild("Bar");
    }

    public override void Update()
    {
        base.Update();
        if (healthBar != null)
        {
            healthBar.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
        }    
    }
}
