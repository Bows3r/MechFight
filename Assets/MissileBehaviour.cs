using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {
    [Tooltip("Does the projectile have homing capabilites?")]
    public bool homing;
    public float downwardsRotateForce = -15f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles -= new Vector3(downwardsRotateForce * Time.deltaTime, 0 , 0);
	}
}
