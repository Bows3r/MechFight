using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSettings : MonoBehaviour {

    public static MapSettings Instance;

    public float gravity = 7.81f; // old is 7.81f
    public List<MechStatus> playerMechs = new List<MechStatus>();

	// Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        playerMechs.RemoveAll(item => item == null);

    }
}
