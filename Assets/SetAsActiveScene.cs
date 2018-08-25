using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SetAsActiveScene : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SceneManager.SetActiveScene(gameObject.scene);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
