using UnityEngine;
using System.Collections;

/* A kind of enemy that speeds up as it flies along. */
public class AlienSprinter : MonoBehaviour {

    // What point on the screen to start speeding up
    public float YValueStartSprinting = 35;

    // How much to speed up by
    public float acceleratePerFrame = 0.05f;
	// Update is called once per frame
	void FixedUpdate () {

        Collision coli;

        // If further down the screen than the minimum value...
        if (transform.position.y > YValueStartSprinting)
        {
            // Add a bit of acceleration in the same direction as its going
            GetComponent<Rigidbody2D>().velocity *= 1 + acceleratePerFrame;
        }
	}
}
