using UnityEngine;
using System.Collections;

// Simple camera class with the ability to shake the camera for exciting effects.
public class DefendorCamera : MonoBehaviour {

    // Pointer to camera's start position
    private Vector3 camOrigPos;

    // Lightly shake the camera
    public void ShakeMicro()
    {
        StartCoroutine(ShakeCamera(3, 0.04f, 0.15f, 0.2f));
    }

    // Shake the camera less lightly
    public void ShakeLight()
    {
        StartCoroutine(ShakeCamera(4,0.05f,0.2f,0.4f));
    }

    // Give the camera a good shake
    public void ShakeMed()
    {
        StartCoroutine(ShakeCamera(6, 0.08f, 0.25f, 0.45f));
    }

    // Give the camera a massive shake
    public void ShakeHeavy()
    {
        StartCoroutine(ShakeCamera(8, 0.09f, 0.29f, 0.49f));
    }

    // Shake the camera with huge forcc
    public void ShakeHuge()
    {
        StartCoroutine(ShakeCamera(12, 0.11f, 0.45f, 0.51f));
    }

    // Coroutine to shake the camera
    private IEnumerator ShakeCamera(int numShakes, float freq, float xVar, float YVar)
    {
        // Pointer to how many times camera has been shook
        int shakes = 0;

        // While times shook is less than requested amount...
        while (shakes++ < numShakes)
        {
            // move the camera a random amount within the guidelines
            Camera.main.transform.position = camOrigPos;
            Camera.main.transform.position += (Random.Range(-xVar, xVar) * Vector3.up);
            Camera.main.transform.position += (Random.Range(-YVar, YVar) * Vector3.right);

            // Wait for frequency before returning to top of loop...
            yield return new WaitForSeconds(freq);
        }

        // Reset position
        Camera.main.transform.position = camOrigPos;
    }

    void Start()
    {
        // Note original position
        camOrigPos = Camera.main.transform.position;
    }
}
