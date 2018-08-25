using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : MonoBehaviour {

    public List<Equipment> linkedEquipment;

    public float transferRate = 0.5f;

    public float armor = 50f;

    public bool playBettySound = true;
    string bettySound = "ArmorIneffective";
    public List<string> sounds;
    int soundsCount = 6;

    public float soundVolume = 1f;

    bool stripped;
    [Tooltip("The damage particle effects are activated when armor reaches 0.")]
    public List<GameObject> damageEffects;

	// Use this for initialization
	void Awake () {
        var rg = GetComponent<Rigidbody>();
        if (rg != null) rg.isKinematic = true;
        if (sounds.Count == 0)
        {
            for (int i = 0; i < soundsCount; i++)
            {
                sounds.Add("Explosion" + (i + 1));
            }
        }
        if (damageEffects.Count != 0)
        {
            for (int i = 0; i < damageEffects.Count; i++)
            {
                damageEffects[i].SetActive(false);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToThis(Equipment script)
    {
        if (!linkedEquipment.Contains(script))
        {
            linkedEquipment.Add(script);
        }
    }

    public void RemoveFromThis(Equipment script)
    {
        if (linkedEquipment.Contains(script))
        {
            linkedEquipment.Remove(script);
        }
    }



    public void ToggleRagdoll()
    {
        Rigidbody rg = GetComponent<Rigidbody>();
        if (rg == null)
        {
            rg = gameObject.AddComponent<Rigidbody>();
            rg.useGravity = true;
        }
        rg.isKinematic = false;
        if (rg != null)
        {
            rg.isKinematic = false;
            gameObject.layer = 9;
        }
        if (linkedEquipment.Count != 0)
        {
            for (int i = 0; i < linkedEquipment.Count; i++)
            {
                Destroy(linkedEquipment[i].gameObject);
            }
        }

    }

    public void ReduceHealth(float damage, bool punchThrough)
    {
        
        if (armor > 0)
        {
            armor -= damage;
        } 
        if (armor <= 0)
        {
            if (!stripped)
            {
                stripped = true;
                if (damageEffects.Count != 0)
                {
                    for (int i = 0; i < damageEffects.Count; i++)
                    {
                        damageEffects[i].SetActive(true);
                    }
                }
                if (!punchThrough)
                {
                    if (sounds.Count != 0) MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.explosionAudioFiles, sounds[Random.Range(0, sounds.Count)], soundVolume, 0.05f, transform.position);
                    if ((bettySound != "" || bettySound != null) && playBettySound)
                    {
                        MasterAudioSummoner.instance.PlayCriticalHit(bettySound, 1f, 0f, transform.position);
                    }
                }
                damage = armor * -1f;
                armor = 0;
                //play a sound?
            }
            float transferedDamage = damage * transferRate;
            if (linkedEquipment.Count != 0)
            {
                transferedDamage = transferedDamage / linkedEquipment.Count;
                for (int i = 0; i < linkedEquipment.Count; i++)
                {
                    if (linkedEquipment[i] != null)
                    {
                        linkedEquipment[i].ReduceHealth(transferedDamage, false);
                    }
                }
            }
        }
    }
}
