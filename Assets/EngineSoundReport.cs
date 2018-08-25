using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]

public class EngineSoundReport : MonoBehaviour {

    Equipment script;
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float currentHealth = 500f;

    bool playedSound = true;
	// Use this for initialization
    void Awake()
    {
        script = GetComponent<Equipment>();
        maxHealth = script.health / 2;
        currentHealth = script.health;
        playedSound = false;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (playedSound) return;
        currentHealth = script.health;
		if (currentHealth <= maxHealth)
        {
            playedSound = true;
            MasterAudioSummoner.instance.PlayCriticalHit("Engine", 1f, 0, transform.position);
        }
	}
}
