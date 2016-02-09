using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
    public int movespeed = 140;
    public int computerDirection;
    Vector2 moveDirection = new Vector3(-1, 0, 0);
    bool movingLeft = false;
    public float min;
    public float max;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!movingLeft && transform.localPosition.x <= min)
        {
            moveDirection = new Vector3(1, 0, 0);
            movingLeft = true;
        }
        if (movingLeft && transform.localPosition.x >= max)
        {
            moveDirection = new Vector3(-1, 0, 0);
            movingLeft = false;
        }
        transform.Translate(movespeed * Time.deltaTime * moveDirection);
	}
}
