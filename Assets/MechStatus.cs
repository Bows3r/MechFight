using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MechStatus : MonoBehaviour {

    public float currentHeat = 0f;
    [HideInInspector]
    float baseHeat = 110f;
    [HideInInspector]
    public float heatCap;
    [HideInInspector]
    public float heatPerSink = 1.8f;
    [HideInInspector]
    float sinkDissipationRate = 0.6f;
    [HideInInspector]
    float baseDissipationRate = 1f;
    [HideInInspector]
    float damagePerHeat = 0.05f;
    [HideInInspector]
    public float heatSinkOrangeCap = 0.8f;

    [Tooltip("If you lose this, you die")]
    public Equipment engine;
    [Tooltip("Heatsinks. If you have less than minimum amount, kills you.")]
    int minimumSinkAmount = 2;
    [SerializeField]
    int currentSinkAmount;
    public List<Heatsink> heatSinks;

    Rigidbody rg;
    CharacterController controller;
    [SerializeField]
    bool cheatMech;
    public bool dead = false;

    public string deathSound = "MechDeath";

	// Use this for initialization
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Heatsink[] allSinks = GetComponentsInChildren<Heatsink>();
        foreach (Heatsink child in allSinks)
        {
            if (!heatSinks.Contains(child))
            {
                heatSinks.Add(child);
            }
        }
        currentSinkAmount = heatSinks.Count;
        if (engine == null || heatSinks.Count == 0)
        {
            cheatMech = true;
        }
    }

	void Start () {
        MapSettings.Instance.playerMechs.Add(this);
        MechWeapon[] allChildren = GetComponentsInChildren<MechWeapon>();
        foreach (MechWeapon child in allChildren)
        {
            child.parentMech = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (cheatMech || dead)
        {
            return;
        }

        //death stuff from now on
        if (engine == null && !dead)
        {
            DeathProcedures();
        }
        if (heatSinks.Count > 0)
        {
            int value = 0;
            for (int i = 0; i < heatSinks.Count; i++)
            {
                if (heatSinks[i] != null)
                {
                    value++;
                }
            }
            currentSinkAmount = value;
            heatSinks.RemoveAll(item => item == null);
        }
        else currentSinkAmount = 0;
        if (currentSinkAmount < minimumSinkAmount)
        {
            DeathProcedures();
        }

        heatCap = baseHeat + heatSinks.Count * heatPerSink;
        if (currentHeat > heatCap * heatSinkOrangeCap)
        {
            engine.ReduceHealth(currentHeat * damagePerHeat * Time.deltaTime, true);
            {
                if (currentHeat > heatCap && engine.health > 0)
                {
                    engine.ReduceHealth(currentHeat * damagePerHeat * Time.deltaTime, true);
                }
            }
        }
        currentHeat -= (baseDissipationRate + (heatSinks.Count * sinkDissipationRate)) * Time.deltaTime;
        if (currentHeat < 0)
        {
            currentHeat = 0;
        }
	}

    void DeathProcedures()
    {
        if (dead) return;
        dead = true;
        gameObject.layer = 9;

        ComponentHealth[] allParts = GetComponentsInChildren<ComponentHealth>();
        foreach (ComponentHealth child in allParts)
        {
            child.ReduceHealth(child.armor, false);
        }
        Equipment[] allEquip = GetComponentsInChildren<Equipment>();
        foreach (Equipment child in allEquip)
        {
            child.ReduceHealth(50000f, true);
        }
        /*
        Rigidbody[] allChildren = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody child in allChildren)
        {
            Destroy(child);
        }
        Equipment[] allEquip = GetComponentsInChildren<Equipment>();
        foreach (Equipment child in allEquip)
        {
            child.ReduceHealth(50000f, true);
        }
        */
        MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.explosionAudioFiles, deathSound, 1f, 0.03f, transform.position);
        controller.enabled = false;
        rg = gameObject.AddComponent<Rigidbody>();
        rg.useGravity = true;
        rg.isKinematic = false;
        var vector = Vector3.zero;
        if (Random.value < 0.5f)
        {
            vector = transform.forward;
        }
        else vector = -transform.forward;
        rg.AddForce(vector * Random.Range(8, 12), ForceMode.Impulse);
        rg.AddTorque(vector * Random.Range(8, 12), ForceMode.Impulse);
        //stuff
    }
}
