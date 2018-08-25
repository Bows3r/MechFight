using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    public GameObject source;

	// Use this for initialization
    void Awake()
    {
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	public void Play()
    {
        if (source != null)
        {
            GameObject go = Instantiate(source, transform.position, Quaternion.identity) as GameObject;
        }
    }
}
