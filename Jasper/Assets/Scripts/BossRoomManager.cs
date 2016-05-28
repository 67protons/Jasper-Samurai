using UnityEngine;
using System.Collections;

public class BossRoomManager : MonoBehaviour {

    public SpiderBoss bossScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossScript.Initialize();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossScript.Despawn();
        }
    }
}
