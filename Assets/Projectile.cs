using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float weaponRange = 100f;
    public float weaponDamage = 10f;
    public float speed = 500f;
    public float penetrationPower;
    public GameObject particleEffect;
    public string soundEffect = "";
    public GameObject owner;
    Vector3 startPos;


    // Use this for initialization
    void Awake()
    {
        startPos = transform.position;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move(Time.deltaTime);

	}
    public void Move(float dTime)
    {
        if (Vector3.Distance(startPos, transform.position) >= weaponRange)
        {
            Destroy(gameObject);
        }
        float step = speed * dTime;
        RaycastHit hit;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.forward, out hit, step, layerMask))
        {
            print("Hit, name of object; " + hit.transform.gameObject);
            transform.position += transform.forward * Vector3.Distance(hit.point, transform.position);
            //transform.position = hit.point;
            ProjectileTriggered(step);
        }

        transform.position += transform.forward * step;
    }
    
    void DoParticlesAndSound(Vector3 normal)
    {
        if (particleEffect != null)
        {
            Instantiate(particleEffect, normal, Quaternion.identity);
        }
        MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.explosionAudioFiles, soundEffect, 0.3f, 0.05f, normal);
    }

    void ProjectileTriggered(float step)
    {
        var soundPos = transform.position;
        var pos = transform.position;
        pos -= transform.forward;
        List<GameObject> alreadyHit = new List<GameObject>();
        RaycastHit[] hits;
        hits = Physics.RaycastAll(pos, transform.forward, 1f + penetrationPower + 0.1f);
        for (int i = 0; i < hits.Length; i++)
        {
            bool alreadyHitComponent = false;
            RaycastHit hit = hits[i];
            if (!alreadyHit.Contains(hit.collider.gameObject))
            {
                var script1 = hit.collider.gameObject.GetComponent<DamageRelayer>();
                if (script1 != null && !alreadyHitComponent)
                {
                    alreadyHitComponent = true;
                    script1.ReduceHealth(weaponDamage, hit.point);
                    if (soundEffect != "" && soundEffect != null)
                    {
                        soundPos = hit.point;
                    }
                }

                var script = hit.collider.gameObject.GetComponent<ComponentHealth>();
                if (script != null && !alreadyHitComponent)
                {
                    alreadyHitComponent = true;
                    script.ReduceHealth(weaponDamage, false);
                    if (soundEffect != "" && soundEffect != null)
                    {
                        soundPos = hit.point;
                    }
                }
                else
                {
                    var script2 = hit.collider.gameObject.GetComponent<Equipment>();
                    if (script2 != null)
                    {
                        script2.ReduceHealth(weaponDamage, false);
                    }
                }
                alreadyHit.Add(hit.collider.gameObject);
            }
        }
        //sounds here?
        DoParticlesAndSound(soundPos);
        Destroy(gameObject);
    }
    
}
