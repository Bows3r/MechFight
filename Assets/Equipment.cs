using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {

    public bool PlayBettySounds = true;

    public float health = 10f;
    [Tooltip("The amount of damage transferred to the owning component on death.")]
    public float damageToComponent = 0f;
    
    [Tooltip("Do we detach the parent of this component on death? Arm falling off etc.")]
    public bool detach;
    [Tooltip("Particle effect played on death.")]
    public GameObject particleEffect;

    public string sound = "ComponentSmall";

    public string specificComponentSound = "";

    public ComponentHealth parentScript;


    public MechMovementScript moveScript;
    float speedCripple;
    public float speedCripplePercent = 0.5f;
    float turnCripple;
    public float turnCripplePercent = 0.5f;

    public float soundVolume = 0.5f;

    Transform owner;

    bool dead = false;

    // Use this for initialization
    void Awake()
    {
        if (parentScript == null) parentScript = transform.parent.GetComponent<ComponentHealth>();
        if (parentScript != null) parentScript.AddToThis(this);
        if (detach)
        {
            owner = parentScript.transform;
        }
        if (moveScript != null)
        {
            speedCripple = moveScript.maxSpeed * speedCripplePercent;
            turnCripple = moveScript.turnSpeed * turnCripple;
        }
    }

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReduceHealth(float damage, bool punchThrough)
    {
        if (parentScript.armor > Mathf.Epsilon && !punchThrough)
        {
            return;
        }
        health -= damage;
        //audio?
        if (health <= 0)
        {
            if (dead == false)
            {
                dead = true;
                for (int i = 0; i < parentScript.linkedEquipment.Count; i++)
                {
                    if( parentScript.linkedEquipment[i] != this)
                    {
                        parentScript.linkedEquipment[i].ReduceHealth(damageToComponent, true);
                    }
                }
            }
            if (!punchThrough)
            {
                if (particleEffect != null)
                {
                    GameObject g = Instantiate(particleEffect, transform.position, transform.rotation) as GameObject;
                }
                MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.explosionAudioFiles, sound, soundVolume, 0, transform.position);
                if ((specificComponentSound != "" || specificComponentSound == null) && PlayBettySounds)
                {
                    MasterAudioSummoner.instance.PlayCriticalHit(specificComponentSound, 1f, 0.05f, transform.position);
                }
                if (moveScript != null)
                {
                    moveScript.maxSpeed -= speedCripple;
                    moveScript.acceleration *= speedCripplePercent;
                    moveScript.turnSpeed -= turnCripple;
                    moveScript.turnAccel *= turnCripplePercent;
                }
            }
            if (detach)
            {
                owner.parent = null;
                parentScript.ToggleRagdoll();

            }
            else
            {
                parentScript.RemoveFromThis(this);
                if (this != null) Destroy(gameObject);
            }
        }
    }
}
