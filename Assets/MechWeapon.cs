using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechWeapon : MonoBehaviour {
    [Tooltip("The type of a weapon. Is it a projectile? Is it a laser beam? Use projectile or beam as names. Missile based weaponry, for example count as burst, but could also be burst fire weaponry.")]
    public string weaponType = "projectile";
    //"projectile", "burst" or "beam"
    [Tooltip("The shell count per shot. For example, if set to 5 and using burst fire, it'll fire 5 shells per burst wave.")]
    public int shellsPerShot = 1;
    [Tooltip("Prefab used for the weapon. If it's a projectile, add the projectile here. If it's a beam, add the beam here.")]
    public GameObject weaponPrefab;
    [Tooltip("The particle effect for the bullet impact.")]
    public GameObject particleEffect;
    [Tooltip("Muzzle flash effect.")]
    public GameObject muzzleFlash;
    public float weaponRange = 100f;
    public float weaponDamage = 10f;
    [Tooltip("how much units it pierces through armor")]
    public float weaponPenetration = 0.5f;
    public float weaponProjectileSpeed = 500f;
    public float weaponHeat = 5f;
    public float weaponCooldown = 3f;
    public float weaponChargeFloat = 0f;
    [Tooltip("The weapon charge up sound (for example PPC charge up sound), if you want to have one. Leave blank if you don't want one. Refer to MasterAudioSummoner for viable sounds.")]
    public string weaponChargeUp;
    [Tooltip("The weapon shoot sound, if you want to have one. Leave blank if you don't want one. Refer to MasterAudioSummoner for viable sounds.")]
    public string weaponShootSound;
    [Tooltip("The sound of hit impact. More than one on the list makes it pick a random sound.")]
    public List<string> weaponHitSound;

    [HideInInspector]
    public MechStatus parentMech;

    [Tooltip("If you start with 0, you have unlimited ammo. Enter here how many times you want the weapon to be shootable instead of the real bullet count.")]
    public int ammoCount;
    int oldAmmoCount;
    [Tooltip("If the timer is 0, never replenish")]
    public float ammoReplenishmentTimer = 60f;
    float oldReplenishmentTimer;

    //CLUSTER WEAPON STUFF
    public float weaponRandomSpread = 0f; //direction spread which the projectile can take, can also use on projectile based weapons too.
    public float weaponRandomOffsetXY = 0f; //very small number here, basically if you have a tube (like summoner pod) you want a slight spread, can also use on projectile based weapons too.
    public float burstTimer = 0.1f;
    public float oldBurstTimer;
    public int shellCount = 5;
    public int oldShellCount;

    bool ready = true;

    GameObject owner;

    void Awake()
    {
        oldShellCount = shellCount;
        oldBurstTimer = burstTimer;
        ammoCount *= shellsPerShot;
        ammoCount *= shellCount;
        oldAmmoCount = ammoCount;
        oldReplenishmentTimer = ammoReplenishmentTimer;
        shellCount = 0;

    }

    public void StartShoot(GameObject g)
    {
        if (ammoCount <= 0 && oldAmmoCount != 0) return;
        if (owner == null)
        {
            owner = g;
        }
        if (!ready)
        {
            return;
        }
        ready = false;
        MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.weaponAudioFiles, weaponChargeUp, 0.3f, 0.03f, transform.position);
        StartCoroutine(ExecuteShoot(weaponChargeFloat));
        StartCoroutine(Cooldown(weaponCooldown));
    }

    void Update()
    {
        if (oldAmmoCount != 0 && ammoCount <= 0)
        {
            if (ammoReplenishmentTimer <= 0)
            {
                ammoCount = oldAmmoCount;
                ammoReplenishmentTimer = oldReplenishmentTimer;
            } else ammoReplenishmentTimer -= Time.deltaTime;
        }
        if (weaponType == "burst" && shellCount > 0 && burstTimer <= 0f)
        {
            burstTimer += oldBurstTimer;
            shellCount--;
            for (int i = 0; i < shellsPerShot; i++)
            {
                Shoot(i);
            }
            return;
        }
        if (burstTimer > 0)
        { 
        burstTimer -= Time.deltaTime;
        }
    }
    IEnumerator ExecuteShoot(float chargeUp)
    {
        yield return new WaitForSeconds(chargeUp);
        //if (weaponShootSound != "" || weaponShootSound != null) MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.weaponAudioFiles, weaponShootSound, 0.3f, 0.03f, transform.position);
        if (weaponPrefab != null) {
            if (weaponType == "burst")
            {
                shellCount = oldShellCount -1;
                burstTimer = oldBurstTimer;
            }
            for (int i = 0; i < shellsPerShot; i++)
            {
                Shoot(i);
            }
        }
    }

    void ApplyHeat()
    {
        if (parentMech != null)
        {
            parentMech.currentHeat += weaponHeat;
        }
    }

    void Shoot(int shotCount)
    {
        if (ammoCount <= 0 && oldAmmoCount != 0) return;
        ammoCount--;
        ApplyHeat();
        if (shotCount == 0)
        {
            if (weaponShootSound != "" || weaponShootSound != null) MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.weaponAudioFiles, weaponShootSound, 0.3f, 0.03f, transform.position);
        }
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation) as GameObject;
        if (weaponType == "beam")
        {
            go.transform.parent = this.transform;
        }

        var script = go.AddComponent<Projectile>();
        if (weaponType == "projectile")
        {
        }
        if (weaponType == "projectile" || weaponType == "burst")
        {
            go.transform.parent = this.transform;
            go.transform.localEulerAngles += new Vector3(RandomNumber(weaponRandomSpread), RandomNumber(weaponRandomSpread), RandomNumber(weaponRandomSpread));
            go.transform.localPosition += new Vector3(RandomNumber(weaponRandomOffsetXY), RandomNumber(weaponRandomOffsetXY), 0);
            go.transform.parent = null;
            script.owner = owner;
            script.weaponDamage = weaponDamage;
            script.weaponRange = weaponRange;
            script.speed = weaponProjectileSpeed;
            script.penetrationPower = weaponPenetration;
            if (weaponHitSound.Count != 0)
            {
                int i = Random.Range(0, weaponHitSound.Count);
                script.soundEffect = weaponHitSound[i];
            }
            if (muzzleFlash != null)
            {
                Instantiate(muzzleFlash, transform.position, transform.rotation, transform);
            }
            if (particleEffect != null)
            {
                script.particleEffect = particleEffect;
            }
            script.Move(Time.deltaTime);

        }
        //TODO: Apply weapon heat
    }

    float RandomNumber(float f)
    {
        return Random.Range(-f, f);
    }

    IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSeconds(weaponCooldown);
        ready = true;
    }
}
