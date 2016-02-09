using UnityEngine;
using System.Collections;

public class Stopanimation : MonoBehaviour {

    public Animator a;
	// Use this for initialization
	void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (a.enabled == false)
            {
                a.enabled = true;
            }
            else
            {
                a.enabled = false;
            }
        }
    }
}
