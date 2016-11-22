using UnityEngine;

/* Alien Slider is a kind of spawn which moves side to side as it gets closer, making it harder to hit. */

public class AlienSlider : MonoBehaviour
{

    /* How much force to add when sliding.*/
    public float slideImpulse = 10;

    /* How long to wait before sliding. */
    public float minSlideWaitTime = 1;

    private float timeStartedSliding;
    Vector2 slideDirection = Vector2.right;
    // Use this for initialization
    void Start()
    {
        timeStartedSliding = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float slideWait = minSlideWaitTime;
        // Add a sideways impulse to the projectile.

        GetComponent<Rigidbody2D>().AddForce(slideDirection * slideImpulse);
        // If it has been sliding in one direction for more than the wait time...
        if (Time.time - timeStartedSliding > slideWait)
        {
            // Switch the slide direction and reset the count.
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x / 2, GetComponent<Rigidbody2D>().velocity.y);
            slideDirection = slideDirection == Vector2.right ? Vector2.left : Vector2.right;
            timeStartedSliding = Time.time;
        }
    }

    void OnSpawnableInit()
    {

    }
}
