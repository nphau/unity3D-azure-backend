using UnityEngine;
using System.Collections;

public class QuaternionThinger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.DrawRay(transform.position, transform.rotation.eulerAngles);
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, Vector3.left);
    }
}
