using UnityEngine;

// Simple class for getting information about user input
public class InputHelper
{
    // Get the position of the mouse, for aiming the gun (and possibly guiding missiles)
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 10);
        return hit.point;
    }
}

