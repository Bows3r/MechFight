using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float timer = 0f;
    //if timer is 0, we check clip length
    public float randomDopplerRange = 0.1f;
    AudioSource source;
    bool run;
	// Use this for initialization
    void Awake()
    {
    }

	void Start () {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
        if (source != null)
        {
            source.pitch += Random.Range(-randomDopplerRange, randomDopplerRange);
            if (timer == 0f)
            {
                timer = source.clip.length * source.pitch + 0.5f;
            }
        }
        run = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!run) return;
        if (timer <= 0)
        {
            Remove();
        }
        timer -= Time.deltaTime;
	}

    void Remove()
    {
        Destroy(gameObject);
    }
}
