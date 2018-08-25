using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestScript : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W))
        {
            anim.CrossFade("Walk_Cycle", 1f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.CrossFade("Idle", 1f);
        }
    }
}
