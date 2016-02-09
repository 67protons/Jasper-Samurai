using UnityEngine;
using System.Collections;

public class SimpleJump : MonoBehaviour {
    public float jumpspeed = 300f;
    public Vector2 jumpVector;


	// Use this for initialization
	void Start () {
	}
	
    // Update is called once per frame
	void Update () {
	if (Input.GetKeyDown(KeyCode.Space))
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpspeed));
    }
     
	}
}
