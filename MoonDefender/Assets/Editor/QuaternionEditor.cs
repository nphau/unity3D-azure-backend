using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(QuaternionThinger))]
public class QuaternionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        QuaternionThinger thing = (QuaternionThinger)target;
        Quaternion originalRotation = thing.gameObject.transform.rotation;

        DrawDefaultInspector();
        if (GUILayout.Button("Rotate 1 Quaternion"))
        {
            thing.gameObject.transform.rotation = originalRotation * new Quaternion(1, 0, 0, 0);
        }

        if (GUILayout.Button("Rotate Around Vector Left"))
        {
            thing.gameObject.transform.rotation = originalRotation * Quaternion.AngleAxis(10, Vector3.left);
        }

        if (GUILayout.Button("Rotate Around Vector Up"))
        {
            thing.gameObject.transform.rotation = originalRotation * Quaternion.AngleAxis(10, Vector3.up);
        }

        if (GUILayout.Button("Rotate Around Vector Back"))
        {
            thing.gameObject.transform.rotation = originalRotation * Quaternion.AngleAxis(10, Vector3.back);
        }

        if (GUILayout.Button("Rotate Towards Up"))
        {
            thing.gameObject.transform.rotation = Quaternion.RotateTowards(originalRotation, Quaternion.LookRotation(Vector3.up, Vector3.forward), 10);
        }

        if (GUILayout.Button("Rotate Towards Top Left"))
        {
            Vector3 topLeftPoint = new Vector3(-5, 10);
            //Quaternion.to
            
            thing.gameObject.transform.rotation = Quaternion.RotateTowards(originalRotation, Quaternion.LookRotation(topLeftPoint - thing.transform.position, Vector3.forward), 10);
        }

        if (GUILayout.Button("Rotate Towards Top Right"))
        {
            Vector3 topRightPoint = new Vector3(5, 10);
            //Quaternion.to

            thing.gameObject.transform.rotation = Quaternion.RotateTowards(originalRotation, Quaternion.LookRotation(topRightPoint - thing.transform.position,Vector3.forward), 10);
        }
    }
}
